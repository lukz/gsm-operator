#ifndef BLEND_MODES_INCLUDED
#define BLEND_MODES_INCLUDED


float HueToRgb(float3 pqt)
{
    if (pqt.z < 0.0) pqt.z += 1.0;
    if (pqt.z > 1.0) pqt.z -= 1.0;
    if (pqt.z < 1.0 / 6.0) return pqt.x + (pqt.y - pqt.x) * 6.0 * pqt.z;
    if (pqt.z < 1.0 / 2.0) return pqt.y;
    if (pqt.z < 2.0 / 3.0) return pqt.x + (pqt.y - pqt.x) * (2.0 / 3.0 - pqt.z) * 6.0;

    return pqt.x;
}

float3 HslToRgb (float3 hsl)
{ 
	float3 rgb;
	float3 pqt;

    if (hsl.y == 0) 
	{
		rgb = hsl.z; 
	}
    else
    {
        pqt.y = hsl.z < 0.5 ? hsl.z * (1.0 + hsl.y) : hsl.z + hsl.y - hsl.z * hsl.y;
        pqt.x = 2.0 * hsl.z - pqt.y;
        rgb.r = HueToRgb(float3(pqt.x, pqt.y, hsl.x + 1.0 / 3.0));
        rgb.g = HueToRgb(float3(pqt.x, pqt.y, hsl.x));
        rgb.b = HueToRgb(float3(pqt.x, pqt.y, hsl.x - 1.0 / 3.0));
    }

    return rgb;
}

float3 RgbToHsl(float3 rgb)
{
    float maxC = max(rgb.r, max(rgb.g, rgb.b));
    float minC = min(rgb.r, min(rgb.g, rgb.b));

    float3 hsl;

    hsl = (maxC + minC) / 2.0;

    if (maxC == minC)
	{
		hsl.x = hsl.y = 0.0;
	} 
    else
    {
        float d = maxC - minC;
        hsl.y = (hsl.z > 0.5) ? d / (2.0 - maxC - minC) : d / (maxC + minC);

        if (rgb.r > rgb.g && rgb.r > rgb.b) 
        	hsl.x = (rgb.g - rgb.b) / d + (rgb.g < rgb.b ? 6.0 : 0.0);
        else if (rgb.g > rgb.b) 
        	hsl.x = (rgb.b - rgb.r) / d + 2.0;
        else 
        	hsl.x = (rgb.r - rgb.g) / d + 4.0;

        hsl.x /= 6.0f;
    }

    return hsl;
}


//Blend Modes


//Darken
float3 Darken(float3 base, float3 top)
{ 
	return min(base, top);
}

float4 Darken(float4 base, float4 top)
{ 
	return min(base, top);
}


float3 Multiply(float3 base, float3 top)
{
	return base * top;
}

float4 Multiply(float4 base, float4 top)
{ 
	return base * top;
}


float3 ColorBurn(float3 base, float3 top) 
{ 
	return 1.0 - (1.0 - base) / top; 
}

float4 ColorBurn(float4 base, float4 top) 
{ 
	return 1.0 - (1.0 - base) / top; 
}


float3 LinearBurn(float3 base, float3 top) 
{ 
	return base + top - 1.0; 
}

float4 LinearBurn(float4 base, float4 top) 
{ 
	return base + top - 1.0; 
}


float3 DarkerColor(float3 base, float3 top)
{
	return base.r + base.g + base.b < top.r + top.g + top.b ? base : top;
}

float4 DarkerColor(float4 base, float4 top)
{
	return base.r + base.g + base.b < top.r + top.g + top.b ? base : top;
}


//Lighten
float3 Lighten(float3 base, float3 top)
{ 
	return max(base, top);
}

float4 Lighten(float4 base, float4 top)
{ 
	return max(base, top);
}


float3 Screen(float3 base, float3 top) 
{ 
	return (1.0 - (1.0 - base) * (1.0 - top)); 
}

float4 Screen(float4 base, float4 top) 
{ 
	return (1.0 - (1.0 - base) * (1.0 - top)); 
}


float3 ColorDodge(float3 base, float3 top)
{ 
	return base / (1.0 - top); 
}

float4 ColorDodge(float4 base, float4 top)
{ 
	return base / (1.0 - top); 
}


float3 LinearDodge(float3 base, float3 top)
{ 
	return base + top; 
}

