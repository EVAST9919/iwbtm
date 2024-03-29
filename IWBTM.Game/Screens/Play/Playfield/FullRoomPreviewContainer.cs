﻿using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class FullRoomPreviewContainer : Container
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        public FullRoomPreviewContainer(Vector2 roomSize)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                FillAspectRatio = Math.Max(25, roomSize.X) / Math.Max(19, roomSize.Y),
                Child = content = new ScalingContainer(roomSize)
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }

        private class ScalingContainer : Container
        {
            private readonly Vector2 roomSize;

            public ScalingContainer(Vector2 roomSize)
            {
                this.roomSize = roomSize;
            }

            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.ChildSize.X / (Math.Max(25, roomSize.X) * DrawableTile.SIZE));
                Size = Vector2.Divide(Vector2.One, Scale);
            }
        }
    }
}
