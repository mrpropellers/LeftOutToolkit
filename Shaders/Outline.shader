Shader "LeftOut/UniversalRP/Outline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #include "UnityCG.cginc"
        
        sampler2D _MainTex;
        sampler2D _ObjectMaskTex;
        float4 _OutlineColor;
        int _OutlineThickness;
        float _OutlineFadeWidth;
        
        ENDHLSL
    	
        Pass
        {
            Name "Outline_ObjectMask"
            HLSLPROGRAM
            #pragma vertex vert_mask
            #pragma fragment frag_mask

            struct vert_input {
                float4 vertex : POSITION;
            };

            struct frag_input {
                float4 vertex : POSITION;
            };
            
            frag_input vert_mask(vert_input input)
            {
                frag_input output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                return output;
            }
            
            float4 frag_mask(frag_input input) : SV_TARGET
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
        Pass
        {
            Name "Outline_BlurVertical"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct vert_input {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct frag_input {
                float4 vertex : POSITION;
            	float4 uv : TEXCOORD0;
            };
            
            frag_input vert(vert_input v)
			{
				frag_input o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
            
            float4 frag(frag_input input) : SV_TARGET
            {
			    float4 color = 0;
            	int visibleCount = 0;
			    
			    // Iterate over blur samples
			    for (int index = -_OutlineThickness; index <= _OutlineThickness; index++)
			    {
				    const float2 offset = float2(0, index) * (1.0f / _ScreenParams.y);
				    const float4 sample = tex2D(_ObjectMaskTex, input.uv + offset);
				    color += sample;
			    	visibleCount += sample.a > 0;
			    }
			    
			    // Divide the sum of values by the number of samples
            	color.xyz = color.xyz / visibleCount;
			    color.a = color.a / (_OutlineThickness * 2 + 1);
			    return color;
            }
            ENDHLSL
        }
        Pass
        {
            Name "Outline_BlurHorizontal"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct vert_input {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct frag_input {
                float4 vertex : POSITION;
            	float4 uv : TEXCOORD0;
            };
            
            frag_input vert(vert_input v)
			{
				frag_input o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
            
            float4 frag(frag_input input) : SV_TARGET
            {
			    float4 color = 0;
            	int visibleCount = 0;
			    
			    // Iterate over blur samples
			    for (int index = -_OutlineThickness; index <= _OutlineThickness; index++)
			    {
				    const float2 offset = float2(index, 0) * (1.0f / _ScreenParams.x);
				    const float4 sample = tex2D(_MainTex, input.uv + offset);
				    color += sample;
			    	visibleCount += sample.a > 0;
			    }
			    
			    // Divide the sum of values by the number of samples
			    color.xyz = color.xyz / visibleCount;
			    color.a = color.a / (_OutlineThickness * 2 + 1);
            	color.a = saturate(color.a * (1.0f / _OutlineFadeWidth));

            	// Subtract the object mask from the final result.
            	color = tex2D(_ObjectMaskTex, input.uv).r > 0 ? float4(0, 0, 0, 0): color;
            	
			    return color;
            }
            ENDHLSL
        }
    }
}