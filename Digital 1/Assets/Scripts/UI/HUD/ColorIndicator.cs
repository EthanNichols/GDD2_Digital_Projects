using UnityEngine;
using UnityEngine.UI;

public class ColorIndicator : MonoBehaviour
{
    [SerializeField] private GameObject colorIndPanel;
    [SerializeField] private GameObject colorIndFill;
    [SerializeField] private Color fillColor;

    private Image fillImage;

    /// <summary>
    /// The current color the indicator displays
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
        fillImage = colorIndFill.GetComponent<Image>();
        UpdateFill();
    }

    void Update()
    {

    }

    /// <summary>
    /// Update the color value of the color indicator fill
    /// </summary>
    public void UpdateFill()
    {
        fillImage.color = fillColor;
    }
}
