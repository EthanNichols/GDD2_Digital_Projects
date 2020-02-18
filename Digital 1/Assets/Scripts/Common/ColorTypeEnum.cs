using UnityEngine;

public enum ColorType
{
		None,
		Red,
		Green,
		Blue
}

public static class ColorExtension
{
		public static Color GetColorFromColorType(this Color color, ColorType colorType)
		{
				return GetColorFromColorType(colorType);
		}

		public static Color GetColorFromColorType(ColorType color)
		{
				switch (color)
				{
						case ColorType.Red:
								return Color.red;

						case ColorType.Green:
								return Color.green;

						case ColorType.Blue:
								return Color.blue;

						case ColorType.None:
						default:
								Debug.LogWarning("Color not set or ColorType is None, Returning Color.black");
								return Color.black;
				}
		}
}