using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public Transform playerTf;
    private float xRotation;
    private float yRotation;
    public float mouseSensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime / Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime / Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -67.5f, 67.5f);

        yRotation += mouseX;

        playerTf.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
