using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 moveDirection;
    public Vector2 cameraBounds;

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(0, 0, 0);
        if (Input.anyKey)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(key))
                {
                    if (key == KeyCode.W)
                    {
                        moveDirection += new Vector3(0, 1, 0);
                    }
                    else if (key == KeyCode.A)
                    {
                        moveDirection += new Vector3(-1, 0, 0);
                    }
                    else if (key == KeyCode.S)
                    {
                        moveDirection += new Vector3(0, -1, 0);
                    }
                    else if (key == KeyCode.D)
                    {
                        moveDirection += new Vector3(1, 0, 0);
                    }
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Camera>().orthographicSize -= 0.5f;
            if (GetComponent<Camera>().orthographicSize < 1f)
            {
                GetComponent<Camera>().orthographicSize = 1f;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Camera>().orthographicSize += 0.5f;
            if (GetComponent<Camera>().orthographicSize > 15f)
            {
                GetComponent<Camera>().orthographicSize = 15f;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + moveDirection, ref velocity, 0.2f);
        float xPos = Mathf.Clamp(transform.position.x, -cameraBounds.x, cameraBounds.x);
        float yPos = Mathf.Clamp(transform.position.y, -cameraBounds.y, cameraBounds.y);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
