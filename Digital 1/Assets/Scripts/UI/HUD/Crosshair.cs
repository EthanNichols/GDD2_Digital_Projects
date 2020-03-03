using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private bool visible;
    [SerializeField] private bool spinning;
    [SerializeField] private float degPerSecond;
    [SerializeField] private int chargeButton;
    [SerializeField] private float chargePerSecond;
    [SerializeField] private float maxCharge;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color chargeColor;
    [SerializeField] private float retractionTime;

    private RectTransform canvasTransform;
    private bool growing;
    private float scale = 1;
    private float retractionIncrement = 1;
    private Image image;

    /// <summary>
    /// Show or hide the crosshair
    /// </summary>
    public bool Visible
    {
        get
        {
            return visible;
        }
        set
        {
            this.visible = value;
        }
    }
    /// <summary>
    /// When spinning is true, the crosshair will rotate <code>DegreesPerSecond</code> degrees per second
    /// </summary>
    public bool Spinning
    {
        get
        {
            return spinning;
        }
        set
        {
            this.spinning = value;
        }
    }
    /// <summary>
    /// The degrees the crosshair will rotate every second
    /// </summary>
    public float DegreesPerSecond
    {
        get
        {
            return degPerSecond;
        }
        set
        {
            degPerSecond = value;
        }
    }
    /// <summary>
    /// The amount the crosshair will charge every second (the change in scale every second, shift to charge color, etc.)
    /// </summary>
    public float ChargePerSecond
    {
        get
        {
            return chargePerSecond;
        }
        set
        {
            this.chargePerSecond = value;
        }
    }
    /// <summary>
    /// The maximum scale that the crosshair will grow to when charged/held
    /// </summary>
    public float MaxCharge
    {
        get
        {
            return maxCharge;
        }
        set
        {
            this.maxCharge = value;
        }
    }
    /// <summary>
    /// The color the crosshair displays in its neutral state
    /// </summary>
    public Color DefaultColor
    {
        get
        {
            return defaultColor;
        }
        set
        {
            this.defaultColor = value;
        }
    }
    /// <summary>
    /// The color the crosshair displays when the fire button is held and charged
    /// </summary>
    public Color ChargeColor
    {
        get
        {
            return chargeColor;
        }
        set
        {
            this.chargeColor = value;
        }
    }
    /// <summary>
    /// The time it takes for the crosshair to scale back down to its original value after being charged
    /// </summary>
    public float RetractionTime
    {
        get
        {
            return retractionTime;
        }
        set
        {
            retractionTime = value;
        }
    }

    void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        gameObject.SetActive(visible);

        Vector3 mousePos = Input.mousePosition;
        Vector3 canvasPos = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0) - new Vector3(0.5f, 0.5f, 0);
        transform.localPosition = Vector3.Scale(canvasPos, canvasTransform.sizeDelta);

        if (spinning)
        {
            transform.localRotation = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z - (degPerSecond / 60f));
        }
        else if (transform.localRotation.z != 0)
        {
            transform.localRotation = Quaternion.identity;
        }

        if (Input.GetMouseButton(chargeButton))
        {
            scale += chargePerSecond / 60f;
            image.color = CalculateChargeColor(scale / maxCharge);
            if (scale > maxCharge)
            {
                scale = maxCharge;
                image.color = chargeColor;
            }
            retractionIncrement = scale / retractionTime / 60f;
        }
        else if (scale > 1)
        {
            scale -= retractionIncrement;
            image.color = CalculateChargeColor(scale / maxCharge);
            if (scale < 1)
            {
                scale = 1;
                image.color = defaultColor;
            }
        }

        transform.localScale = new Vector3(scale, scale, 1);
    }

    /// <summary>
    /// Calculate the color of the crosshair based on how far charged it is. 
    /// The closer <code>mix</code> is to 0, the closer the color will be to <code>DefaultColor</code>.
    /// The closer <code>mix</code> is to 1, the closer the color will be to <code>ChargeColor</code>
    /// </summary>
    /// <param name="mix"></param>
    /// <returns></returns>
    public Color CalculateChargeColor(float mix)
    {
        return new Color(defaultColor.r + (mix * (chargeColor.r - defaultColor.r)),
                        defaultColor.g + (mix * (chargeColor.g - defaultColor.g)),
                        defaultColor.b + (mix * (chargeColor.b - defaultColor.b)),
                        defaultColor.a + (mix * (chargeColor.a - defaultColor.a)));
    }
}
