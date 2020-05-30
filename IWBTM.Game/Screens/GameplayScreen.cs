using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : GameScreen
    {
        protected readonly DefaultPlayfield Playfield;

        private readonly Room room;

        public GameplayScreen(Room room, string name)
        {
            this.room = room;

            ValidForResume = false;

            AddInternal(new PlayfieldAdjustmentContainer
            {
                Child = Playfield = CreatePlayfield(room, name).With(p => p.Completed = OnCompletion)
            });
        }

        protected virtual void OnCompletion(List<Vector2> deathSpots)
        {
            this.Push(new ResultsScreen(deathSpots, room));
        }

        protected virtual DefaultPlayfield CreatePlayfield(Room room, string name) => new DefaultPlayfield(room, name);
    }
}
