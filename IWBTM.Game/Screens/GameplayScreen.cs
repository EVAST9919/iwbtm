using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Screens;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : GameScreen
    {
        protected readonly DefaultPlayfield Playfield;

        public GameplayScreen(Room room)
        {
            AddInternal(new PlayfieldAdjustmentContainer
            {
                Child = Playfield = new DefaultPlayfield(room)
                {
                    Completed = onCompletion
                }
            });
        }

        private void onCompletion()
        {
            this.Exit();
        }
    }
}
