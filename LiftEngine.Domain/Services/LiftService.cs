using System;
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

            if (Lift.Stops.Count == 0)
            {
                // nothing in the queue - just add it
                AcceptStop(0, stop);
                return;
            }

            if (Lift.Stops.Contains(stop))
            {
                // ignore the stop if we are already stopping there
                return;
            }

            // insert the stop if we are going past in the right direction
            // or any direction (someone is disembarking)
            // otherwise add it to the end of the queue

            // insert as the first stop if possible
            if (StopBetween(stop, Lift.CurrentLevel, Lift.Stops[0]))
            {
                AcceptStop(0, stop);
                return;
            }

            for (var i = 0; i < Lift.Stops.Count - 1; i++)
            {
                if (StopBetween(stop, Lift.Stops[i].Level, Lift.Stops[i+1]))
                {
                    AcceptStop(i, stop);
                    return;
                }
            }
            AcceptStop(Lift.Stops.Count, stop);
        }

        private bool StopBetween(StopModel stop, int startLevel, StopModel nextStop)
        {
            // returns whether the stop should be inserted to separate up and down requests from the same floor
            // or the stop is on the way in the same direction as startLevel -> next stop
            // or the direction doesn't matter because the person is disembarking
            return (startLevel == nextStop.Level) ||
                (startLevel < stop.Level && stop.Level < nextStop.Level && stop.Direction != DirectionEnum.Down) ||
                (startLevel > stop.Level && stop.Level > nextStop.Level && stop.Direction != DirectionEnum.Up);
        }

        private void AcceptStop(int position, StopModel stop)
        {
            if (position == Lift.Stops.Count)
            {
                Lift.Stops.Add(stop);
            }
            else
            {
                Lift.Stops.Insert(position, stop);
            }

            var level = Lift.Levels[stop.Level];
            switch (stop.Direction)
            {
                case DirectionEnum.Up:
                    level.SummonedUp = true;
                    break;
                case DirectionEnum.Down:
                    level.SummonedDown = true;
                    break;
                default:
                    level.RequestDisembark= true;
                    break;
            }
        }

        public void Travel()
        {
            if (Lift.DoorsOpen)
            {
                Lift.DoorsOpen = false;
            }
            if (Lift.Stops.Count == 0)
            {
                return;
            }

            // move to the first stop
            Lift.CurrentLevel = Lift.Stops[0].Level;
            var level = Lift.Levels[Lift.CurrentLevel];
            Lift.StopHistory.Enqueue(Lift.CurrentLevel);

            // remove requests to stop here depending on direction
            // and open the doors
            var currentStop = Lift.Stops[0];
            Lift.Stops.RemoveAt(0);

            level.RequestDisembark = false;
            if (currentStop.Direction == DirectionEnum.Up)
            {
                level.SummonedUp = false;
            }
            if (currentStop.Direction == DirectionEnum.Down)
            {
                level.SummonedDown = false;
            }
            Lift.DoorsOpen = true;
        }

    }
}
