using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject followTarget;
    Vector3 targetPos;
    public float moveSpeed;
    Camera myCamera;

    // camera size calculation specific
    float verticalSize;
    float horizontalSize;

    bool outOfBoundsX;
    bool outOfBoundsY;

    float currentX, currentY;

    // jump immediately to player position at beginning
    void Start()
    {
        myCamera = GetComponent<Camera>();

        // first we need to figure out how to know when the edge of the camera
        // is at the same position as the room limits

        // the `orthographicSize` in camera refers to half of the vertical size seen
        // ex: if size = 10, then there are 10 pixels between jethro and the top/bottom of the camera view

        // to get the horizontal size, we need to do some math with the screen's dimensions

        verticalSize = myCamera.orthographicSize;
        horizontalSize = verticalSize * Screen.width / Screen.height;        
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        FollowTarget();
        StayWithinRoom();
	}

    void FollowTarget()
    {
        if (!outOfBoundsX)
            currentX = followTarget.transform.position.x;
        if (!outOfBoundsY)
            currentY = followTarget.transform.position.y;

        targetPos = new Vector3(currentX, currentY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void StayWithinRoom()
    {
        //var currentPos = transform.position;
        var currentPos = followTarget.transform.position;
        var currentRoom = GameControl.control.currentRoom;

        if (currentRoom == null) return;

        outOfBoundsX = false;
        outOfBoundsY = false;

        // check left
        if (currentPos.x - horizontalSize <= currentRoom.roomLimits.minX)
        {
            currentX = currentRoom.roomLimits.minX + horizontalSize;
            targetPos = new Vector3(currentX, targetPos.y, transform.position.z);
            outOfBoundsX = true;
        }

        // check right
        if (currentPos.x + horizontalSize >= currentRoom.roomLimits.maxX)
        {
            currentX = currentRoom.roomLimits.maxX - horizontalSize;
            targetPos = new Vector3(currentX, targetPos.y, transform.position.z);
            outOfBoundsX = true;
        }

        // check up
        if (currentPos.y + verticalSize >= currentRoom.roomLimits.minY)
        {
            currentY = currentRoom.roomLimits.minY - verticalSize;
            targetPos = new Vector3(targetPos.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }

        // check down
        if (currentPos.y - verticalSize <= currentRoom.roomLimits.maxY)
        {
            currentY = currentRoom.roomLimits.maxY + verticalSize;
            targetPos = new Vector3(targetPos.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }
    }

    public void StayWithinRoomAtStart()
    {
        transform.position = new Vector3(GameControl.control.currentPosition.x, GameControl.control.currentPosition.y, transform.position.z);
        var currentPos = transform.position;
        var currentRoom = GameControl.control.currentRoom;

        if (currentPos.x - horizontalSize <= currentRoom.roomLimits.minX)
        {
            transform.Translate(new Vector3(horizontalSize, 0));
            currentX = currentRoom.roomLimits.minX + horizontalSize;
            outOfBoundsX = true;
        }
        if (currentPos.x + horizontalSize >= currentRoom.roomLimits.maxX)
        {
            transform.Translate(new Vector3(-horizontalSize, 0));
            currentX = currentRoom.roomLimits.maxX - horizontalSize;
            outOfBoundsX = true;
        }
        if (currentPos.y + verticalSize >= currentRoom.roomLimits.minY)
        {
            transform.Translate(new Vector3(-verticalSize, 0));
            currentY = currentRoom.roomLimits.minY - verticalSize;
            outOfBoundsY = true;
        }
        if (currentPos.y - verticalSize <= currentRoom.roomLimits.maxY)
        {
            transform.Translate(new Vector3(verticalSize, 0));
            currentY = currentRoom.roomLimits.maxY + verticalSize;
            outOfBoundsY = true;
        }
    }
}
