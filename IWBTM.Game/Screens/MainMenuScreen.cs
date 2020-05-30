using IWBTM.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens
{
    public class MainMenuScreen : GameScreen
    {
        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private Track track;

        public MainMenuScreen()
        {
            AddInternal(new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "IWBTM",
                Font = FontUsage.Default.With(size: 30),
                Y = -100
            });

            AddInternal(new FillFlowContainer<Button>
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5, 0),
                Children = new[]
                {
                    new Button("Play")
                    {
                        Action = () => this.Push(new SelectScreen())
                    },
                    new Button("Create")
                    {
                        Action = () => this.Push(new CreationScreen())
                    }
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            audio.VolumeTrack.Value = 0.3f;
            track = audio.Tracks.Get("main menu");
            track.Looping = true;
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
            //track.Start();
        }

        private class Button : ClickableContainer
        {
            public Button(string text)
            {
                Size = new Vector2(100);

                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new SpriteText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black
                    }
                });
            }
        }
    }
}
