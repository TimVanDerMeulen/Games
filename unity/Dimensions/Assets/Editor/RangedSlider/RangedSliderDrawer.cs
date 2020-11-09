using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;

[CustomPropertyDrawer(typeof(RangedSliderAttribute))]
public class RangedSliderDrawer : PropertyDrawer  
{
	RangedSliderAttribute rangedSliderAttribute { get { return ((RangedSliderAttribute)attribute); } }
	
	private bool init = true;
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		RangedSliderValues values = GetValues(property);
		values.minLimit = rangedSliderAttribute.minLimit;
		values.maxLimit = rangedSliderAttribute.maxLimit;
		
		if(init){
			init = false;
			values.minVal = rangedSliderAttribute.minVal;
			values.maxVal = rangedSliderAttribute.maxVal;
		}
		
		SerializedObject serializedObject = property.serializedObject;
		
		serializedObject.Update();
		EditorGUI.MinMaxSlider(
			position,
            label,
            ref values.minVal, ref values.maxVal,
            rangedSliderAttribute.minLimit, rangedSliderAttribute.maxLimit);
		serializedObject.ApplyModifiedProperties();
	}
	
	private RangedSliderValues GetValues(SerializedProperty property){
		SerializedObject serializedObject = property.serializedObject;
		var target = serializedObject.targetObject;
		
		var type = target.GetType();
		var f = type.GetField(property.propertyPath);
		if(f == null){
			var p = type.GetProperty(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if(p == null)
				return null;
			return (RangedSliderValues)p.GetValue(target, null);
		}
		return (RangedSliderValues)f.GetValue(target);
	}

}
