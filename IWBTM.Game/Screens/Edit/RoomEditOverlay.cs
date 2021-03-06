﻿using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Create;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Edit
{
    public class RoomEditOverlay : RoomCreationOverlay
    {
        public RoomEditOverlay()
            : base(false)
        {
        }

        private Room roomToEdit;

        public void Edit(Room room)
        {
            roomToEdit = room;
            var music = room.Music;
            var skin = room.Skin;
            var completion = room.RoomCompletionType;

            SizeSetting.Current.Value = new Vector2(room.SizeX, room.SizeY);
            MusicSelector.Current.Value = string.IsNullOrEmpty(music) ? "none" : music;
            SkinSelector.Current.Value = string.IsNullOrEmpty(skin) ? "Default" : skin;
            RoomCompletionSelector.Current.Value = completion;

            Show();
        }

        protected override void Commit()
        {
            var newSize = new Vector2(SizeSetting.Current.Value.X, SizeSetting.Current.Value.Y);

            roomToEdit = new Room
            {
                Music = MusicSelector.Current.Value,
                Skin = SkinSelector.Current.Value,
                RoomCompletionType = RoomCompletionSelector.Current.Value,
                SizeX = newSize.X,
                SizeY = newSize.Y,
                Tiles = convertTiles(roomToEdit, newSize)
            };

            CreatedRoom?.Invoke(roomToEdit);
        }

        private List<Tile> convertTiles(Room room, Vector2 newSize)
        {
            var oldTiles = room.Tiles;
            var newTiles = new List<Tile>();

            foreach (var t in oldTiles)
            {
                if (t.PositionX + DrawableTile.GetSize(t.Type).X <= newSize.X * DrawableTile.SIZE)
                {
                    if (t.PositionY + DrawableTile.GetSize(t.Type).Y <= newSize.Y * DrawableTile.SIZE)
                        newTiles.Add(t);
                }
            }

            return newTiles;
        }
    }
}
