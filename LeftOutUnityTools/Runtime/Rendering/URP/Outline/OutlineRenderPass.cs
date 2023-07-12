using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using LeftOut.GlobalConsts;

namespace LeftOut.Toolkit.Rendering
{
    public class OutlineRenderPass : ScriptableRenderPass
    {
        static readonly int k_ObjectMaskRT = Shader.PropertyToID("_Outline_ObjectMaskRT");
        static readonly int k_BlurVerticalRT = Shader.PropertyToID("_Outline_BlurVerticalRT");
        static readonly int k_FinalRT = Shader.PropertyToID("_Outline_FinalRT");
        static readonly int k_PropOutlineColor = Shader.PropertyToID("_OutlineColor");
        static readonly int k_PropObjectMaskTex = Shader.PropertyToID("_ObjectMaskTex");
        static readonly int k_PropOutlineThickness = Shader.PropertyToID("_OutlineThickness");
        static readonly int k_PropOutlineFadeWidth = Shader.PropertyToID("_OutlineFadeWidth");
        static readonly int k_MainTextureProperty = ShaderProperty.MainTextureId;
        
        Material m_OutlineMaterial;
        static readonly Material k_BlendTexture = CoreUtils.CreateEngineMaterial(
            Shader.Find("LeftOut/UniversalRP/BlendTextures"));

        // The UI scaling factor that may need to be applied to the render outline when outlining a UI element
        // Can be computed with Screen.width / referenceResolution.x from the UI Canvas
        public static float ScaleFactor = 1f;
        public static GameObject Target;
        public static Color Color;

        OutlineRenderFeature m_RenderFeature;

        public OutlineRenderPass(OutlineRenderFeature renderFeature)
        {
            m_RenderFeature = renderFeature;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            m_OutlineMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Kemps/Outline"));
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(k_ObjectMaskRT, descriptor.width, descriptor.height, 
                descriptor.depthBufferBits, FilterMode.Point, GraphicsFormat.R8G8B8A8_UNorm);
            cmd.GetTemporaryRT(k_BlurVerticalRT, descriptor.width, descriptor.height, 
                descriptor.depthBufferBits, FilterMode.Point, GraphicsFormat.R8G8B8A8_UNorm);
            cmd.GetTemporaryRT(k_FinalRT, descriptor.width, descriptor.height, 
                descriptor.depthBufferBits, FilterMode.Point, GraphicsFormat.R8G8B8A8_UNorm);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (Target == null)
                return;
            
            var cmd = CommandBufferPool.Get("Outline Render Pass");
            var colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
            
            cmd.SetRenderTarget(k_ObjectMaskRT);
            cmd.ClearRenderTarget(true, true, Color.clear);
            
            // Render the highlighted object's mask.
            var renderers = Target.GetComponentsInChildren<Renderer>();
            cmd.SetGlobalVector(k_PropOutlineColor, Color);
            foreach (var renderer in renderers)
                DrawRenderer(cmd, renderer, m_OutlineMaterial, 0);
            
            // Perform a vertical blur on the object mask.
            cmd.SetRenderTarget(k_BlurVerticalRT);
            cmd.ClearRenderTarget(true, true, Color.clear);
            var blurThickness = Mathf.FloorToInt(ScaleFactor * m_RenderFeature.outlineWidth);
            cmd.SetGlobalInteger(k_PropOutlineThickness, blurThickness);
            cmd.SetGlobalTexture(k_PropObjectMaskTex, k_ObjectMaskRT);
            cmd.Blit(k_ObjectMaskRT, k_BlurVerticalRT, m_OutlineMaterial, 1);
            
            // Perform a horizontal blur on top of the vertical blur, and subtract the object mask.
            cmd.SetRenderTarget(k_FinalRT);
            cmd.ClearRenderTarget(true, true, Color.clear);
            cmd.SetGlobalFloat(k_PropOutlineFadeWidth, m_RenderFeature.fadeWidth);
            cmd.SetGlobalTexture(k_MainTextureProperty, k_BlurVerticalRT);
            cmd.Blit(k_BlurVerticalRT, k_FinalRT, m_OutlineMaterial, 2);
            
            // Overlay the outline on the color buffer;
            cmd.SetGlobalTexture(k_MainTextureProperty, k_FinalRT);
            cmd.Blit(k_FinalRT, colorBuffer, k_BlendTexture);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(k_ObjectMaskRT);
            cmd.ReleaseTemporaryRT(k_BlurVerticalRT);
            cmd.ReleaseTemporaryRT(k_FinalRT);
        }

        static void DrawRenderer(CommandBuffer cmd, Renderer renderer, Material material, int materialPassIndex)
        {
            if (renderer is MeshRenderer meshRenderer)
            {
                var meshFilter = meshRenderer.GetComponent<MeshFilter>();
                for (var i = 0; i < meshFilter.sharedMesh.subMeshCount; i++)
                    cmd.DrawRenderer(meshRenderer, material, i, materialPassIndex);
            }
            else if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
            {
                for (var i = 0; i < skinnedMeshRenderer.sharedMesh.subMeshCount; i++)
                    cmd.DrawRenderer(renderer, material, i, materialPassIndex);
            }
        }

        public void Dispose()
        {
            Object.DestroyImmediate(m_OutlineMaterial);
        }
    }
}
