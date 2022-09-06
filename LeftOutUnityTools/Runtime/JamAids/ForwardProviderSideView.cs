using System;
using UnityEngine;

namespace LeftOut.JamAids
{
    public class ForwardProviderSideView : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer m_Renderer;

        public Vector2 Forward
        {
            get
            {
                var spriteForward = m_Renderer == null || !m_Renderer.flipX
                    ? Vector2.right
                    : Vector2.left;
                spriteForward.Scale(transform.lossyScale);
                return spriteForward;
            }
        }

        void OnValidate()
        {
            if (m_Renderer == null)
            {
                var renderers = GetComponentsInChildren<SpriteRenderer>();
                if (renderers.Length > 1)
                {
                    Debug.LogWarning($"Multiple SpriteRenderers attached to {name} -- " +
                        "you'll need to assign the correct reference manually");
                }
                else if (renderers.Length == 1)
                {
                    m_Renderer = renderers[0];
                }
            }
        }

    }
}
