using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    
    public float panSpeed = 0.2f;
    public float movementTime =1f;
    public float relativePanBorderThickneess = 0.1f;
    public float scrollSpeed = 5f;
    public float rotSpeed = 0.5f;
    public Vector3 zoomSpeed;

    private float panBorderThicknessHeight;
    private float panBorderThicknessWidth;
    private float minY = -100;
    private float maxY = 100;
    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;


    void Start()
    {
        panBorderThicknessHeight = (Screen.height * relativePanBorderThickneess);
        panBorderThicknessWidth = (Screen.width * relativePanBorderThickneess);
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            this.enabled = false;
        }

        HandleMouseInput();
        HandleKeyboardInput();
    }

    void HandleKeyboardInput()
    {

        // Movement
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))// || Input.mousePosition.y >= Screen.height - panBorderThicknessHeight)
        {
            //transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            newPosition += transform.forward * panSpeed;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))// || Input.mousePosition.y <= panBorderThicknessHeight)
        {
            //transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            newPosition += transform.forward * -panSpeed;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))// || Input.mousePosition.x >= Screen.width - panBorderThicknessWidth)
        {
            //transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            newPosition += transform.right * panSpeed;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))// || Input.mousePosition.x <= panBorderThicknessWidth)
        {
            //transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            newPosition += transform.right * -panSpeed;
        }

        // Zoom

        if (Input.GetKey("r") && cameraTransform.localPosition.y >= minY)
        {
            newZoom += zoomSpeed;
        }

        if (Input.GetKey("f") && cameraTransform.localPosition.y <= maxY)
        {
            newZoom -= zoomSpeed;
        }

        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //newZoom.y -= scroll * scrollSpeed * Time.deltaTime * 2000;
        //newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);

        // Rotation
        if (Input.GetKey("q"))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotSpeed);
        }

        if (Input.GetKey("e"))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotSpeed);
        }

        transform.position = newPosition;
        transform.rotation = newRotation;
        cameraTransform.localPosition = newZoom;
    }

    void HandleMouseInput()
    {
        // Scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            if(Input.mouseScrollDelta.y < 0 && cameraTransform.position.y <= maxY)
            {
                newZoom += Input.mouseScrollDelta.y * zoomSpeed;
            }
            if (Input.mouseScrollDelta.y > 0 && cameraTransform.position.y >= minY)
            {
                newZoom += Input.mouseScrollDelta.y * zoomSpeed;
            }
        }

        // Drag Movement
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        // Rotate
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition- rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }
}
