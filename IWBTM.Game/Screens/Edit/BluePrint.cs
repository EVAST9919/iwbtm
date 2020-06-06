﻿using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using System.Linq;
using osuTK.Input;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Edit
{
    public class BluePrint : CompositeDrawable
    {
        public readonly Bindable<TileType> Selected = new Bindable<TileType>();
        public readonly Bindable<int> SnapValue = new Bindable<int>();

        private readonly ObjectsLayer objectsLayer;
        private readonly Container hoverLayer;
        private readonly Vector2 roomSize;

        public BluePrint(Room room)
        {
            roomSize = new Vector2(room.SizeX, room.SizeY);
            Size = roomSize * DrawableTile.SIZE;

            Grid grid;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                objectsLayer = new ObjectsLayer(room),
                grid = new Grid(new Vector2(room.SizeX, room.SizeY)),
                hoverLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
            });

            grid.Current.BindTo(SnapValue);
            objectsLayer.SnapValue.BindTo(SnapValue);
        }

        public List<Tile> GetTiles()
        {
            var tiles = new List<Tile>();

            foreach (var dt in objectsLayer.Children)
                tiles.Add(dt.Tile);

            return tiles;
        }

        public bool SpawnDefined() => objectsLayer.SpawnDefined();

        public bool EndDefined() => objectsLayer.EndDefined();

        private DrawableTile tileToPlace;
        private Vector2 mousePosition;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!IsHovered)
                return false;

            mousePosition = e.MousePosition;

            if (!hoverLayer.Any())
                hoverLayer.Child = tileToPlace = createTile(new Tile { Type = Selected.Value }).With(t =>
                {
                    t.Alpha = 0.5f;
                });

            tileToPlace.Position = GetSnappedPosition(mousePosition, roomSize, SnapValue.Value);

            var buttons = e.CurrentState.Mouse.Buttons;

            if (buttons.IsPressed(MouseButton.Left))
            {
                objectsLayer.TryPlace(Selected.Value, mousePosition, roomSize);
                return true;
            }

            if (buttons.IsPressed(MouseButton.Right))
            {
                objectsLayer.TryRemove(mousePosition);
                return true;
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (IsDisposed)
                return;

            hoverLayer.Clear();
            tileToPlace?.Expire();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!IsHovered)
                return false;

            switch (e.Button)
            {
                case MouseButton.Left:
                    objectsLayer.TryPlace(Selected.Value, mousePosition, roomSize);
                    return true;

                case MouseButton.Right:
                    objectsLayer.TryRemove(mousePosition);
                    return true;
            }

            return base.OnMouseDown(e);
        }

        public static Vector2 GetSnappedPosition(Vector2 input, Vector2 roomSize, int snapValue)
        {
            return new Vector2((int)(input.X / (roomSize.X * DrawableTile.SIZE) * (roomSize.X * DrawableTile.SIZE / snapValue)), (int)(input.Y / (roomSize.Y * DrawableTile.SIZE) * (roomSize.Y * DrawableTile.SIZE / snapValue))) * snapValue;
        }

        private static DrawableTile createTile(Tile tile)
        {
            if (tile.Type == TileType.Cherry)
                return new DrawableCherry(tile);

            return new DrawableTile(tile);
        }
    }
}
