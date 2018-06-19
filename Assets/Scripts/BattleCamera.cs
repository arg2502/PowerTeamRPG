using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

    float movementRate = 5f;
    float zoomRate = 5f;
    float originalZoom;
    float attackZoom = 12f;
    Vector3 originalPos;

    Vector3 desiredPos;
    float desiredZoom;

    public Camera thisCamera;

    void Start()
    {
        originalPos = transform.position;
        desiredPos = originalPos;
        originalZoom = thisCamera.orthographicSize;
        desiredZoom = originalZoom;
    }

    /// <summary>
    /// Always Lerping to it's desired position. Whenever you want to change the zoom or move the camera, just change the desiredPos/desiredZoom
    /// </summary>
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPos, movementRate * Time.deltaTime);
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, desiredZoom, zoomRate * Time.deltaTime);

        // just so we're not travelling into infinity, if the position and zoom get really close to their desired positions, just set them
        if (Vector3.Magnitude(transform.position - desiredPos) < 0.01f)
            transform.position = desiredPos;
        if (Mathf.Abs(thisCamera.orthographicSize - desiredZoom) < 0.01f)
            thisCamera.orthographicSize = desiredZoom;
    }

    public void MoveTo(Vector3 position)
    {        
        // remove z from the equation
        var newPos = position;
        newPos.z = originalPos.z;
        position = newPos;
        
        desiredPos = position;
    }

    public void ZoomAttack()
    {
        desiredZoom = attackZoom;
    }
    public void ZoomTarget()
    {
        desiredZoom = originalZoom;
    }
    public void BackToStart()
    {
        desiredPos = originalPos;
    }
}
