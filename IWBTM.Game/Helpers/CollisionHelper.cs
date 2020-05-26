using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Player;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Helpers
{
    public class CollisionHelper
    {
        public static bool Collided(Vector2 position, DrawableTile tile)
        {
            var rectanglePoints = createRectanglePoints(position);
            var trianglePoints = createTrianglePoints(tile);

            foreach (var t in trianglePoints)
            {
                if (pointInPlayer(t, position))
                    return true;
            }

            foreach (var r in rectanglePoints)
            {
                if (pointInTriangle(r, trianglePoints))
                    return true;
            }

            return false;
        }

        private static List<Vector2> createTrianglePoints(DrawableTile tile)
        {
            var list = new List<Vector2>();

            var cornerPosition = tile.Position;
            var size = tile.Size;

            switch (tile.Tile.Type)
            {
                case TileType.SpikeBottom:
                case TileType.SmallSpikeBottom:
                    list.Add(cornerPosition);
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + (size.X - 1) / 2, cornerPosition.Y + size.Y - 2));
                    break;

                case TileType.SpikeTop:
                case TileType.SmallSpikeTop:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y - 2));
                    list.Add(new Vector2(cornerPosition.X + (size.X - 1) / 2, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 2));
                    break;

                case TileType.SpikeLeft:
                case TileType.SmallSpikeLeft:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + (size.Y - 1) / 2));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 2));
                    break;

                case TileType.SpikeRight:
                case TileType.SmallSpikeRight:
                    list.Add(cornerPosition);
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y - 2));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + (size.Y - 1) / 2));
                    break;
            }

            return list;
        }

        private static List<Vector2> createRectanglePoints(Vector2 position)
        {
            var size = DefaultPlayer.SIZE;

            var list = new List<Vector2>
            {
                new Vector2(position.X - size.X / 2f, position.Y - size.Y / 2f + 1),
                new Vector2(position.X + size.X / 2f - 1, position.Y - size.Y / 2f + 1),
                new Vector2(position.X - size.X / 2f, position.Y + size.Y / 2f - 1),
                new Vector2(position.X + size.X / 2f - 1, position.Y + size.Y / 2f - 1)
            };

            return list;
        }

        private static bool pointInPlayer(Vector2 point, Vector2 playerPosition)
        {
            if (point.X > playerPosition.X - (DefaultPlayer.SIZE.X / 2) && point.X < playerPosition.X + (DefaultPlayer.SIZE.X / 2) - 1)
            {
                if (point.Y > playerPosition.Y - (DefaultPlayer.SIZE.Y / 2) && point.Y < playerPosition.Y + (DefaultPlayer.SIZE.Y / 2) - 1)
                    return true;
            }

            return false;
        }

        private static bool pointInTriangle(Vector2 point, List<Vector2> trianglePoints)
        {
            var d1 = sign(point, trianglePoints[0], trianglePoints[1]);
            var d2 = sign(point, trianglePoints[1], trianglePoints[2]);
            var d3 = sign(point, trianglePoints[2], trianglePoints[0]);

            var has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            var has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        private static float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

    }
}
