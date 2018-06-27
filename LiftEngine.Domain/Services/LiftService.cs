using System;
using System.Linq;
using LiftEngine.Domain.Enums;
using LiftEngine.Domain.Models;

namespace LiftEngine.Domain.Services
{
    public class LiftService
    {

        public Lift Lift { get; }

        public LiftService(Lift lift)
        {
            Lift = lift;
        }

        public void Initialise(bool doorsOpen = false, int currentLevel = 0)
        {
            Lift.DoorsOpen = doorsOpen;
            if (currentLevel >= Lift.Levels.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(currentLevel), "Not a valid level");
            }
            Lift.CurrentLevel = currentLevel;
        }

        public void RequestStop(StopModel stop)
        {
            // you should travel to a stop before requesting a stop to disembark (direction == Any)
            // as the disembark request is assumed to be from someone already in the lift
            if (stop.Level < 0 || stop.Level > Lift.Levels.Length - 1)
            {
                throw new InvalidOperationException("Invalid Stop");
            }
            if (stop.Level == 0 && stop.Direction == DirectionEnum.Down)
            {
                throw new InvalidOperationException("Invalid Summons.  Cannot go down from lowest level");
            }
            if (stop.Level == Lift.Levels.Length - 1 && stop.Direction == DirectionEnum.Up)
            {
                throw new InvalidOperationException("Invalid Summons.  Cannot go up from highest level");
            }

            if (Lift.CurrentLevel == stop.Level)
            {
                // already here - open doors
                Lift.DoorsOpen = true;
                return;
            }

            AcceptStop(stop);
        }

        private void AcceptStop(StopModel stop)
        {
            switch (stop.Direction)
            {
                case DirectionEnum.Up:
                    Lift.SummonsUp.Add(stop.Level);
                    break;
                case DirectionEnum.Down:
                    Lift.SummonsDown.Add(stop.Level);
                    break;
                case DirectionEnum.Any:
                default:
                    Lift.Disembark.Add(stop.Level);
                    break;
            }
            if (Lift.CurrentDirection == DirectionEnum.Any)
            {
                // first request - set our direction
                Lift.CurrentDirection = stop.Level > Lift.CurrentLevel ? DirectionEnum.Up : DirectionEnum.Down;
            }
        }

        public void Travel()
        {
            // for efficient use of our lift 
            // we service everyone in front of us in our direction of travel before changing direction

            if (Lift.Disembark.Count == 0 && Lift.SummonsUp.Count == 0 && Lift.SummonsDown.Count == 0)
            {
                // idle
                return;
            }
            if (Lift.DoorsOpen)
            {
                Lift.DoorsOpen = false;
            }

            if (Lift.CurrentDirection == DirectionEnum.Up)
            {
                TravelToNextStopUp();
            }
            else if (Lift.CurrentDirection == DirectionEnum.Down)
            {
                TravelToNextStopDown();
            }

            // stopping going either direction services disembarks
            Lift.StopHistory.Enqueue(Lift.CurrentLevel);
            Lift.Disembark.Remove(Lift.CurrentLevel);

            if (Lift.Disembark.Count == 0 && Lift.SummonsUp.Count == 0 && Lift.SummonsDown.Count == 0)
            {
                // idle
                Lift.CurrentDirection = DirectionEnum.Any;
            }

            Lift.DoorsOpen = true;
        }

        private void TravelToNextStopDown()
        {
            // travels to next down if any
            // otherwise turns around for lowest up if any
            // otherwise resets from top to travel down

            // next level from down or disembark.  Remains at current level if no requests
            var nextLevel = Lift.SummonsDown.Where(x => x < Lift.CurrentLevel)
                .Union(Lift.Disembark.Where(x => x < Lift.CurrentLevel))
                .DefaultIfEmpty(Lift.CurrentLevel)
                .Max();

            if (Lift.CurrentLevel == nextLevel)
            {
                // no more stops below current level 
                if (Lift.SummonsUp.Count > 0 || Lift.Disembark.Count > 0)
                {
                    // travel to the lowest request for up or disembark, and turn around
                    Lift.CurrentDirection = DirectionEnum.Up;
                    Lift.CurrentLevel = Lift.SummonsUp.Union(Lift.Disembark).Max();
                    Lift.SummonsUp.Remove(Lift.CurrentLevel);
                }
                else if (Lift.SummonsDown.Count > 0)
                {
                    // travel to the highest summons for down and start up again
                    Lift.CurrentLevel = Lift.SummonsDown.Min();
                    Lift.SummonsDown.Remove(Lift.CurrentLevel);
                }
            }
            else
            {
                Lift.CurrentLevel = nextLevel;
                Lift.SummonsDown.Remove(Lift.CurrentLevel);
            }
        }

        private void TravelToNextStopUp()
        {
            // travels to next up if any
            // otherwise turns around for highest down if any
            // otherwise resets from bottom to travel up

            // next level from up or disembark.  Remains at current level if no requests
            var nextLevel = Lift.SummonsUp.Where(x => x > Lift.CurrentLevel)
                .Union(Lift.Disembark.Where(x => x > Lift.CurrentLevel))
                .DefaultIfEmpty(Lift.CurrentLevel)
                .Min();

            if (Lift.CurrentLevel == nextLevel)
            {
                // no more stops above current level 
                if (Lift.SummonsDown.Count > 0 || Lift.Disembark.Count > 0)
                {
                    // travel to the highest request for down or disembark, and turn around
                    Lift.CurrentDirection = DirectionEnum.Down;
                    Lift.CurrentLevel = Lift.SummonsDown.Union(Lift.Disembark).Max();
                    Lift.SummonsDown.Remove(Lift.CurrentLevel);
                }
                else if (Lift.SummonsUp.Count > 0)
                {
                    // travel to the lowest summons for up and start up again
                    Lift.CurrentLevel = Lift.SummonsUp.Min();
                    Lift.SummonsUp.Remove(Lift.CurrentLevel);
                }
            }
            else
            {
                Lift.CurrentLevel = nextLevel;
                Lift.SummonsUp.Remove(Lift.CurrentLevel);
            }
        }
    }
}
