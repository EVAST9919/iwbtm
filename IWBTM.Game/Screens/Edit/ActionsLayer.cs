using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using System.Collections.Generic;
using IWBTM.Game.Rooms.Drawables;
using osuTK;
using osu.Framework.Graphics.Shapes;
using IWBTM.Game.Helpers;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class ActionsLayer : Container
    {
        public ActionsLayer()
        {
            RelativeSizeAxes = Axes.Both;
        }

        public void UpdateActions(List<DrawableTile> tiles)
        {
            Clear();

            foreach (var tile in tiles)
            {
                if (tile.Tile.Action != null)
                {
                    Add(new EndPoint(tile));
                    Add(new ConnectionLine(tile));
                }
            }
        }

        private class ConnectionLine : CompositeDrawable
        {
            public ConnectionLine(DrawableTile tile)
            {
                var startPosition = new Vector2(tile.Tile.PositionX + tile.Size.X / 2, tile.Tile.PositionY + tile.Size.Y / 2);
                var endPosition = new Vector2(tile.Tile.Action.EndX + tile.Size.X / 2, tile.Tile.Action.EndY + tile.Size.Y / 2);

                Size = new Vector2(MathExtensions.Distance(startPosition, endPosition), 3);
                Position = startPosition;
                Rotation = MathExtensions.GetAngle(startPosition, endPosition);
                Origin = Anchor.CentreLeft;
                AddInternal(new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Red,
                        EdgeSmoothness = Vector2.One
                    }
                });
            }
        }

        private class EndPoint : CompositeDrawable
        {
            public EndPoint(DrawableTile tile)
            {
                var action = tile.Tile.Action;

                Size = tile.Size;
                Position = new Vector2(action.EndX, action.EndY);
                Masking = true;
                BorderColour = Color4.Red;
                BorderThickness = 3;
                AddInternal(new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    AlwaysPresent = true
                });
            }
        }
    }
}
