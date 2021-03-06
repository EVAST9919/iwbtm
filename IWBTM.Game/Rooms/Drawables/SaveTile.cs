﻿namespace IWBTM.Game.Rooms.Drawables
{
    public class SaveTile : DrawableTile
    {
        public SaveTile(Tile t, string skin, bool allowEdit)
            : base(t, skin, allowEdit)
        {
        }

        public void Activate()
        {
            Scheduler.CancelDelayedTasks();
            MainSprite.Texture = Textures.Get($"Tiles/{Skin}/save-active") ?? Textures.Get("Tiles/Default/save-active");
            Scheduler.AddDelayed(() => MainSprite.Texture = Textures.Get($"Tiles/{Skin}/save") ?? Textures.Get("Tiles/Default/save"), 1000);
        }
    }
}
