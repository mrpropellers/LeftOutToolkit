using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeftOut.JamAids
{
    // Abstraction layer for the different types of objects which may return a bounding object
    public class BoundsSource : MonoBehaviour
    {
        // TODO: Write custom Inspector so that only one source is visible
        [SerializeField]
        MeshFilter m_MeshSource;

        //[SerializeField]
        //Collider m_ColliderSource;

        // TODO: Switch on bounding type
        Transform BoundsTransform => m_MeshSource.transform;

        Bounds LocalBounds => m_MeshSource.mesh.bounds;

        public Vector3 GetRandomPoint()
        {
            var tf = BoundsTransform;
            var extents = LocalBounds.extents;
            var pointLocal = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z));
            return tf.TransformPoint(pointLocal);
        }
    }
}
