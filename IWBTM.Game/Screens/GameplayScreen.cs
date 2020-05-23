using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Screens;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : GameScreen
    {
        protected readonly DefaultPlayfield Playfield;

        private readonly Room room;

        public GameplayScreen(Room room)
        {
            this.room = room;

            ValidForResume = false;

            AddInternal(new PlayfieldAdjustmentContainer
            {
                Child = Playfield = new DefaultPlayfield(room)
                {
                    Completed = OnCompletion
                }
            });
        }

        protected virtual void OnCompletion(int deathCount)
        {
            this.Push(new ResultsScreen(deathCount, room));
        }
    }
}
