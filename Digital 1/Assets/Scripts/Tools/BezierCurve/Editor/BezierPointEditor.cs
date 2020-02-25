#pragma warning disable
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(BezierPoint)), CanEditMultipleObjects]
public class BezierPointEditor : Editor
{
		BezierPoint point;

		SerializedProperty handleTypeProp;
		SerializedProperty handle1Prop;
		SerializedProperty handle2Prop;

		private delegate void HandleFunction(BezierPoint p);
		private HandleFunction[] handlers = new HandleFunction[] { HandleConnected, HandleBroken, HandleAbsent };


		void OnEnable()
		{
				point = (BezierPoint)target;

				handleTypeProp = serializedObject.FindProperty("handleStyle");
				handle1Prop = serializedObject.FindProperty("handle1");
				handle2Prop = serializedObject.FindProperty("handle2");
		}


		public override void OnInspectorGUI()
		{

				serializedObject.Update();

				BezierPoint.HandleStyle newHandleType = (BezierPoint.HandleStyle)EditorGUILayout.EnumPopup("Handle Type", (BezierPoint.HandleStyle)handleTypeProp.intValue);

				if (newHandleType != (BezierPoint.HandleStyle)handleTypeProp.intValue)
				{
						handleTypeProp.intValue = (int)newHandleType;

						if ((int)newHandleType == 0)
						{
								if (handle1Prop.vector3Value != Vector3.zero)
								{
										handle2Prop.vector3Value = -handle1Prop.vector3Value;
								}
								else if (handle2Prop.vector3Value != Vector3.zero)
								{
										handle1Prop.vector3Value = -handle2Prop.vector3Value;
								}
								else
								{
										handle1Prop.vector3Value = new Vector3(0.1f, 0, 0);
										handle2Prop.vector3Value = new Vector3(-0.1f, 0, 0);
								}
						}

						else if ((int)newHandleType == 1)
						{
								if (handle1Prop.vector3Value == Vector3.zero && handle2Prop.vector3Value == Vector3.zero)
								{
										handle1Prop.vector3Value = new Vector3(0.1f, 0, 0);
										handle2Prop.vector3Value = new Vector3(-0.1f, 0, 0);
								}
						}

						else if ((int)newHandleType == 2)
						{
								handle1Prop.vector3Value = Vector3.zero;
								handle2Prop.vector3Value = Vector3.zero;
						}
				}

				if (handleTypeProp.intValue != 2)
				{
						Vector3 newHandle1 = EditorGUILayout.Vector3Field("Handle 1", handle1Prop.vector3Value);
						Vector3 newHandle2 = EditorGUILayout.Vector3Field("Handle 2", handle2Prop.vector3Value);

						if (handleTypeProp.intValue == 0)
						{
								if (newHandle1 != handle1Prop.vector3Value)
								{
										handle1Prop.vector3Value = newHandle1;
										handle2Prop.vector3Value = -newHandle1;
								}

								else if (newHandle2 != handle2Prop.vector3Value)
								{
										handle1Prop.vector3Value = -newHandle2;
										handle2Prop.vector3Value = newHandle2;
								}
						}

						else
						{
								handle1Prop.vector3Value = newHandle1;
								handle2Prop.vector3Value = newHandle2;
						}
				}

				if (GUI.changed)
				{
						serializedObject.ApplyModifiedProperties();
						EditorUtility.SetDirty(target);
				}
		}


		void OnSceneGUI()
		{
				Handles.color = Color.green;
				Vector3 newPosition = Handles.FreeMoveHandle(point.Position, point.transform.rotation, HandleUtility.GetHandleSize(point.Position) * 0.2f, Vector3.zero, Handles.CubeCap);
				if (point.Position != newPosition)
				{
						point.Position = newPosition;
				}

				handlers[(int)point.handleStyle](point);

				Handles.color = Color.yellow;
				Handles.DrawLine(point.Position, point.GlobalHandle1);
				Handles.DrawLine(point.Position, point.GlobalHandle2);

				BezierCurveEditor.DrawOtherPoints(point.Curve, point);
		}


		private static void HandleConnected(BezierPoint p)
		{
				Handles.color = Color.cyan;

				Vector3 newGlobal1 = Handles.FreeMoveHandle(p.GlobalHandle1, p.transform.rotation, HandleUtility.GetHandleSize(p.GlobalHandle1) * 0.15f, Vector3.zero, Handles.SphereCap);

				if (newGlobal1 != p.GlobalHandle1)
				{
						Undo.RegisterUndo(p, "Move Handle");
						p.GlobalHandle1 = newGlobal1;
						p.GlobalHandle2 = -(newGlobal1 - p.Position) + p.Position;
				}

				Vector3 newGlobal2 = Handles.FreeMoveHandle(p.GlobalHandle2, p.transform.rotation, HandleUtility.GetHandleSize(p.GlobalHandle2) * 0.15f, Vector3.zero, Handles.SphereCap);

				if (newGlobal2 != p.GlobalHandle2)
				{
						Undo.RegisterUndo(p, "Move Handle");
						p.GlobalHandle1 = -(newGlobal2 - p.Position) + p.Position;
						p.GlobalHandle2 = newGlobal2;
				}
		}


		private static void HandleBroken(BezierPoint p)
		{
				Handles.color = Color.cyan;

				Vector3 newGlobal1 = Handles.FreeMoveHandle(p.GlobalHandle1, Quaternion.identity, HandleUtility.GetHandleSize(p.GlobalHandle1) * 0.15f, Vector3.zero, Handles.SphereCap);
				Vector3 newGlobal2 = Handles.FreeMoveHandle(p.GlobalHandle2, Quaternion.identity, HandleUtility.GetHandleSize(p.GlobalHandle2) * 0.15f, Vector3.zero, Handles.SphereCap);

				if (newGlobal1 != p.GlobalHandle1)
				{
						Undo.RegisterUndo(p, "Move Handle");
						p.GlobalHandle1 = newGlobal1;
				}

				if (newGlobal2 != p.GlobalHandle2)
				{
						Undo.RegisterUndo(p, "Move Handle");
						p.GlobalHandle2 = newGlobal2;
				}
		}


		private static void HandleAbsent(BezierPoint p)
		{
				p.Handle1 = Vector3.zero;
				p.Handle2 = Vector3.zero;
		}
}