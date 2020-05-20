using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;

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
            });
        }
    }
}
