using System;

namespace IWBTM.Game.Helpers
{
    public class MathExtensions
    {
        public static float Map(float value, float lowerCurrent, float upperCurrent, float lowerTarget, float upperTarget)
        {
            return (value - lowerCurrent) / (upperCurrent - lowerCurrent) * (upperTarget - lowerTarget) + lowerTarget;
        }

        public static float[] Get(double seed, int count, float min, float max)
        {
            var random = new Random((int)Math.Round(seed * 100));

            float[] randoms = new float[count];

            for (int i = 0; i < count; i++)
                randoms[i] = Map((float)random.NextDouble(), 0, 1, min, max);

            return randoms;
        }
    }
}
