namespace LiftEngine.Domain.Entities
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
        public bool SummonedUp { get; set; }
        public bool SummonedDown { get; set; }
        public bool Depart { get; set; }
    }
}