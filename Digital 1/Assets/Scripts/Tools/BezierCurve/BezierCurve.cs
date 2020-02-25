using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


/// <summary>
/// Class for creating, editing, and drawing Bezier Curves
/// </summary>
[Serializable, ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
		/// <summary>
		/// The number of mid-points to calculate between key bezier points.
		/// </summary>
		[SerializeField]
		public int resolution = 30;
		/// <summary>
		/// The color of the bezier curve in the Unity editor.
		/// </summary>
		public Color drawColor = Color.white;
		/// <summary>
		/// Array of point objects that make up the this curve.
		/// </summary>
		[SerializeField]
		private List<BezierPoint> points = new List<BezierPoint>();
		/// <summary>
		/// Whether this path is a closed loop or a line that starts and ends.
		/// </summary>
		[SerializeField]
		private bool close;
		/// <summary>
		/// The approximate length of the curve.
		/// </summary>
		private float length;


		#region Properties

		/// <summary>
		/// The number of mid-points to calculate between key bezier points.
		/// </summary>
		public int Resolution
		{
				get
				{
						return resolution;
				}
				set
				{
						resolution = value;
						CalculateLength();
				}
		}

		/// <summary>
		/// Get whether the bezier curve is a loop or not.
		/// </summary>
		public bool Close
		{
				get
				{
						return close;
				}
				set
				{
						if (close == value)
						{
								return;
						}
						close = value;
						CalculateLength();
				}
		}

		/// <summary>
		/// Gets a point in the array of points, but doesn't allow setting.
		/// </summary>
		public BezierPoint this[int index]
		{
				get
				{
						return points[index];
				}
		}

		/// <summary>
		/// Get a read only version of the array of bezier points.
		/// </summary>
		public ReadOnlyCollection<BezierPoint> Points
		{
				get
				{
						return points.AsReadOnly();
				}
		}

		/// <summary>
		/// The number of points that are in the points array.
		/// </summary>
		public int PointCount
		{
				get
				{
						return points.Count;
				}
		}

		/// <summary>
		/// Get the length of the curve.
		/// </summary>
		public float Length
		{
				get
				{
						return length;
				}
		}

		#endregion


		void Awake()
		{
				CalculateLength();
		}


		#region Adding / Removing points

		/// <summary>
		/// Add a bezier point to the end[index] of this curve.
		/// </summary>
		/// <param name="point">The point to add to the curve.</param>
		/// <param name="index">The index the point will be inserted. (null = end)</param>
		public void AddPoint(BezierPoint point, int? index = null)
		{
				if (index == null)
				{
						points.Add(point);
				}
				else
				{
						points.Insert(Mathf.Abs(index.Value) % PointCount, point);
				}

				point.Curve = this;
				CalculateLength();
		}


		/// <summary>
		/// Create a point given the position and add it to the end of this curve.
		/// </summary>
		/// <param name="position">The position the point will be in world space.</param>
		/// <param name="index">The index the point will be inserted. (null = end)</param>
		/// <returns>The bezier point that was created.</returns>
		public BezierPoint AddPointAt(Vector3 position, int? index = null)
		{
				GameObject pointObject = new GameObject("Point " + PointCount);
				pointObject.transform.parent = transform;
				pointObject.transform.position = position;
				BezierPoint newPoint = pointObject.AddComponent<BezierPoint>();

				AddPoint(newPoint);
				return newPoint;
		}


		/// <summary>
		/// Remove a point from the curve.
		/// </summary>
		/// <param name='point'>The point to remove.</param>
		public void RemovePoint(BezierPoint point)
		{
				points.Remove(point);
				CalculateLength();
		}


		/// <summary>
		/// Remove all of the points from the curve.
		/// </summary>
		public void ClearPoints()
		{
				points.Clear();
				CalculateLength();
		}

		#endregion


		#region Accessor Functions

		/// <summary>
		/// Get a point at t percent along this curve.
		/// </summary>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <returns>The position at 't' percent.</returns>
		public Vector3 GetPointAt(float t)
		{
				if (t <= 0 || (t >= 1 && Close))
				{
						return points[0].Position;
				}
				else if (t >= 1)
				{
						return points[PointCount - 1].Position;
				}

				float totalPercent = 0;
				float curvePercent = 0;

				BezierPoint p1 = null;
				BezierPoint p2 = null;

				int offset = !Close ? PointCount - 1 : 0;

				for (int i = 0; i < offset; i++)
				{
						int p1I = i % PointCount;
						int p2I = (p1I + 1) % PointCount;

						if (p2I == 0 && Close)
						{
								curvePercent = 1 - totalPercent;
						}
						else
						{
								curvePercent = ApproximateLength(points[p1I], points[p2I]) / Length;
						}

						if (totalPercent + curvePercent > t)
						{
								p1 = points[p1I];
								p2 = points[p2I];
								break;
						}

						totalPercent += curvePercent;
				}

				if (Close && p1 == null)
				{
						p1 = points[PointCount - 1];
						p2 = points[0];
				}
				else if (p1 == null)
				{
						p1 = points[PointCount - 1];
						p2 = points[PointCount - 1];
				}

				t -= totalPercent;

				return GetPoint(p1, p2, t / curvePercent);
		}


		/// <summary>
		/// Get the percentage along the curve a bezier point is.
		/// </summary>
		public float GetPercentAt(BezierPoint point)
		{
				if (point == points[0])
				{
						return 0;
				}
				else if ((point == points[PointCount - 1] && !Close) || (point == points[0] && Close))
				{
						return 1;
				}

				float distance = 0;

				int offset = !Close ? 1 : 0;

				for (int i = 0; i < PointCount - offset; i++)
				{
						BezierPoint p1 = points[i % PointCount];
						BezierPoint p2 = points[(i + 1) % PointCount];

						if (p1 == point)
						{
								return distance / Length;
						}

						distance += ApproximateLength(p1, p2);
				}

				// No point was found.
				return -1;
		}


		/// <summary>
		/// Get the index of the given point in this curve.
		/// </summary>
		/// <param name='point'>Point to search for.</param>
		public int GetPointIndex(BezierPoint point)
		{
				for (int i = 0; i < PointCount; i++)
				{
						if (points[i] == point)
						{
								return i;
						}
				}

				// No index was found.
				return -1;
		}

		#endregion


		#region Public Static Functions

		/// <summary>
		/// Gets the point 't' percent between two points on a curve.
		/// </summary>
		/// <param name='p1'>The bezier point at the beginning of the curve.</param>
		/// <param name='p2'>The bezier point at the end of the curve.</param>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <returns>The position 't' percent along the curve.</returns>
		public static Vector3 GetPoint(BezierPoint p1, BezierPoint p2, float t)
		{
				if (p1.Handle2 != Vector3.zero)
				{
						if (p2.Handle1 != Vector3.zero)
						{
								return GetCubicCurvePoint(p1.Position, p1.GlobalHandle2, p2.GlobalHandle1, p2.Position, t);
						}
						else
						{
								return GetQuadraticCurvePoint(p1.Position, p1.GlobalHandle2, p2.Position, t);
						}
				}
				else
				{
						if (p2.Handle1 != Vector3.zero)
						{
								return GetQuadraticCurvePoint(p1.Position, p2.GlobalHandle1, p2.Position, t);
						}
						else
						{
								return GetLinearPoint(p1.Position, p2.Position, t);
						}
				}
		}


		/// <summary>
		/// Gets the point 't' percent along a third-order curve.
		/// </summary>
		/// <param name='p1'>The bezier point at the beginning of the curve.</param>
		/// <param name='p2'>The bezier point at the end of the curve.</param>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <returns>The position 't' percent along the curve.</returns>
		public static Vector3 GetCubicCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
		{
				t = Mathf.Clamp01(t);

				Vector3 part1 = Mathf.Pow(1 - t, 3) * p1;
				Vector3 part2 = 3 * Mathf.Pow(1 - t, 2) * t * p2;
				Vector3 part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * p3;
				Vector3 part4 = Mathf.Pow(t, 3) * p4;

				return part1 + part2 + part3 + part4;
		}


		/// <summary>
		/// Gets the point 't' percent along a second-order curve.
		/// </summary>
		/// <param name='p1'>The bezier point at the beginning of the curve.</param>
		/// <param name='p2'>The bezier point at the end of the curve.</param>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <returns>The position 't' percent along the curve.</returns>
		public static Vector3 GetQuadraticCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
				t = Mathf.Clamp01(t);

				Vector3 part1 = Mathf.Pow(1 - t, 2) * p1;
				Vector3 part2 = 2 * (1 - t) * t * p2;
				Vector3 part3 = Mathf.Pow(t, 2) * p3;

				return part1 + part2 + part3;
		}


		/// <summary>
		/// Gets point 't' percent along a linear line.
		/// </summary>
		/// <param name='p1'>The bezier point at the beginning of the curve.</param>
		/// <param name='p2'>The bezier point at the end of the curve.</param>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <returns>The position 't' percent along the curve.</returns>
		public static Vector3 GetLinearPoint(Vector3 p1, Vector3 p2, float t)
		{
				return p1 + ((p2 - p1) * t);
		}


		/// <summary>
		/// Gets point 't' percent along n-order curve.
		/// </summary>
		/// <param name='t'>The percent of the curve to get the position (0-1).</param>
		/// <param name='points'>The points used to define the curve.</param>
		/// <returns>The point 't' percent along the curve.</returns>
		public static Vector3 GetPoint(float t, params Vector3[] points)
		{
				t = Mathf.Clamp01(t);

				int order = points.Length - 1;
				Vector3 point = Vector3.zero;
				Vector3 vectorToAdd;

				for (int i = 0; i < points.Length; i++)
				{
						vectorToAdd = points[points.Length - i - 1] * (BinomialCoefficient(i, order) * Mathf.Pow(t, order - i) * Mathf.Pow((1 - t), i));
						point += vectorToAdd;
				}

				return point;
		}


		/// <summary>
		/// Get the approximate length between two points.
		/// </summary>
		/// <param name='p1'>The bezier point at the start of the curve.</param>
		/// <param name='p2'>The bezier point at the end of the curve.</param>
		/// <param name='resolution'>The number of points along the curve used to create measurable segments.</param>
		public static float ApproximateLength(BezierPoint p1, BezierPoint p2, int resolution = 10)
		{
				float res = resolution;
				float total = 0;
				Vector3 lastPosition = p1.Position;
				Vector3 currentPosition;

				for (int i = 0; i < resolution + 1; i++)
				{
						currentPosition = GetPoint(p1, p2, i / res);
						total += (currentPosition - lastPosition).magnitude;
						lastPosition = currentPosition;
				}

				return total;
		}

		#endregion


		#region UtilityFunctions

		private static int BinomialCoefficient(int i, int n)
		{
				return Factoral(n) / (Factoral(i) * Factoral(n - i));
		}


		private static int Factoral(int i)
		{
				if (i == 0)
				{
						return 1;
				}

				int total = 1;

				while (i - 1 >= 0)
				{
						total *= i;
						i--;
				}

				return total;
		}


		/// <summary>
		/// Calculate the length of the curve using the full resolution.
		/// This updates the length of the curve locally when called.
		/// </summary>
		/// <returns>The length of the curve.</returns>
		public float CalculateLength()
		{
				length = 0;

				int offset = !Close ? 1 : 0;

				for (int i = 0; i < PointCount - offset; i++)
				{
						int p1I = i % PointCount;
						int p2I = (p1I + 1) % PointCount;

						length += ApproximateLength(points[p1I], points[p2I], resolution);
				}

				return length;
		}

		#endregion


		#region Display Curve Functions

		void OnDrawGizmos()
		{
				Gizmos.color = drawColor;

				if (points.Count > 1)
				{
						int offset = !Close ? 1 : 0;

						for (int i = 0; i < PointCount - offset; i++)
						{
								DrawCurve(points[i], points[(i + 1) % PointCount], resolution);
						}
				}
		}


		/// <summary>
		/// Draw the curve between two points in the editor.
		/// </summary>
		/// <param name='p1'>The first point to start drawing from</param>
		/// <param name='p2'>The second point that is being drawn to</param>
		/// <param name='resolution'>The resolution/lines that are being drawn between the two points</param>
		private static void DrawCurve(BezierPoint p1, BezierPoint p2, int resolution)
		{
				int limit = resolution + 1;
				float res = resolution;
				Vector3 lastPoint = p1.Position;
				Vector3 currentPoint = Vector3.zero;

				for (int i = 1; i < limit; i++)
				{
						currentPoint = GetPoint(p1, p2, i / res);
						Gizmos.DrawLine(lastPoint, currentPoint);
						lastPoint = currentPoint;
				}
		}

		#endregion
}