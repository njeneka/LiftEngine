namespace LiftEngine.Domain.Models
{
    public class Level
    {
        public Level(string name)
        {
            Name = name;
            SummonsDown = false;
            SummonsUp = false;
        }

        public bool SummonsUp { get; set; }
        public bool SummonsDown { get; set; }

        public string Name { get; }
    }
}