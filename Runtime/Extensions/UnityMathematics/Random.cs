using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace LeftOut.Extensions.Mathematics
{
    public static class RandomExtensions
    {
        // https://stackoverflow.com/questions/218060/random-gaussian-variables
        public static float SampleGaussian(ref this Random self, float mean, float standardDeviation)
        {
            var x = 1f - self.NextFloat();
            var y = 1f - self.NextFloat();
            var normalized = math.sqrt(-2f * math.log(x)) * math.sin(2f * math.PI * y);
            return mean + standardDeviation * normalized;
        }

        public static float SampleGaussianClamped(
            ref this Random self,
            float mean,
            float standardDeviation,
            float maxDeviation)
        {
            var sample = self.SampleGaussian(mean, standardDeviation);
            return math.clamp(sample, mean - maxDeviation, mean + maxDeviation);
        }

        public static void VerifyNormalDistribution(ref this Random self, int numSamples)
        {
            const float min = -1f;
            const float max = 1f;
            var percentInsideStdDev = 0f;
            var oneSamplePercent = 100f / (float)numSamples;
            for (var i = 0; i < numSamples; ++i)
            {
                var sample = self.SampleGaussian(0f, max);
                if (sample is >= min and <= max)
                {
                    percentInsideStdDev += oneSamplePercent;
                }
            }
            Debug.Log($"Took {numSamples} samples and {percentInsideStdDev}% were inside {min} and {max}");
        }
    }
}
