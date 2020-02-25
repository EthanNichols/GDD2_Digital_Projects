using UnityEngine;
using UnityEditor;


/// <summary>
/// Editor class to draw the min-max slider in the inspector
/// </summary>
[CustomPropertyDrawer(typeof(RangeSliderAttribute))]
class RangeSliderDrawer : PropertyDrawer
{
		/// <summary>
		/// Name of the float range type
		/// </summary>
		readonly string VALUE_RANGE_TYPE_NAME = typeof(ValueRange).ToString();
		/// <summary>
		/// Name of the int range type
		/// </summary>
		readonly string VALUE_INT_RANGE_TYPE_NAME = typeof(ValueIntRange).ToString();

		/// <summary>
		/// Minimum value property name
		/// </summary>
		const string MIN_VALUE_NAME = "minValue";
		/// <summary>
		/// Maximumvalue property name
		/// </summary>
		const string MAX_VALUE_NAME = "maxValue";

		/// <summary>
		/// The width of the min/max field boxes on the sides of the slider
		/// </summary>
		const float FIELD_WIDTH = 35.0f;
		/// <summary>
		/// The space between the the min-max slider and field boxes
		/// </summary>
		const float FIELD_SPACING = 2.0f;


		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
				/// If the property type doesn't match <see cref="ValueRange"/> or <see cref="ValueIntRange"/>
				/// return since the property type isn't supported
				if (property.type != VALUE_RANGE_TYPE_NAME && property.type != VALUE_INT_RANGE_TYPE_NAME)
				{
						EditorGUI.LabelField(position, label, new GUIContent("ValueRange and ValueIntRange support only"));
						return;
				}

				// Calculate the actual spacing relative to the screen size
				float spacing = FIELD_SPACING * EditorGUIUtility.pixelsPerPoint;

				// Get the property values
				SerializedProperty minProperty = property.FindPropertyRelative(MIN_VALUE_NAME);
				SerializedProperty maxProperty = property.FindPropertyRelative(MAX_VALUE_NAME);

				// Get the range attribute
				RangeSliderAttribute rangeAttribute = attribute as RangeSliderAttribute;

				// Default min/max range values
				int minInt = 0;
				int maxInt = 0;
				float minFloat = 0;
				float maxFloat = 0;

				// Set the min/max range value if the range type is float
				if (property.type == VALUE_RANGE_TYPE_NAME)
				{
						minFloat = minProperty.floatValue;
						maxFloat = maxProperty.floatValue;
				}
				// Set the min/max range value if the range type is int
				// Set the float as well, because sliders only deal with floats
				else
				{
						minInt = minProperty.intValue;
						maxInt = maxProperty.intValue;

						minFloat = minInt;
						maxFloat = maxInt;
				}

				// Draw the name of the property
				EditorGUI.PrefixLabel(position, label);

				// Calculate the rectangle of the slider
				Rect sliderPos = position;
				sliderPos.x += EditorGUIUtility.labelWidth + FIELD_WIDTH + spacing;
				sliderPos.width -= EditorGUIUtility.labelWidth + (FIELD_WIDTH + spacing) * 2;
				// Draw the slider
				EditorGUI.MinMaxSlider(sliderPos, ref minFloat, ref maxFloat, rangeAttribute.minFloat, rangeAttribute.maxFloat);
				// Set the results from any changes that slider had
				minInt = Mathf.RoundToInt(minFloat);
				maxInt = Mathf.RoundToInt(maxFloat);

				// Calculate the rectangle for the min input field
				Rect minPos = position;
				minPos.x += EditorGUIUtility.labelWidth;
				minPos.width = FIELD_WIDTH;
				EditorGUI.showMixedValue = property.FindPropertyRelative(MIN_VALUE_NAME).hasMultipleDifferentValues;
				// Draw the float field if the range is a float
				if (property.type == VALUE_RANGE_TYPE_NAME)
				{
						minFloat = EditorGUI.DelayedFloatField(minPos, minFloat);
				}
				// Draw the int field if the range is an int
				else
				{
						minInt = EditorGUI.DelayedIntField(minPos, minInt);
				}

				// Calculate the rectangle for the max input field
				Rect maxPos = position;
				maxPos.x += maxPos.width - FIELD_WIDTH;
				maxPos.width = FIELD_WIDTH;
				EditorGUI.showMixedValue = property.FindPropertyRelative(MAX_VALUE_NAME).hasMultipleDifferentValues;
				// Draw the float field if the range is a float
				if (property.type == VALUE_RANGE_TYPE_NAME)
				{
						maxFloat = EditorGUI.DelayedFloatField(maxPos, maxFloat);
				}
				// Draw the int field if the range is an int
				else
				{
						maxInt = EditorGUI.DelayedIntField(maxPos, maxInt);
				}

				EditorGUI.showMixedValue = false;

				// Set the min/max values of the range if after the inspector updates
				minProperty.floatValue = Mathf.Max(Mathf.Min(minFloat, maxFloat), rangeAttribute.minFloat);
				maxProperty.floatValue = Mathf.Min(Mathf.Max(minFloat, maxFloat), rangeAttribute.maxFloat);
				minProperty.intValue = Mathf.Max(Mathf.Min(minInt, maxInt), rangeAttribute.minInt);
				maxProperty.intValue = Mathf.Min(Mathf.Max(minInt, maxInt), rangeAttribute.maxInt);
		}
}