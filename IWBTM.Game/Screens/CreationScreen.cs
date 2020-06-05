using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Screens.Create;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace IWBTM.Game.Screens
{
    public class CreationScreen : IWannaScreen
    {
        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private readonly NameSettingWindow nameWindow;
        private readonly MusicSettingWindow musicWindow;

        private string roomName;

        public CreationScreen()
        {
            ValidForResume = false;

            AddRangeInternal(new Drawable[]
            {
                nameWindow = new NameSettingWindow(),
                musicWindow = new MusicSettingWindow
                {
                    Alpha = 0
                }
            });

            nameWindow.OnCommit += onNameCommit;
            musicWindow.OnCommit += onMusicCommit;
        }

        private void onNameCommit(string name)
        {
            roomName = name;
            nameWindow.Hide();
            musicWindow.Show();
        }

        private void onMusicCommit(string name)
        {
            RoomStorage.CreateRoomDirectory(roomName);
            var room = RoomStorage.CreateEmptyRoom(roomName, name);
            this.Push(new EditorScreen(room, roomName));
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit? All unsaved progress will be lost.", () => base.OnExit());
        }
    }
}
