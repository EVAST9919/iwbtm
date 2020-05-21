using IWBTM.Game.Screens.Play.Playfield;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Helpers
{
    public class CollisionHelper
    {
        public static bool Collided(Vector2 position, Vector2 size, Tile tile)
        {
            var rectanglePoints = createRectanglePoints(position, size);
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

        private static List<Vector2> createTrianglePoints(Tile tile)
        {
            var list = new List<Vector2>();

            var cornerPosition = tile.Position;

            switch (tile.Type)
            {
                case TileType.SpikeBottom:
                    list.Add(cornerPosition);
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE - 1, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE / 2 - 0.5f, cornerPosition.Y + Tile.SIZE - 1));
                    break;

                case TileType.SpikeTop:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + Tile.SIZE - 1));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE / 2 - 0.5f, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE - 1, cornerPosition.Y + Tile.SIZE - 1));
                    break;

                case TileType.SpikeLeft:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + Tile.SIZE / 2 - 0.5f));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE - 1, cornerPosition.Y));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE - 1, cornerPosition.Y + Tile.SIZE - 1));
                    break;

                case TileType.SpikeRight:
                    list.Add(cornerPosition);
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + Tile.SIZE - 1));
                    list.Add(new Vector2(cornerPosition.X + Tile.SIZE - 1, cornerPosition.Y + Tile.SIZE / 2 - 0.5f));
                    break;
            }

            return list;
        }

        private static List<Vector2> createRectanglePoints(Vector2 position, Vector2 size)
        {
            var list = new List<Vector2>
            {
                new Vector2(position.X - size.X / 2f, position.Y - size.Y / 2f),
                new Vector2(position.X + size.X / 2f - 1, position.Y - size.Y / 2f),
                new Vector2(position.X - size.X / 2f, position.Y + size.Y / 2f - 1),
                new Vector2(position.X + size.X / 2f - 1, position.Y + size.Y / 2f - 1)
            };

            return list;
        }

        private static bool lineCollision(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 b = Vector2.Subtract(a2, a1);
            Vector2 d = Vector2.Subtract(b2, b1);
            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = Vector2.Subtract(b1, a1);
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }
    }
}
