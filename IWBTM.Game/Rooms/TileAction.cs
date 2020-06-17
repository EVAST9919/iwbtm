namespace IWBTM.Game.Rooms
{
    public class TileAction
    {
        public double Time { get; set; }

        public float EndX { get; set; }

        public float EndY { get; set; }

        public TileActionType Type { get; set; } = TileActionType.Movement;
    }

    public enum TileActionType
    {
        Movement,
        Rotation
    }
}
