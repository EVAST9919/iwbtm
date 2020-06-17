using osuTK;
using System;

namespace IWBTM.Game.Helpers
{
    public class MathExtensions
    {
        public static float Map(float value, float lowerCurrent, float upperCurrent, float lowerTarget, float upperTarget)
        {
            return (value - lowerCurrent) / (upperCurrent - lowerCurrent) * (upperTarget - lowerTarget) + lowerTarget;
        }

        public static float Distance(Vector2 input1, Vector2 input2)
        {
            return (float)Math.Sqrt(Pow(input2.X - input1.X) + Pow(input2.Y - input1.Y));
        }

        public static float Pow(float input) => input * input;

        public static float GetAngle(Vector2 input1, Vector2 input2)
        {
            var angle = (float)(Math.Atan2(input1.Y - input2.Y, input1.X - input2.X) * 180 / Math.PI) + 90;

            if (angle < 0)
                angle += 360;

            if (angle > 360)
                angle %= 360f;

            return angle;
        }

        public static Vector2 GetRotatedPosition(Vector2 originPosition, float distance, float angle)
        {
            var rotatedXPos = originPosition.X + (distance * Math.Sin(angle * Math.PI / 180));
            var rotatedYPos = originPosition.Y + (distance * -Math.Cos(angle * Math.PI / 180));

            return new Vector2((float)rotatedXPos, (float)rotatedYPos);
        }
    }
}
