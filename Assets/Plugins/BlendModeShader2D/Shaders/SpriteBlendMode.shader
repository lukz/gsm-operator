// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BlendModeShader2D/SpriteBlendMode"
{
	Properties 
	{
		_MainTex ("Main texture", 2D) = "white" {}
		_Color ("Main texture color", Color) = (1,1,1,1)

		//Seems that 'KeywordEnum' has a limit number of 9.
		//[KeywordEnum(Multiply, Screen, Overlay, Hardlight, Softlight, Colordodge, Lineardodge, Colorburn, Linearburn, Vividlight, Linearlight, Subtract, Divide, Addition, Difference, Darken, Lighten, Hue, Saturation, Clor, Luminosity)]
		//_BM("Blend Mode", Float) = 0

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
			"IgnoreProjector"="true" 
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
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
			#include "BlendModes.cginc"
			#include "BlendModeUtils.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
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
				half2 effectUV : TEXCOORD1;
			};

			v2f vert(a2v v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				o.vertex = UnityObjectToClipPos(v.vertex);
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
				fixed4 mainColor = tex2D(_MainTex, i.uv) * i.color;

				fixed3 eColor = CheckColorSolo(tex2D(_EffectTex, i.effectUV).rgb) * _EffectColor;

			#if GRAY_BASE_ON
				mainColor.rgb = (mainColor.r + mainColor.g + mainColor.b) / 3;
			#endif

				float4 c = float4(Blend(mainColor, eColor), mainColor.a); 	

				return c;
			}

			ENDCG
		}	
	}

	FallBack "Sprites/Default"

	CustomEditor "BlendModeMaterialEditor"
}
