using System.Collections.Generic;
using System.Linq;
using LiftEngine.Domain.Enums;

namespace LiftEngine.Domain.Models
{
    public class Lift
    {
        public Lift()
        {
            Disembark = new HashSet<int>();

            CurrentDirection = DirectionEnum.Any;
            StopHistory = new Queue<int>();
        }

        public Lift(int numberOfLevels) : this()
        {
            // default names for levels G, L1, L2, .... L numberOfLevels-1
            var levels = new Level[numberOfLevels];
            levels[0] = new Level("G");
            for (var level = 1; level < numberOfLevels; level++)
            {
                levels[level] = new Level($"L{level}");
            }
            Levels = levels;
        }

        public Lift(Level[] levels) : this()
        {
            Levels = levels;
        }

        // Levels (floors) in the building
        public Level[] Levels { get; }

        // levels that have summonsed down
        public HashSet<int> SummonsDown =>
            new HashSet<int>(
                Enumerable.Range(0, Levels.Length)
                    .Where(i => Levels[i].SummonsDown));

        // levels that have summonsed up
        public HashSet<int> SummonsUp =>
            new HashSet<int>(
                Enumerable.Range(0, Levels.Length)
                    .Where(i => Levels[i].SummonsUp));

        public HashSet<int> Disembark { get; set; }

        public int CurrentLevel { get; set; }
        public DirectionEnum CurrentDirection { get; set; }
        public bool DoorsOpen { get; set; }

        public Queue<int> StopHistory { get; set; }
        public string StopHistoryDisplay => string.Join(",", StopHistory);
    }
}