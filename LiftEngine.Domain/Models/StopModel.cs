using LiftEngine.Domain.Enums;

namespace LiftEngine.Domain.Models
{
    public class StopModel
    {
        public StopModel()
        {
        }

        public StopModel(int level, DirectionEnum direction)
        {
            Level = level;
            Direction = direction;
        }

        public int Level { get; set; }
        public DirectionEnum Direction { get; set; }

    }
}