float4 LinearDodge(float4 base, float4 top)
{ 
	return base + top; 
}


float3 LighterColor(float3 base, float3 top)
{
	return base.r + base.g + base.b > top.r + top.g + top.b ? base : top;
}

float4 LighterColor(float4 base, float4 top)
{
	return base.r + base.g + base.b > top.r + top.g + top.b ? base : top;
}


//Contrast
float3 Overlay(float3 base, float3 top)
{
	return base < 0.5 ? 2.0 * base * top : 1.0 - 2.0 * (1.0 - base) * (1.0 - top);
}

float4 Overlay(float4 base, float4 top)
{
	return base < 0.5 ? 2.0 * base * top : 1.0 - 2.0 * (1.0 - base) * (1.0 - top);
}


float3 SoftLight(float3 base, float3 top)
{
	return (1.0 - 2.0 * top) * base * base + 2.0 * base * top;	
}

float4 SoftLight(float4 base, float4 top)
{
	return (1.0 - 2.0 * top) * base * base + 2.0 * base * top;	
}


float3 HardLight(float3 base, float3 top)
{
	return Overlay(top, base);
}

float4 HardLight(float4 base, float4 top)
{
	return Overlay(top, base);
}


float3 VividLight(float3 base, float3 top)
{
	return top > 0.5 ? base / (2.0 * (1.0 - top)) : 1.0 - (1.0 - base) / (top * 2.0);
}

float4 VividLight(float4 base, float4 top)
{
	return top > 0.5 ? base / (2.0 * (1.0 - top)) : 1.0 - (1.0 - base) / (top * 2.0);
}


float3 LinearLight(float3 base, float3 top)
{
	return base + 2.0 * top - 1.0;
}

float4 LinearLight(float4 base, float4 top)
{
	return base + 2.0 * top - 1.0;
}


float3 PinLight(float3 base, float3 top)
{
	return max(2 * top - 1, min(base, 2 * top));
}

float4 PinLight(float4 base, float4 top)
{
	return max(2 * top - 1, min(base, 2 * top));
}


float3 HardMix(float3 base, float3 top)
{
	return top + base < 1 ? 0 : 1;
}

float4 HardMix(float4 base, float4 top)
{
	return top + base < 1 ? 0 : 1;
}


//Inversion
float3 Difference(float3 base, float3 top) 
{ 
	return abs(base - top); 
}

float4 Difference(float4 base, float4 top) 
{ 
	return abs(base - top); 
}


float3 Exclusion(float3 base, float3 top)
{
	return base + top - 2 * base * top;
}

float4 Exclusion(float4 base, float4 top)
{
	return base + top - 2 * base * top;
}


//Cancelation
float3 Subtract(float3 base, float3 top)
{ 
	return base - top; 
}

float4 Subtract(float4 base, float4 top)
{ 
	return base - top; 
}


float3 Divide(float3 base, float3 top)
{ 
	return base / top;
}

float4 Divide(float4 base, float4 top)
{ 
	return base / top;
}


//Component
float3 Hue(float3 base, float3 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return HslToRgb(float3(topHsl.x, baseHsl.y, baseHsl.z));
}

float4 Hue(float4 base, float4 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return float4(HslToRgb(float3(topHsl.x, baseHsl.y, baseHsl.z)), base.a);
}


float3 Saturation(float3 base, float3 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return HslToRgb(float3(baseHsl.x, topHsl.y, baseHsl.z));
}

float4 Saturation(float4 base, float4 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return float4(HslToRgb(float3(baseHsl.x, topHsl.y, baseHsl.z)), base.a);
}


float3 Color(float3 base, float3 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return HslToRgb(float3(topHsl.x, topHsl.y, baseHsl.z));
}

float4 Color(float4 base, float4 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return float4(HslToRgb(float3(topHsl.x, topHsl.y, baseHsl.z)), base.a);
}


float3 Luminosity(float3 base, float3 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return HslToRgb(float3(baseHsl.x, baseHsl.y, topHsl.z));
}

float4 Luminosity(float4 base, float4 top)
{ 
	float3 baseHsl = RgbToHsl(base.rgb);
	float3 topHsl = RgbToHsl(top.rgb);

	return float4(HslToRgb(float3(baseHsl.x, baseHsl.y, topHsl.z)), base.a);
}


#endif