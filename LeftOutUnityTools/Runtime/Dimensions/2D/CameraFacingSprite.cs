using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeftOut.Perspectives
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public class CameraFacingSprite : MonoBehaviour
    {
        [SerializeField]
        Camera m_CameraToFace;
        
        void Awake()
        {
            m_CameraToFace = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            var tf = transform;
            //var cameraVector = tf.position - m_CameraToFace.transform.position;
            //tf.forward = cameraVector;
            tf.forward = m_CameraToFace.transform.forward;
        }
    }
}
