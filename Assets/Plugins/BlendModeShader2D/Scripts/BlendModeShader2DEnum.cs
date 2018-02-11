using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlendModeShader2D
{
    public enum BlendMode
    {
        Normal,
        //Darken
        Darken,
        Multiply,
        ColorBurn,
        LinearBurn,
        DarkerColor,
        //Lighten
        Lighten,
        Screen,
        ColorDodge,
        LinearDodge,
        LighterColor,
        //Contrast
        Overlay,
        SoftLight,
        HardLight,
        VividLight,
        LinearLight,
        PinLight,
        HardMix,
        //Inversion
        Difference,
        Exclusion,
        //Cancelation
        Subtract,
        Divide,
        //Component
        Hue,
        Saturation,
        Color,
        Luminosity
    }

    public enum ColorSoloMode
    {
        None,
        Red,
        Green,
        Blue
    }
}