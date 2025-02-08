using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Zoom : MonoBehaviour
{


    private float zoom;
    [SerializeField] bool scrolled;
    private Vector3 worldPosition;
    private Vector3 screenPosition;
    private Vector3 zoomMove;
    [SerializeField] private float zoomMultiplier = 4f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private float smoothTime = 0.25f;

    [SerializeField] private Camera cam;

    [SerializeField] private float movementMultiplier = 0.1f;
    [SerializeField] private float widthLimit;
    [SerializeField] private float heightLimit;


    private float zoomMovMult;
    private (float, float) centre;
    private (float, float) size;

    // Start is called before the first frame update
    void Start()
    {
        zoom = cam.orthographicSize;
        scrolled = false;
        
    }

    public void Setup(Grid<ShowdownGridObject> grid)
    {
        size = ((float)grid.GetWidth()*grid.GetSize()/2, (float)grid.GetHeight()*grid.GetSize()/2);
        Vector3 temp = grid.GetWorldPosition(0, 0);
        centre = (temp.x + grid.GetWidth()/2*grid.GetSize(), temp.y + grid.GetHeight() / 2 * grid.GetSize());

        //If size + limit would restrict seeing the entire board at once (if maxZoom is normally large enough), change limit
        if ((size.Item1 + widthLimit) * Screen.height / Screen.width < size.Item2 && maxZoom >= size.Item2)
        {
            widthLimit = size.Item2 * Screen.width/Screen.height - size.Item1;
        }
        else if (size.Item2 + heightLimit < size.Item1 && maxZoom>= size.Item1)
        {
            heightLimit = heightLimit + size.Item1 - (size.Item2 + heightLimit);
        }

        //If maxZoom would see beyond the camera borders (size + limit), reduce maxZoom 
        if (maxZoom * 2 * Screen.width > (size.Item1 + widthLimit)*2)
        {
            maxZoom = (size.Item1 + widthLimit) * Screen.height /  Screen.width;
        }
        if (maxZoom * 2 > (size.Item2+ heightLimit)*2)
        {
            maxZoom = size.Item2 + heightLimit;
        }
        if (cam.orthographicSize > maxZoom)
        {
            cam.orthographicSize = maxZoom;
        }
        cam.transform.position = new Vector3(centre.Item1, centre.Item2, cam.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        //Zoom in towards mouse/Zoom out away from mouse via: Get screen position of mouse, Get world position of mouse, Zoom, Compare world pos of previous mouse screen pos, Move camera by offset amount

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            scrolled = true;
            zoom -= scroll * zoomMultiplier;
            if (zoom < 0)
            {
                Vector3 temp = Input.mousePosition;
                screenPosition = new Vector3(Screen.width / 2 + (Screen.width / 2 - temp.x), Screen.height / 2 + (Screen.height / 2 - temp.y), cam.transform.position.z);
                temp = cam.ScreenToWorldPoint(temp);
                worldPosition = new Vector3(cam.transform.position.x + (cam.transform.position.x - temp.x), cam.transform.position.y + (cam.transform.position.y - temp.y), cam.transform.position.z);
            }
            else
            {
                screenPosition = Input.mousePosition;
                worldPosition = cam.ScreenToWorldPoint(screenPosition);
            }
        }



        //Zoom smoothing
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);


        //Camera movement with directional/wasd keys
        float xAxisValue = Input.GetAxisRaw("Horizontal");
        float zAxisValue = Input.GetAxisRaw("Vertical");

        if (xAxisValue != 0 || zAxisValue != 0)
        {
            zoomMove = new Vector3(0, 0, 0);
            scrolled = false;
        }
        else
        {
            zoomMove = new Vector3(worldPosition.x - cam.ScreenToWorldPoint(screenPosition).x, worldPosition.y - cam.ScreenToWorldPoint(screenPosition).y, 0);
        }

        //Multiplier to move camera more when zoomed out
        zoomMovMult = (float) (Math.Sqrt((double) cam.orthographicSize)/ Math.Sqrt((double)minZoom));

        
        
        //Checks to stop camera from seeing out of bounds (size + limit)
        if (cam.transform.position.x + (cam.orthographicSize *  Screen.width / Screen.height) + xAxisValue * movementMultiplier * zoomMovMult  >= centre.Item1 + size.Item1 + widthLimit)
        {
            cam.transform.position = new Vector3( centre.Item1 + size.Item1 + widthLimit - cam.orthographicSize * Screen.width / Screen.height, cam.transform.position.y, cam.transform.position.z);
            if (xAxisValue > 0)
            {
                xAxisValue = 0;
            }
            if (zoomMove.x > 0)
            {
                zoomMove.x = 0;
            }
        }
        else if (cam.transform.position.x - (cam.orthographicSize * Screen.width / Screen.height) + xAxisValue * movementMultiplier * zoomMovMult <= centre.Item1 - size.Item1 - widthLimit)
        {
            cam.transform.position = new Vector3(centre.Item1 - size.Item1 - widthLimit + cam.orthographicSize * Screen.width / Screen.height, cam.transform.position.y, cam.transform.position.z);
            if (xAxisValue < 0)
            {
                xAxisValue = 0;
            }
            if (zoomMove.x < 0)
            {
                zoomMove.x = 0;
            }
        }
        if (cam.transform.position.y + cam.orthographicSize+ zAxisValue * movementMultiplier * zoomMovMult  >= centre.Item2 + size.Item2 + heightLimit)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, centre.Item2 + size.Item2 + heightLimit - cam.orthographicSize, cam.transform.position.z);
            if (zAxisValue > 0)
            {
                zAxisValue = 0;
            }
            if (zoomMove.y > 0)
            {
                zoomMove.y = 0;
            }
        }
        else if (cam.transform.position.y - cam.orthographicSize + zAxisValue * movementMultiplier * zoomMovMult <= centre.Item2 - size.Item2 - heightLimit)
        {
            cam.transform.position= new Vector3(cam.transform.position.x, centre.Item2 - size.Item2 - heightLimit + cam.orthographicSize, cam.transform.position.z);
            if (zAxisValue < 0)
            {
                zAxisValue = 0;
            }
            if (zoomMove.y < 0)
            {
                zoomMove.y = 0;
            }
        }

        if (xAxisValue != 0 || zAxisValue != 0)
        {
            //Move camera from directional/wasd key inputs
            cam.transform.Translate(new Vector3(xAxisValue * movementMultiplier * zoomMovMult, zAxisValue * movementMultiplier * zoomMovMult, 0.0f));
        }
        else if (scrolled && zoomMove.magnitude > 0)
        {
            //Offset camera for Zoom towards/away from point
            cam.transform.Translate (zoomMove);
        }
    }

}
