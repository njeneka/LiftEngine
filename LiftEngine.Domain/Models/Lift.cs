using System;
using System.Collections.Generic;
using LiftEngine.Domain.Enums;
using LiftEngine.Domain.Models;

namespace LiftEngine.Domain.Models
{
    public class Lift
    {
        public Lift()
        {
            SummonsDown = new HashSet<int>();
            SummonsUp = new HashSet<int>();
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

        public HashSet<int> SummonsUp { get; set; }
        public HashSet<int> SummonsDown { get; set; }
        public HashSet<int> Disembark { get; set; }

        public int CurrentLevel { get; set; }
        public DirectionEnum CurrentDirection { get; set; }
        public bool DoorsOpen { get; set; }

        public Queue<int> StopHistory { get; set; }
        public string StopHistoryDisplay => string.Join(",", StopHistory);
    }
}