using IWBTM.Game.Playfield;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class BluePrint : CompositeDrawable
    {
        public readonly Bindable<TileType> Selected = new Bindable<TileType>();

        private readonly Container objectsLayer;
        private readonly Container hoverLayer;

        public BluePrint()
        {
            Size = DefaultPlayfield.BASE_SIZE;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                objectsLayer = new Container
                {
                    Size = DefaultPlayfield.BASE_SIZE,
                },
                hoverLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Grid()
            });
        }

        private Tile tileToPlace;
        private Vector2 mousePosition;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            mousePosition = e.MousePosition;

            if (!hoverLayer.Any())
                hoverLayer.Child = tileToPlace = new Tile(Selected.Value)
                {
                    Origin = Anchor.Centre
                };

            tileToPlace.Position = mousePosition;

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            hoverLayer.Clear();
            tileToPlace?.Expire();
        }

        protected override bool OnClick(ClickEvent e)
        {
            objectsLayer.Add(new Tile(Selected.Value)
            {
                Position = getSnappedPosition(mousePosition)
            });

            return base.OnClick(e);
        }

        private static Vector2 getSnappedPosition(Vector2 input)
        {
            return new Vector2((int)(input.X / DefaultPlayfield.BASE_SIZE.X * DefaultPlayfield.TILES_WIDTH), (int)(input.Y / DefaultPlayfield.BASE_SIZE.Y * DefaultPlayfield.TILES_HEIGHT)) * Tile.SIZE;
        }
    }
}
