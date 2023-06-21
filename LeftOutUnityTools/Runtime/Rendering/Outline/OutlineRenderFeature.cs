using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LeftOut.Toolkit.Rendering
{
    public class OutlineRenderFeature : ScriptableRendererFeature
    {
        OutlineRenderPass m_RenderPass;
        
        [Range(1, 20)] public int outlineWidth = 7;
        [Range(0.05f, 1f)] public float fadeWidth = 0.15f;

        public override void Create()
        {
            m_RenderPass = new OutlineRenderPass(this);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
#if UNITY_EDITOR
            if (renderingData.cameraData.isSceneViewCamera)
                return;
#endif
            renderer.EnqueuePass(m_RenderPass);
        }

        protected override void Dispose(bool disposing)
        {
            m_RenderPass.Dispose();
        }
    }
}
