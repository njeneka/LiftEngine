namespace LiftEngine.Domain.Models
{
    public class Level
    {
        public Level(string name, bool summonedUp = false, bool summonedDown = false, bool requestDisembark = false)
        {
            Name = name;
            SummonedUp = summonedUp;
            SummonedDown = summonedDown;
            RequestDisembark = requestDisembark;
        }

        public string Name { get; }
        public bool SummonedUp { get; set; }
        public bool SummonedDown { get; set; }
        public bool RequestDisembark { get; set; }
    }
}