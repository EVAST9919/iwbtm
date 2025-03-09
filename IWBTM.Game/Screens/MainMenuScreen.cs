using IWBTM.Game.Overlays;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using System;
using IWBTM.Game.BossCreation;

namespace IWBTM.Game.Screens
{
    public partial class MainMenuScreen : IWannaScreen
    {
        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private Track track;

        private Screen selectScreen;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddRangeInternal(new Drawable[]
            {
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "I Wanna Be The Mapper",
                    Font = FontUsage.Default.With(size: 50),
                    Y = -100,
                    Shadow = true,
                    ShadowOffset = new Vector2(0, 0.15f)
                },
                new FillFlowContainer<Button>
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(40, 0),
                    Children = new[]
                    {
                        new Button("Play", () => this.Push(consumeSelect())),
                        new Button("Create", () => this.Push(new CreationScreen())),
                        //new Button("Create Boss", () => this.Push(new BossCreationScreen()))
                    }
                },
                new SpriteText
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Text = "v.0.9.4",
                    Margin = new MarginPadding(10)
                }
            });

            audio.VolumeTrack.Value = 0.3f;
            track = audio.Tracks.Get("main menu");
            track.Looping = true;

            preloadSelectScreen();
        }

        private void preloadSelectScreen()
        {
            if (selectScreen == null)
                LoadComponentAsync(selectScreen = new SelectScreen());
        }

        private Screen consumeSelect()
        {
            var s = selectScreen;
            selectScreen = null;
            return s;
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit?", () => Game.Exit());
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            //track.Start();
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            base.OnSuspending(e);
            //track.Stop();
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);

            preloadSelectScreen();
            //track.Start();
        }

        private class Button : IWannaButton
        {
            public Button(string text, Action action)
                : base(text, action)
            {
                Size = new Vector2(100);
            }
        }
    }
}
