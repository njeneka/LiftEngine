using System.Collections.Generic;

namespace LiftEngine.Domain.Entities
{
    public class Lift
    {
        public Lift(List<Level> levels, int currentLevel = 0, bool doorsOpen = false)
        {
            Levels = levels;
            Stops = new List<int>();
            CurrentLevel = currentLevel;
            DoorsOpen = doorsOpen;
        }

        public List<Level> Levels { get; }
        public List<int> Stops { get; }
        public int CurrentLevel { get; set; }
        public bool DoorsOpen { get; set; }
    }
}