Shader "Custom/BlendModeShader2D/UIBlendMode" 
{
	Properties
	{
		[PerRendererData] 
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(GRAY_BASE_ON)] 
		_GrayBase ("Enable gray base", Float) = 0

		[Space(20)]
		_EffectTex("Effect texture", 2D) = "white" {}
		_EffectColor("Effect texture color", Color) = (1,1,1,1)
		[KeywordEnum(None, Red, Green, Blue)]
		_ColorSolo("Color solo mode", Float) = 0

		[Space(20)][Toggle(PIXELSNAP_ON)] 
		PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _BM_NORMAL _BM_DARKEN _BM_MULTIPLY _BM_COLORBURN _BM_LINEARBURN _BM_DARKERCOLOR _BM_LIGHTEN _BM_SCREEN _BM_COLORDODGE _BM_LINEARDODGE _BM_LIGHTERCOLOR _BM_OVERLAY _BM_SOFTLIGHT _BM_HARDLIGHT _BM_VIVIDLIGHT _BM_LINEARLIGHT _BM_PINLIGHT _BM_HARDMIX _BM_DIFFERENCE _BM_EXCLUSION _BM_SUBTRACT _BM_DIVIDE _BM_HUE _BM_SATURATION _BM_COLOR _BM_LUMINOSITY         
			#pragma multi_compile _ GRAY_BASE_ON
			#pragma multi_compile _COLORSOLO_NONE _COLORSOLO_RED _COLORSOLO_GREEN _COLORSOLO_BLUE		
			#pragma multi_compile _ PIXELSNAP_ON

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "BlendModes.cginc"
			#include "BlendModeUtils.cginc"		
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			sampler2D _EffectTex;
			float4 _EffectTex_ST;
			fixed4 _EffectColor;
					
			struct a2v
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};	
			
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv  : TEXCOORD0;
				float2 effectUV : TEXCOORD1;
				float4 worldPosition : TEXCOORD2;
				UNITY_VERTEX_OUTPUT_STEREO
			};						
		
			v2f vert(a2v v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(o.worldPosition);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);				
				o.color = v.color * _Color;
				o.effectUV = TRANSFORM_TEX (v.texcoord, _EffectTex);
			#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
			#endif
				return o;
			}	

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mainColor = (tex2D(_MainTex, i.uv) + _TextureSampleAdd)* i.color;			

				fixed3 eColor = CheckColorSolo(tex2D(_EffectTex, i.effectUV).rgb) * _EffectColor;

			#if GRAY_BASE_ON
				mainColor.rgb = (mainColor.r + mainColor.g + mainColor.b) / 3;
			#endif

				float4 c = float4(Blend(mainColor, eColor), mainColor.a); 	

				c.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

			#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
			#endif

				return c;
			}	

			ENDCG
		}
	}

	FallBack "UI/Default"

	CustomEditor "BlendModeMaterialEditor"
}
