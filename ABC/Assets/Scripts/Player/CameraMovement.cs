using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity;
    public GameObject cam;
    GameObject player;
    float xMouse;
    float yMouse;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        CameraMovements();
    }


    void Inputs()
    {
        //Camera Inputs
        xMouse += Input.GetAxis("Mouse X") * sensitivity;

        yMouse += Input.GetAxis("Mouse Y") * sensitivity;
        yMouse = Mathf.Clamp(yMouse, -90, 90);

    }

    void CameraMovements()
    {
        cam.transform.rotation = Quaternion.Euler(-yMouse, xMouse, 0);

        player.transform.rotation = Quaternion.Euler(0, xMouse, 0);
    }

}
