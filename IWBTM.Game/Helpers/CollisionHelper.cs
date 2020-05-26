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

            if (lineCollision(rectanglePoints[0], rectanglePoints[1], trianglePoints[0], trianglePoints[1]))
                return true;

            if (lineCollision(rectanglePoints[0], rectanglePoints[1], trianglePoints[1], trianglePoints[2]))
                return true;

            if (lineCollision(rectanglePoints[0], rectanglePoints[1], trianglePoints[2], trianglePoints[0]))
                return true;


            if (lineCollision(rectanglePoints[1], rectanglePoints[2], trianglePoints[0], trianglePoints[1]))
                return true;

            if (lineCollision(rectanglePoints[1], rectanglePoints[2], trianglePoints[1], trianglePoints[2]))
                return true;

            if (lineCollision(rectanglePoints[1], rectanglePoints[2], trianglePoints[2], trianglePoints[0]))
                return true;


            if (lineCollision(rectanglePoints[2], rectanglePoints[3], trianglePoints[0], trianglePoints[1]))
                return true;

            if (lineCollision(rectanglePoints[2], rectanglePoints[3], trianglePoints[1], trianglePoints[2]))
                return true;

            if (lineCollision(rectanglePoints[2], rectanglePoints[3], trianglePoints[2], trianglePoints[0]))
                return true;


            if (lineCollision(rectanglePoints[3], rectanglePoints[0], trianglePoints[0], trianglePoints[1]))
                return true;

            if (lineCollision(rectanglePoints[3], rectanglePoints[0], trianglePoints[1], trianglePoints[2]))
                return true;

            if (lineCollision(rectanglePoints[3], rectanglePoints[0], trianglePoints[2], trianglePoints[0]))
                return true;

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
                    list.Add(new Vector2(cornerPosition.X + (size.X - 1) / 2, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeTop:
                case TileType.SmallSpikeTop:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y - 1));
                    list.Add(new Vector2(cornerPosition.X + (size.X - 1) / 2, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeLeft:
                case TileType.SmallSpikeLeft:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + (size.Y - 1) / 2));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeRight:
                case TileType.SmallSpikeRight:
                    list.Add(cornerPosition);
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y - 1));
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

        private static bool lineCollision(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float uA = ((d.X - c.X) * (a.Y - c.Y) - (d.Y - c.Y) * (a.X - c.X)) / ((d.Y - c.Y) * (b.X - a.X) - (d.X - c.X) * (b.Y - a.Y));
            float uB = ((b.X - a.X) * (a.Y - c.Y) - (b.Y - a.Y) * (a.X - c.X)) / ((d.Y - c.Y) * (b.X - a.X) - (d.X - c.X) * (b.Y - a.Y));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
                return true;

            return false;
        }
    }
}
