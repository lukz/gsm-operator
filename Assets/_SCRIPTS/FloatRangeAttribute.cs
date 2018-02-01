using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatRangeAttribute: PropertyAttribute {
    public float min;
    public float max;

    public FloatRangeAttribute(float min, float max) {
        this.min = min;
        this.max = max;
    }
}