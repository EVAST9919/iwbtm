using IWBTM.Game.Screens.Select;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using System;
using System.Linq;

namespace IWBTM.Game.Screens
{
    public class SelectScreen : IWannaScreen
    {
        private readonly Bindable<CarouselItem> selectedRoom = new Bindable<CarouselItem>();

        private Carousel carousel;
        private RoomPreviewContainer preview;

        private Action onEnterPressed;

        [BackgroundDependencyLoader]
        private void load()
        {
            onEnterPressed = () =>
            {
                if (selectedRoom.Value != default)
                    this.Push(new GameplayScreen(selectedRoom.Value.Level.Rooms.First(), selectedRoom.Value.LevelName));
            };

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Children = new Drawable[]
                {
                    carousel = new Carousel
                    {
                        Width = 0.5f,
                        OnEdit = editRequested,
                    },
                    preview = new RoomPreviewContainer
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Width = 0.5f
                    },
                    new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Child = new PlayButton(onEnterPressed.Invoke)
                    }
                }
            });

            selectedRoom.BindTo(carousel.Current);
            selectedRoom.BindValueChanged(selected => preview.Preview(selected.NewValue.Level));

            carousel.UpdateItems();
        }

        private void editRequested(CarouselItem item)
        {
            this.Push(new EditorScreen(item.Level, item.LevelName));
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            carousel.UpdateItems();
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);
            carousel.UpdateItems();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        onEnterPressed.Invoke();
                        return true;

                    case Key.Down:
                        carousel.TrySelectNext();
                        return true;

                    case Key.Up:
                        carousel.TrySelectPrev();
                        return true;
                }
            };

            return base.OnKeyDown(e);
        }

        private class PlayButton : IWannaButton
        {
            public PlayButton(Action action)
                : base("Play", action)
            {
                Size = new Vector2(100, 50);
            }
        }
    }
}
