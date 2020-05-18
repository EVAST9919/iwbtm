using IWBTM.Game.Playfield;
using IWBTM.Game.Rooms;

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
