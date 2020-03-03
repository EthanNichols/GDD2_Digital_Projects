using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject canvas;
    public bool visible;
    public bool spinning;
    public float degPerSecond;
    public int chargeButton;
    public float chargePerSecond;
    public float maxCharge;
    public Color defaultColor;
    public Color chargeColor;
    public float retractionTime;

    private RectTransform canvasTransform;
    private bool growing;
    private float scale = 1;
    private float retractionIncrement = 1;
    private UnityEngine.UI.Image image;

    void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
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

    public Color CalculateChargeColor(float mix)
    {
        return new Color(defaultColor.r + (mix * (chargeColor.r - defaultColor.r)),
                        defaultColor.g + (mix * (chargeColor.g - defaultColor.g)),
                        defaultColor.b + (mix * (chargeColor.b - defaultColor.b)),
                        defaultColor.a + (mix * (chargeColor.a - defaultColor.a)));
    }
}
