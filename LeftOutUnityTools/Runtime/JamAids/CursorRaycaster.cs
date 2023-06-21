using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut.JamAids
{
    public class CursorRaycaster : MonoBehaviour
    {
        public LayerMask Mask;
        [Min(0)]
        public int MaxDistance = int.MaxValue;
        
        [field: SerializeField]
        public UnityEvent<GameObject> RaycastHit { get; private set; }

        void Update()
        {
            
        }
    }
}
