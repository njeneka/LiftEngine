using System;
using System.Threading;
using System.Threading.Tasks;
using LiftEngine.Domain.Entities;
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

        public void AddStop(StopModel stop)
        {
            if (stop.Level < 0 || stop.Level > Lift.Levels.Count - 1)
            {
                throw new InvalidOperationException("Invalid Stop");
            }
            if (stop.Level == 0 && stop.Direction == -1)
            {
                throw new InvalidOperationException("Invalid Summons.  Cannot go down from lowest level");
            }
            if (stop.Level == Lift.Levels.Count-1 && stop.Direction == 1)
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
                Lift.Stops.Add(stop.Level);
                return;
            }

            // insert the stop if we are going past in the right direction
            // or no direction (someone is disembarking)
            // otherwise add it to the end of the queue

            // insert as the first stop if possible
            if (StopBetween(stop, Lift.CurrentLevel, Lift.Stops[0]))
            {
                Lift.Stops.Insert(0, stop.Level);
                return;
            }

            for (var i = 0; i < Lift.Stops.Count - 1; i++)
            {
                if (StopBetween(stop, Lift.Stops[i], Lift.Stops[i+1]))
                {
                    Lift.Stops.Insert(i, stop.Level);
                    return;
                }
            }
            Lift.Stops.Add(stop.Level);
        }

        private bool StopBetween(StopModel stop, int levelA, int levelB)
        {
            // returns whether the stop is on the way in the same direction as levelA -> levelB
            // or the direction doesn't matter because the person is disembarking
            return (levelA < stop.Level && stop.Level < levelB && stop.Direction != -1) ||
                   (levelA > stop.Level && stop.Level > levelB && stop.Direction != 1);
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

            // move to the next stop
            Lift.CurrentLevel = Lift.Stops[0];
            var level = Lift.Levels[Lift.CurrentLevel];
            // remove requests to stop here depending on direction
            // and open the doors
            Lift.Stops.RemoveAt(0);

            level.Depart = false;
            if (Lift.Stops[0] > Lift.CurrentLevel)
            {
                level.SummonedUp = false;
            }
            else
            {
                level.SummonedDown = false;
            }
            Lift.DoorsOpen = true;
        }

    }
}
