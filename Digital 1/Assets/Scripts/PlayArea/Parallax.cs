using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private RectTransform background, layer1, layer2;
    
    void Start()
    {
        
    }

    void Update()
    {
        background.transform.position = new Vector3(cam.transform.position.x, -1, cam.transform.position.z);
        layer1.transform.position = new Vector3(cam.transform.position.x / 2, -1, cam.transform.position.z / 2);
        layer2.transform.position = new Vector3(cam.transform.position.x / 3, -1, cam.transform.position.z / 3);
    }
}
