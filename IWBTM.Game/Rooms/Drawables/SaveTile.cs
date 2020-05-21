namespace IWBTM.Game.Rooms.Drawables
{
    public class SaveTile : DrawableTile
    {
        public SaveTile()
            : base(TileType.Save)
        {
        }

        public void Activate()
        {
            Scheduler.CancelDelayedTasks();
            Texture = Textures.Get("Tiles/save-active");
            Scheduler.AddDelayed(() => Texture = Textures.Get("Tiles/save"), 1000);
        }
    }
}
