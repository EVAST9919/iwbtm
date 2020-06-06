using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;
using System;
using System.Collections.Generic;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : IWannaScreen
    {
        protected readonly DefaultPlayfield Playfield;

        private readonly Room room;

        private readonly float roomXBorder;
        private readonly float roomYBorder;

        public GameplayScreen(Room room, string name)
        {
            this.room = room;

            ValidForResume = false;

            AddInternal(new PlayfieldCameraContainer()
            {
                Child = Playfield = CreatePlayfield(room, name).With(p => p.Completed = OnCompletion)
            });

            roomXBorder = (room.SizeX - 25) / 2 * DrawableTile.SIZE;
            roomYBorder = (room.SizeY - 19) / 2 * DrawableTile.SIZE;
        }

        protected override void Update()
        {
            base.Update();

            var playerPosition = Playfield.Player.PlayerPosition();

            if (room.SizeX > 25)
                Playfield.X = Math.Clamp(room.SizeX / 2 * DrawableTile.SIZE - playerPosition.X, -roomXBorder, roomXBorder);

            if (room.SizeY > 19)
                Playfield.Y = Math.Clamp(room.SizeY / 2 * DrawableTile.SIZE - playerPosition.Y, -roomYBorder, roomYBorder);
        }

        protected virtual void OnCompletion(List<Vector2> deathSpots)
        {
            this.Push(new ResultsScreen(deathSpots, room));
        }

        protected virtual DefaultPlayfield CreatePlayfield(Room room, string name) => new DefaultPlayfield(room, name);
    }
}
