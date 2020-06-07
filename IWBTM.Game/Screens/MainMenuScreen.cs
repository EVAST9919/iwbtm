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

namespace IWBTM.Game.Screens
{
    public class MainMenuScreen : IWannaScreen
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
                    Y = -100
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
                        new Button("Create", () => this.Push(new CreationScreen()))
                    }
                },
                new SpriteText
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Text = "v.0.7.0",
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

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            //track.Start();
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);
            //track.Stop();
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

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
