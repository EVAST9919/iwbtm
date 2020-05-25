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
    }
}
