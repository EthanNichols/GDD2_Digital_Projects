using UnityEngine;
using UnityEditor;


/// <summary>
/// Editor class to draw and display a time span in the inspector
/// </summary>
[CustomPropertyDrawer(typeof(TimeSpanProperty)), CanEditMultipleObjects, ExecuteInEditMode]
public class TimeSpanDrawer : PropertyDrawer
{
		/// <summary>
		/// The width at which the amount of line the time takes up changes
		/// </summary>
		const int MIN_PROP_WIDTH = 294;

		/// <summary>
		/// Reset button value
		/// </summary>
		bool toggle;

		/// <summary>
		/// Days property
		/// </summary>
		SerializedProperty days;
		/// <summary>
		/// Hours property
		/// </summary>
		SerializedProperty hours;
		/// <summary>
		/// Minutes property
		/// </summary>
		SerializedProperty minutes;
		/// <summary>
		/// Seconds property
		/// </summary>
		SerializedProperty seconds;
		/// <summary>
		/// Milliseconds property
		/// </summary>
		SerializedProperty milliseconds;

		/// <summary>
		/// Clear all of the properties back to 0
		/// </summary>
		private void ClearTime()
		{
				days.intValue = 0;
				hours.intValue = 0;
				minutes.intValue = 0;
				seconds.intValue = 0;
				milliseconds.intValue = 0;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
				// OnGui is called twice.
				// This filters out one of the calls.
				if (position.x <= 0.01f)
				{
						return;
				}

				// Get the serialized properties
				days = property.FindPropertyRelative("days");
				hours = property.FindPropertyRelative("hours");
				minutes = property.FindPropertyRelative("minutes");
				seconds = property.FindPropertyRelative("seconds");
				milliseconds = property.FindPropertyRelative("milliseconds");

				// Calculate the position of the reset box
				Rect resetBox = position;
				resetBox.height = EditorGUI.GetPropertyHeight(property);
				resetBox.x = resetBox.width;
				resetBox.width = EditorGUI.GetPropertyHeight(property);

				// If the reset button is pressed clear the time
				// This needs to be done before the properties are drawn
				bool resetTime = GUI.Button(resetBox, GUIContent.none);
				if (resetTime)
				{
						ClearTime();
				}

				// Draw the label over the box
				int indentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				resetBox.x = position.width + 4;
				EditorGUI.LabelField(resetBox, new GUIContent("R"));
				EditorGUI.indentLevel = indentLevel;

				// Calculate the rectangle for the timespan properties
				Rect propertyRect = position;
				propertyRect.height = EditorGUI.GetPropertyHeight(property);
				propertyRect.width -= EditorGUI.GetPropertyHeight(property);

				// Labels for each field boc
				GUIContent[] labels = {
						new GUIContent("D"),
						new GUIContent("H"),
						new GUIContent("M"),
						new GUIContent("S"),
						new GUIContent("s")
				};
				EditorGUI.MultiPropertyField(propertyRect, labels, days, label);
		}


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
				// If the width of the inspector is small the timespan property expands to two lines
				// This will adjust to the correct line height to avoid overlapping properties
				// that are drawn under the timespan property
				return base.GetPropertyHeight(property, label) * ((Screen.width < 333) ? 2 : 1);
		}
}
