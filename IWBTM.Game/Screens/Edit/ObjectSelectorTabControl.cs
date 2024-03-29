﻿using osu.Framework.Graphics;
using osuTK;
using System;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Rooms;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectSelectorTabControl : EditorTabControl<TileType>
    {
        public ObjectSelectorTabControl()
        {
            foreach (var val in Enum.GetValues(typeof(TileType)))
                AddItem((TileType)val);
        }

        protected override EditorTabItem<TileType> CreateItem(TileType value) => new ObjectSelectorTabItem(value);

        private class ObjectSelectorTabItem : EditorTabItem<TileType>, IHasTooltip
        {
            public ObjectSelectorTabItem(TileType value)
                : base(value)
            {
                Add(createTile(new Tile { Type = value }).With(t =>
                {
                    t.Scale = new Vector2(0.8f);
                    t.Anchor = Anchor.Centre;
                    t.Origin = Anchor.Centre;
                }));
            }

            public LocalisableString TooltipText => Value.ToString();

            private static DrawableTile createTile(Tile tile)
            {
                if (tile.Type == TileType.Cherry)
                    return new DrawableCherry(tile, "Default", false, false);

                return new DrawableTile(tile, "Default", false);
            }
        }
    }
}
