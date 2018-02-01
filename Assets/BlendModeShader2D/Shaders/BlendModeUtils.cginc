#ifndef BM_UTILS_INCLUDED
#define BM_UTILS_INCLUDED


//struct a2vBM
//{
//	float4 vertex   : POSITION;
//	float4 color    : COLOR;
//	float2 texcoord : TEXCOORD0;
//	UNITY_VERTEX_INPUT_INSTANCE_ID
//};

//struct v2fSpriteBM
//{
//	float4 vertex   : SV_POSITION;
//	fixed4 color    : COLOR;
//	float2 uv  : TEXCOORD0;	
//	half2 effectUV : TEXCOORD1;
//};

//struct v2fUIBM
//{
//	float4 vertex   : SV_POSITION;
//	fixed4 color    : COLOR;
//	float2 uv  : TEXCOORD0;
//	float2 effectUV : TEXCOORD1;
//	float4 worldPosition : TEXCOORD2;
//	UNITY_VERTEX_OUTPUT_STEREO
//};


//v2fSpriteBM vertSpriteBM(a2vBM v)
//{
//	v2fSpriteBM o;
//	UNITY_SETUP_INSTANCE_ID(v);
//	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//	o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
//	o.color = v.color * _Color;
//	o.effectUV = TRANSFORM_TEX (v.texcoord, _EffectTex);
//#ifdef PIXELSNAP_ON
//	o.vertex = UnityPixelSnap (o.vertex);
//#endif
//	return o;
//}

//v2fUIBM vertUIBM(a2vBM v)
//{
//	v2fUIBM o;
//	UNITY_SETUP_INSTANCE_ID(v);
//	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
//	o.worldPosition = v.vertex;
//	o.vertex = UnityObjectToClipPos(o.worldPosition);
//	o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);				
//	o.color = v.color * _Color;
//	o.effectUV = TRANSFORM_TEX (v.texcoord, _EffectTex);
//#ifdef PIXELSNAP_ON
//	o.vertex = UnityPixelSnap (o.vertex);
//#endif
//	return o;
//}


fixed3 CheckColorSolo(fixed3 sourceColor) //EffectColor
{
#if _COLORSOLO_RED 
		return fixed3(sourceColor.r, sourceColor.r, sourceColor.r); 
#elif _COLORSOLO_GREEN 
		return fixed3(sourceColor.g, sourceColor.g, sourceColor.g); 
#elif _COLORSOLO_BLUE 
		return fixed3(sourceColor.b, sourceColor.b, sourceColor.b); 
#else 
		return fixed3(sourceColor.r, sourceColor.g, sourceColor.b); 
#endif
}

float3 Blend(float3 base, float3 top)
{
#if _BM_NORMAL
	return base;
//Darken
#elif _BM_DARKEN
	return Darken(base, top);
#elif _BM_MULTIPLY
	return Multiply(base, top);
#elif _BM_COLORBURN
	return ColorBurn(base, top);
#elif _BM_LINEARBURN
	return LinearBurn(base, top);
#elif _BM_DARKERCOLOR
	return DarkerColor(base, top);
//Lighten
#elif _BM_LIGHTEN
	return Lighten(base, top);
#elif _BM_SCREEN
	return Screen(base, top);
#elif _BM_COLORDODGE
	return ColorDodge(base, top);
#elif _BM_LINEARDODGE
	return LinearDodge(base, top);
#elif _BM_LIGHTERCOLOR
	return LighterColor(base, top);
//Contrast
#elif _BM_OVERLAY
	return Overlay(base, top);
#elif _BM_SOFTLIGHT
	return SoftLight(base, top);
#elif _BM_HARDLIGHT
	return HardLight(base, top);	
#elif _BM_VIVIDLIGHT
	return VividLight(base, top);
#elif _BM_LINEARLIGHT
	return LinearLight(base, top);
#elif _BM_PINLIGHT
	return PinLight(base, top);
#elif _BM_HARDMIX
	return HardMix(base, top);
//Inversion
#elif _BM_DIFFERENCE
	return Difference(base, top);
#elif _BM_EXCLUSION
	return Exclusion(base, top);
//Cancelation
#elif _BM_SUBTRACT
	return Subtract(base, top);
#elif _BM_DIVIDE
	return Divide(base, top);
//Component
#elif _BM_HUE
	return Hue(base, top);
#elif _BM_SATURATION
	return Saturation(base, top);
#elif _BM_COLOR
	return Color(base, top);
#elif _BM_LUMINOSITY
	return Luminosity(base, top);	
#else
	return float3(1, 0, 1); //shouldn't happen
#endif
}

