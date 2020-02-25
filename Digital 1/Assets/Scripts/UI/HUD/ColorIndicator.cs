using UnityEngine;

public class ColorIndicator : MonoBehaviour
{
    public Color fillColor;
    public GameObject colorIndPanel;
    public GameObject colorIndFill;
    private UnityEngine.UI.Image fillImage;

    private Color[] testColors = new Color[] {new Color(1, 0, 0, 1), new Color(0, 1, 0, 1), new Color(0, 0, 1, 1)};

    /// <summary>
    /// Get/Set the color of the color indicator
    /// </summary>
    public Color FillColor
    {
        get
        {
            return fillColor;
        }
        set
        {
            fillColor = value;
            UpdateFill();
        }
    }

    void Start()
    {
        fillImage = colorIndFill.GetComponent<UnityEngine.UI.Image>();
        UpdateFill();
    }

    void Update()
    {
        //For testing only
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (FillColor.Equals(testColors[0]))
            {
                FillColor = testColors[1];
            }
            else if (FillColor.Equals(testColors[1]))
            {
                FillColor = testColors[2];
            }
            else if (FillColor.Equals(testColors[2]))
            {
                FillColor = testColors[0];
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (FillColor == testColors[0])
            {
                FillColor = testColors[2];
            }
            else if (FillColor == testColors[1])
            {
                FillColor = testColors[0];
            }
            else if (FillColor == testColors[2])
            {
                FillColor = testColors[1];
            }
        }
    }

    public void UpdateFill()
    {
        fillImage.color = fillColor;
    }
}
