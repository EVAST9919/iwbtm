using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using System.Collections.Generic;
using IWBTM.Game.Rooms.Drawables;
using osuTK;
using osu.Framework.Graphics.Shapes;
using IWBTM.Game.Helpers;
using osuTK.Graphics;
using IWBTM.Game.Rooms;

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
                var action = tile.Tile.Action;

                if (action != null)
                {
                    var halfSize = Vector2.Divide(DrawableTile.GetSize(tile.Tile.Type), new Vector2(2));
                    var start = new Vector2(tile.X, tile.Y) + halfSize;
                    Vector2 end;

                    switch (action.Type)
                    {
                        case TileActionType.Movement:
                            Add(new BoxEndPoint(tile));
                            end = new Vector2(tile.Tile.Action.EndX, tile.Tile.Action.EndY) + halfSize;
                            Add(new ConnectionLine(start, end));
                            break;

                        case TileActionType.Rotation:
                            Add(new EndPoint(tile));
                            end = new Vector2(tile.Tile.Action.EndX, tile.Tile.Action.EndY);
                            Add(new ConnectionLine(start, end));
                            Add(new ConnectionCircle(tile)
                            {
                                Size = new Vector2(MathExtensions.Distance(start, end) * 2)
                            });
                            break;
                    }
                }
            }
        }

        private class ConnectionLine : CompositeDrawable
        {
            public ConnectionLine(Vector2 start, Vector2 end)
            {
                Size = new Vector2(MathExtensions.Distance(start, end), 3);
                Position = start;
                Rotation = MathExtensions.GetAngle(start, end) + 90;
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

        private class ConnectionCircle : CompositeDrawable
        {
            public ConnectionCircle(DrawableTile tile)
            {
                Origin = Anchor.Centre;
                Position = new Vector2(tile.Tile.Action.EndX, tile.Tile.Action.EndY);
                AddInternal(new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Color4.Red,
                    BorderThickness = 3,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                });
            }
        }

        private class BoxEndPoint : CompositeDrawable
        {
            public BoxEndPoint(DrawableTile tile)
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

        private class EndPoint : Circle
        {
            public EndPoint(DrawableTile tile)
            {
                var action = tile.Tile.Action;

                Masking = true;
                Size = new Vector2(10);
                Origin = Anchor.Centre;
                Position = new Vector2(action.EndX, action.EndY);
                Colour = Color4.Red;
            }
        }
    }
}
