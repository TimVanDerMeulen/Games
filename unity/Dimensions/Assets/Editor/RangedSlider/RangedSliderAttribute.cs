using UnityEngine;
using UnityEditor;

public class RangedSliderAttribute : PropertyAttribute 
{
    public readonly float minLimit;
    public readonly float maxLimit;
    public float minVal;
    public float maxVal;

    public RangedSliderAttribute (float minLimit, float maxLimit) : this(minLimit, maxLimit, minLimit + (maxLimit - minLimit) * 1/3, minLimit + (maxLimit - minLimit) * 2/3){}
	
	public RangedSliderAttribute (float minLimit, float maxLimit, float minDefault, float maxDefault) {
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;
        this.minVal = minDefault;
        this.maxVal = maxDefault;
    }
}
