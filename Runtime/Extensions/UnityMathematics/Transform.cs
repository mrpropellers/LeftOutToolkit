using UnityEngine;
using Unity.Mathematics;

namespace LeftOut.Extensions.Mathematics
{
    public static class TransformExtensions
    {
        public static float3 FromLocalToWorld(this Transform tf, float3 point)
        {
            var matrix = (float4x4)tf.localToWorldMatrix;
            var pointHomogenous = new float4(point, 1f);
            return math.mul(matrix, pointHomogenous).xyz;
        }
        
        public static float3 FromWorldToLocal(this Transform tf, float3 point)
        {
            var matrix = (float4x4)tf.worldToLocalMatrix;
            var pointHomogenous = new float4(point, 1f);
            return math.mul(matrix, pointHomogenous).xyz;
        }
    }
}
