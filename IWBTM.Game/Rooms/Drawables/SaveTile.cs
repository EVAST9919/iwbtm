namespace IWBTM.Game.Rooms.Drawables
{
    public class SaveTile : DrawableTile
    {
        public SaveTile(Tile t)
            : base(t)
        {
        }

        public void Activate()
        {
            Scheduler.CancelDelayedTasks();
            MainSprite.Texture = Textures.Get("Tiles/save-active");
            Scheduler.AddDelayed(() => MainSprite.Texture = Textures.Get("Tiles/save"), 1000);
        }
    }
}
