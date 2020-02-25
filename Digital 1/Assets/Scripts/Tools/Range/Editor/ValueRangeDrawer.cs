using UnityEngine;
using UnityEditor;

/// Even though a value range is essentially a <see cref="Vector2"/>
/// I wasn't a fan of how a <see cref="Vector2"/> property was diaplyed in the inspector
/// When the inspector width was too small a <see cref="Vector2"/> would expand
/// to occupy 2 line heights instead of 1
/// So the with range values they will always only take up 1 line height

/// <summary>
/// Editor class to draw and display a value range in the inspector
/// </summary>
[CustomPropertyDrawer(typeof(ValueRange)), CustomPropertyDrawer(typeof(ValueIntRange)), CanEditMultipleObjects, ExecuteInEditMode]
public class ValueRangeDrawer : PropertyDrawer
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
				// Get the property values
				SerializedProperty minProperty = property.FindPropertyRelative(MIN_VALUE_NAME);
				SerializedProperty maxProperty = property.FindPropertyRelative(MAX_VALUE_NAME);

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
				EditorGUI.LabelField(position, label);

				// Calculate the rect that the properties have
				Rect propertyRect = position;
				propertyRect.x += EditorGUIUtility.labelWidth;
				propertyRect.width -= EditorGUIUtility.labelWidth;

				// Calculate the rect that the min property has
				Rect minPropertyRect = propertyRect;
				minPropertyRect.width *= 0.5f;
				minPropertyRect.width -= FIELD_SPACING;

				// Calculate the rect that the max property has
				Rect maxPropertyRect = minPropertyRect;
				maxPropertyRect.x += maxPropertyRect.width + FIELD_SPACING * 2;

				// Calculate the rect for the min property label
				Rect minLabelRect = minPropertyRect;
				minLabelRect.width = FIELD_WIDTH;

				// Calculate the rect for the max property label
				Rect maxLabelRect = maxPropertyRect;
				maxLabelRect.width = FIELD_WIDTH;

				// Calculate the rect for the min property
				Rect minFieldRect = minPropertyRect;
				minFieldRect.x += minLabelRect.width;
				minFieldRect.width -= minLabelRect.width;

				// Calculate the rect for the max property
				Rect maxFieldRect = maxPropertyRect;
				maxFieldRect.x += maxLabelRect.width;
				maxFieldRect.width -= maxLabelRect.width;

				// Draw the min label and min property
				EditorGUI.PrefixLabel(minLabelRect, new GUIContent("Min"));
				if (property.type == VALUE_RANGE_TYPE_NAME)
				{
						minFloat = EditorGUI.DelayedFloatField(minFieldRect, minProperty.floatValue);
				}
				else
				{
						minInt = EditorGUI.DelayedIntField(minFieldRect, minProperty.intValue);
				}

				// Draw the max lavel and max property
				EditorGUI.PrefixLabel(maxLabelRect, new GUIContent("Max"));
				if (property.type == VALUE_RANGE_TYPE_NAME)
				{
						maxFloat = EditorGUI.DelayedFloatField(maxFieldRect, maxProperty.floatValue);
				}
				else
				{
						maxInt = EditorGUI.DelayedIntField(maxFieldRect, maxProperty.intValue);
				}

				// Set the min/max values of the range if after the inspector updates
				minProperty.floatValue = Mathf.Min(minFloat, maxFloat);
				maxProperty.floatValue = Mathf.Max(minFloat, maxFloat);
				minProperty.intValue = Mathf.Min(minInt, maxInt);
				maxProperty.intValue = Mathf.Max(minInt, maxInt);
		}
}