float3 Blend1(float3 base, float3 top)
{
#if _BM_NORMAL
	return top;
//Darken
#elif _BM_DARKEN
	return Darken(base, top);
#elif _BM_MULTIPLY
	return Multiply(base, top);
#elif _BM_COLORBURN
	return ColorBurn(base, top);
#elif _BM_LINEARBURN
	return LinearBurn(base, top);
#elif _BM_DARKERCOLOR
	return DarkerColor(base, top);
//Lighten
#elif _BM_LIGHTEN
	return Lighten(base, top);
#elif _BM_SCREEN
	return Screen(base, top);
#elif _BM_COLORDODGE
	return ColorDodge(base, top);
#elif _BM_LINEARDODGE
	return LinearDodge(base, top);
#elif _BM_LIGHTERCOLOR
	return LighterColor(base, top);
//Contrast
#elif _BM_OVERLAY
	return Overlay(base, top);
#elif _BM_SOFTLIGHT
	return SoftLight(base, top);
#elif _BM_HARDLIGHT
	return HardLight(base, top);	
#elif _BM_VIVIDLIGHT
	return VividLight(base, top);
#elif _BM_LINEARLIGHT
	return LinearLight(base, top);
#elif _BM_PINLIGHT
	return PinLight(base, top);
#elif _BM_HARDMIX
	return HardMix(base, top);
//Inversion
#elif _BM_DIFFERENCE
	return Difference(base, top);
#elif _BM_EXCLUSION
	return Exclusion(base, top);
//Cancelation
#elif _BM_SUBTRACT
	return Subtract(base, top);
#elif _BM_DIVIDE
	return Divide(base, top);
//Component
#elif _BM_HUE
	return Hue(base, top);
#elif _BM_SATURATION
	return Saturation(base, top);
#elif _BM_COLOR
	return Color(base, top);
#elif _BM_LUMINOSITY
	return Luminosity(base, top);	
#else
	return float3(1, 0, 1); //shouldn't happen
#endif
}


//fixed3 EffectColor(fixed3 sourceColor)
//{
//#ifdef _COLORSOLO_RED 
//		return fixed3(sourceColor.r, sourceColor.r, sourceColor.r); 
//#endif
//#ifdef _COLORSOLO_GREEN 
//		return fixed3(sourceColor.g, sourceColor.g, sourceColor.g); 
//#endif
//#ifdef _COLORSOLO_BLUE 
//		return fixed3(sourceColor.b, sourceColor.b, sourceColor.b); 
//#endif
//#ifdef _COLORSOLO_NONE
//		return fixed3(sourceColor.r, sourceColor.g, sourceColor.b); 
//#endif
//}

//#if GRAY_BASE_ON
//	#define COLOR_BLEND_GRAY_CHECK(blendModeFunc, base, top, result) \
//		result = float4(blendModeFunc((base.r + base.g + base.b) / 3.0, eColor), base.a);
//#else	
//	#define COLOR_BLEND_GRAY_CHECK(blendModeFunc, base, top, result) \
//		result = float4(blendModeFunc(base, eColor), base.a);
//#endif


//#define COLOR_BLEND(blendModeFunc, base, top, result) \
//	result = float4(blendModeFunc(base, eColor), base.a);


#endif