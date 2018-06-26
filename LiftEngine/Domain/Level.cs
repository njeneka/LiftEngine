namespace LiftEngine.Domain
{
    public class Level
    {
        public Level(string name, bool summonedUp = false, bool summonedDown = false, bool depart = false)
        {
            Name = name;
            SummonedUp = summonedUp;
            SummonedDown = summonedDown;
            Depart = Depart;
        }

        public string Name { get; }
        public bool SummonedUp { get; }
        public bool SummonedDown { get; }
        public bool Depart { get; }
    }
}