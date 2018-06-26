namespace LiftEngine.Domain.Models
{
    public class StopModel
    {
        public StopModel()
        {
        }

        public StopModel(int level, int direction)
        {
            Level = level;
            Direction = direction;
        }

        public int Level { get; set; }
        public int Direction { get; set; }
    }
}
