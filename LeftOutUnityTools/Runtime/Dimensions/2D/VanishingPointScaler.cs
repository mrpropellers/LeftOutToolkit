using System;
using UnityEngine;

namespace LeftOut.Perspectives
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class VanishingPointScaler : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve m_ScaleCurve = new AnimationCurve();

        // TODO: Really need some boilerplate for editor-only buttons
        [SerializeField]
        bool BUTTON_ClampToCurrentDistance;
        
        // Maximum distance from the vanishing point to clamp to - anything further away from the vanishing point than
        // this distance will be the same size
        [field: SerializeField]
        public float ClampDistance { get; set; }
        
        [field: SerializeField]
        public Transform VanishingPoint { get; set; }

        public float DistanceToPoint => (VanishingPoint.position - transform.position).magnitude;

        void OnValidate()
        {
            if (BUTTON_ClampToCurrentDistance && VanishingPoint != null)
            {
                ClampDistance = DistanceToPoint;
                BUTTON_ClampToCurrentDistance = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            var d = VanishingPoint != null
                ? Mathf.Clamp01(DistanceToPoint / ClampDistance)
                : 1f;
            transform.localScale = Vector3.one * m_ScaleCurve.Evaluate(d);
        }
    }
}
