using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour {

    float movementRate = 3f;
    float zoomRate = 3f;
    float originalZoom;
    float attackZoom = 7.5f;
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
    }

    public void MoveTo(Vector3 position)
    {        
        // remove z from the equation
        var newPos = position;
        newPos.z = originalPos.z;
        position = newPos;

        //StartCoroutine(MoveCameraTo(transform.position, position));
        desiredPos = position;
    }

    public void ZoomAttack()
    {
        //StartCoroutine(ZoomCamera(originalZoom, attackZoom));
        desiredZoom = attackZoom;
    }
    public void ZoomTarget()
    {
        //StartCoroutine(ZoomCamera(attackZoom, originalZoom));
        desiredZoom = originalZoom;
    }

    //IEnumerator MoveCameraTo(Vector3 startPos, Vector3 desiredPos)
    //{
    //    for (float i = 0; i <= 1; i += Time.deltaTime * movementRate)
    //    {
    //        transform.position = Vector3.Lerp(startPos, desiredPos, i);
    //        yield return null;
    //    }
    //}
    //IEnumerator ZoomCamera(float startZoom, float desiredZoom)
    //{
    //    for (float i = 0; i <= 1; i += Time.deltaTime * zoomRate)
    //    {
    //        GetComponent<Camera>().orthographicSize = Mathf.Lerp(startZoom, desiredZoom, i);
    //        yield return null;
    //    }
    //}

    public void BackToStart()
    {
        //StartCoroutine(MoveCameraTo(transform.position, originalPos));
        desiredPos = originalPos;
    }
}
