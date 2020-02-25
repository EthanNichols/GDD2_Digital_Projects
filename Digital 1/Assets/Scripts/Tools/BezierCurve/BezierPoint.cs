using UnityEngine;
using System;
using System.Collections;


/// <summary>
/// Helper class for Bezier Points apart of a Bezier Curve.
/// </summary>
[Serializable]
public class BezierPoint : MonoBehaviour
{
		/// <summary>
		/// The relationship type between a bezier point and its handles.
		/// </summary>
		public enum HandleStyle
		{
				Connected,
				Broken,
				None,
		}


		/// <summary>
		///	The Curve this point is apart of.
		/// </summary>
		[SerializeField]
		private BezierCurve curve;
		/// <summary>
		/// Value describing the relationship between this point's handles.
		/// </summary>
		public HandleStyle handleStyle;
		/// <summary>
		/// The position of the first handle relative to the point.
		/// </summary>
		[SerializeField]
		private Vector3 handle1;
		/// <summary>
		/// The position of the second handle relative to the point.
		/// </summary>
		[SerializeField]
		private Vector3 handle2;
		/// <summary>
		/// Used to determine if this point has moved since the last frame.
		/// </summary>
		private Vector3 lastPosition;


		#region Properties

		/// <summary>
		/// The curve this point is apart of.
		/// Setting this updates the length of the curve.
		/// This is not an alternative to add a point to a curve,
		/// Call <see cref="BezierCurve.AddPoint(BezierPoint, int?)"/> instead.
		/// </summary>
		public BezierCurve Curve
		{
				get
				{
						return curve;
				}
				set
				{
						if (curve != null && curve != value)
						{
								curve.RemovePoint(this);
						}
						if (value == null)
						{
								curve = null;
								return;
						}

						curve = value;
						curve.CalculateLength();
				}
		}


		/// <summary>
		/// Easy access to the transform global position.
		/// </summary>
		public Vector3 Position
		{
				get
				{
						return transform.position;
				}
				set
				{
						transform.position = value;
				}
		}


		/// <summary>
		/// Easy acces to the transform local position.
		/// </summary>
		public Vector3 LocalPosition
		{
				get
				{
						return transform.localPosition;
				}
				set
				{
						transform.localPosition = value;
				}
		}


		/// <summary>
		/// The position of the first handle relative to the point.
		/// Setting this updates the length of the curve.
		/// </summary>
		public Vector3 Handle1
		{
				get
				{
						return handle1;
				}
				set
				{
						if (handle1 == value)
						{
								return;
						}

						handle1 = value;
						if (handleStyle == HandleStyle.None)
						{
								handleStyle = HandleStyle.Broken;
						}
						else if (handleStyle == HandleStyle.Connected)
						{
								handle2 = -value;
						}
						curve.CalculateLength();
				}
		}


		/// <summary>
		///	The position of the first handle in world space
		/// </summary>
		public Vector3 GlobalHandle1
		{
				get
				{
						return transform.TransformPoint(Handle1);
				}
				set
				{
						Handle1 = transform.InverseTransformPoint(value);
						curve.CalculateLength();
				}
		}


		/// <summary>
		/// The position of the second handle relative to the point.
		/// Setting this updates the length of the curve.
		/// </summary>
		public Vector3 Handle2
		{
				get
				{
						return handle2;
				}
				set
				{
						if (handle2 == value)
						{
								return;
						}

						handle2 = value;
						if (handleStyle == HandleStyle.None)
						{
								handleStyle = HandleStyle.Broken;
						}
						else if (handleStyle == HandleStyle.Connected)
						{
								handle1 = -value;
						}
						curve.CalculateLength();
				}
		}


		/// <summary>
		///	The position of the second handle in world space
		/// </summary>
		public Vector3 GlobalHandle2
		{
				get
				{
						return transform.TransformPoint(Handle2);
				}
				set
				{
						Handle2 = transform.InverseTransformPoint(value);
						curve.CalculateLength();
				}
		}

		#endregion


		void Update()
		{
				if (curve != null)
				{
						if (transform.position != lastPosition)
						{
								curve.CalculateLength();
								lastPosition = transform.position;
						}
				}
		}
}