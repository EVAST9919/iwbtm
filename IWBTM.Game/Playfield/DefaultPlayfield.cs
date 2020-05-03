using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using IWBTM.Game.Player;
using System.Collections.Generic;
using System.Linq;
using IWBTM.Game.Objects;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace IWBTM.Game.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public static int WIDTH = 26;
        public static int HEIGHT = 19;

        private DefaultPlayer player;
        private Track track;

        public DefaultPlayfield()
        {
            Width = WIDTH * Tile.SIZE;
            Height = HEIGHT * Tile.SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Tile(TileType.PlatformMiddle)
                {
                    X = Tile.SIZE,
                    Width = (WIDTH - 2) * Tile.SIZE,
                    Height = Tile.SIZE,
                },
                new Tile(TileType.PlatformMiddle)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = Tile.SIZE,
                    Width = (WIDTH - 2) * Tile.SIZE,
                    Height = Tile.SIZE,
                },
                new Tile(TileType.PlatformMiddleRotated)
                {
                    Y = Tile.SIZE,
                    Height = (HEIGHT - 2) * Tile.SIZE,
                    Width = Tile.SIZE
                },
                new Tile(TileType.PlatformMiddleRotated)
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Y = Tile.SIZE,
                    Height = (HEIGHT - 2) * Tile.SIZE,
                    Width = Tile.SIZE
                },
                new BasicTile(TileType.PlatformCorner),
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                },
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft
                },
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight
                }
            });

            AddInternal(player = new DefaultPlayer(GetTiles()));
            AddInternal(new ObjectsController(player));

            track = audio.Tracks.Get("Ghost Rule");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            restart();
        }

        public List<Tile> GetTiles() => InternalChildren.OfType<Tile>().ToList();

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch(e.Key)
                {
                    case Key.R:
                        restart();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        private void restart()
        {
            track.Restart();
            player.SetDefaultPosition();
        }
    }
}
