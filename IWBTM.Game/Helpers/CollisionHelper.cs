using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Player;
using osuTK;
using System;
using System.Collections.Generic;

namespace IWBTM.Game.Helpers
{
    public class CollisionHelper
    {
        public static bool Collided(Vector2 position, DrawableTile tile)
        {
            var playerPoints = createPlayerPoints(position);
            var trianglePoints1 = createTrianglePoints1(tile);
            var trianglePoints2 = createTrianglePoints2(tile);


            foreach (var t in trianglePoints1)
            {
                if (pointInPlayer(t, playerPoints))
                    return true;
            }

            foreach (var r in playerPoints)
            {
                if (pointInTriangle(r, trianglePoints2))
                    return true;
            }

            return false;
        }

        public static bool CollidedWithCircle(Vector2 playerPosition, DrawableTile tile)
        {
            var radius = tile.Size.X / 2;
            var circlePosition = new Vector2(tile.Position.X + radius, tile.Position.Y + radius);

            var adjustedX = playerPosition.X - DefaultPlayer.SIZE.X / 2;
            var adjustedY = playerPosition.Y - DefaultPlayer.SIZE.Y / 2;

            var adjustedPlayerPosition = new Vector2(adjustedX, adjustedY);

            var deltaX = circlePosition.X - Math.Max(adjustedPlayerPosition.X, Math.Min(circlePosition.X, adjustedPlayerPosition.X + DefaultPlayer.SIZE.X));
            var deltaY = circlePosition.Y - Math.Max(adjustedPlayerPosition.Y, Math.Min(circlePosition.Y, adjustedPlayerPosition.Y + DefaultPlayer.SIZE.Y));
            return MathExtensions.Pow(deltaX) + MathExtensions.Pow(deltaY) < MathExtensions.Pow(radius);
        }

        private static List<Vector2> createTrianglePoints1(DrawableTile tile)
        {
            var list = new List<Vector2>();

            var cornerPosition = tile.Position;
            var size = tile.Size;

            switch (tile.Tile.Type)
            {
                case TileType.SpikeBottom:
                case TileType.SmallSpikeBottom:
                    list.Add(cornerPosition + new Vector2(0, 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X / 2, cornerPosition.Y + size.Y));
                    break;

                case TileType.SpikeTop:
                case TileType.SmallSpikeTop:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X / 2, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y));
                    break;

                case TileType.SpikeLeft:
                case TileType.SmallSpikeLeft:
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y / 2));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y));
                    break;

                case TileType.SpikeRight:
                case TileType.SmallSpikeRight:
                    list.Add(cornerPosition + new Vector2(0, 1));
                    list.Add(new Vector2(cornerPosition.X, cornerPosition.Y + size.Y));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y / 2));
                    break;
            }

            return list;
        }

        private static List<Vector2> createTrianglePoints2(DrawableTile tile)
        {
            var list = new List<Vector2>();

            var cornerPosition = tile.Position;
            var size = tile.Size;

            switch (tile.Tile.Type)
            {
                case TileType.SpikeBottom:
                case TileType.SmallSpikeBottom:
                    list.Add(cornerPosition + Vector2.One);
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X / 2f - 1, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeTop:
                case TileType.SmallSpikeTop:
                    list.Add(new Vector2(cornerPosition.X + 1, cornerPosition.Y + size.Y - 1));
                    list.Add(new Vector2(cornerPosition.X + size.X / 2f, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeLeft:
                case TileType.SmallSpikeLeft:
                    list.Add(new Vector2(cornerPosition.X + 1, cornerPosition.Y + size.Y / 2f - 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y - 1));
                    break;

                case TileType.SpikeRight:
                case TileType.SmallSpikeRight:
                    list.Add(cornerPosition + Vector2.One);
                    list.Add(new Vector2(cornerPosition.X + 1, cornerPosition.Y + size.Y - 1));
                    list.Add(new Vector2(cornerPosition.X + size.X - 1, cornerPosition.Y + size.Y / 2f - 1));
                    break;
            }

            return list;
        }

        private static List<Vector2> createPlayerPoints(Vector2 position)
        {
            var size = DefaultPlayer.SIZE;

            var list = new List<Vector2>
            {
                new Vector2(position.X - size.X / 2, position.Y - size.Y / 2),
                new Vector2(position.X + size.X / 2, position.Y - size.Y / 2),
                new Vector2(position.X - size.X / 2, position.Y + size.Y / 2),
                new Vector2(position.X + size.X / 2, position.Y + size.Y / 2)
            };

            return list;
        }

        private static bool pointInPlayer(Vector2 point, List<Vector2> playerPoints)
        {
            if (point.X > playerPoints[0].X - 1 && point.X < playerPoints[1].X)
            {
                if (point.Y > playerPoints[0].Y && point.Y <= playerPoints[3].Y)
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
