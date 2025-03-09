using osuTK;

namespace IWBTM.Game.BossCreation;

public class ActiveObject
{
    public int StartTime { get; set; }

    public int Duration { get; set; }

    public int EndTime => StartTime + Duration;

    public Vector2 Position { get; set; }

    public float Rotation { get; set; }

    public Vector2 OriginPosition { get; set; }

    public Composition Parent { get; set; }
}
