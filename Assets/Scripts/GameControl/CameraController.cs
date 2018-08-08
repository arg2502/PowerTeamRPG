using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject followTarget;
    Vector3 targetPos;
    public float moveSpeed, origMoveSpeed;
    Camera myCamera;

    // camera size calculation specific
    float verticalSize;
    float horizontalSize;

    bool outOfBoundsX;
    bool outOfBoundsY;

    float currentX, currentY;

    SpriteRenderer blackCanvas;
    Color black;
    Color clear;
    float timeToFade;

    // jump immediately to player position at beginning
    void Start()
    {
        origMoveSpeed = moveSpeed;
        myCamera = GetComponent<Camera>();

        // first we need to figure out how to know when the edge of the camera
        // is at the same position as the room limits

        // the `orthographicSize` in camera refers to half of the vertical size seen
        // ex: if size = 10, then there are 10 pixels between jethro and the top/bottom of the camera view

        // to get the horizontal size, we need to do some math with the screen's dimensions

        verticalSize = myCamera.orthographicSize;
        horizontalSize = verticalSize * Screen.width / Screen.height;
        //transform.position = new Vector3(GameControl.control.currentPosition.x, GameControl.control.currentPosition.y, transform.position.z);
        StayWithinRoomAtStart();
        

        blackCanvas = GetComponentInChildren<SpriteRenderer>();
        black = new Color(0, 0, 0, 1);
        clear = new Color(0, 0, 0, 0);
        timeToFade = 0.5f;
        blackCanvas.color = black;
        StartCoroutine(Fade());
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        FollowTarget();
        //StayWithinRoom();
	}

    void FollowTarget()
    {
        if (GameControl.control.currentCharacterState != characterControl.CharacterState.Normal)
            return;

        if (!outOfBoundsX)
            currentX = followTarget.transform.position.x;
        if (!outOfBoundsY)
            currentY = followTarget.transform.position.y;

        targetPos = new Vector3(currentX, currentY, transform.position.z);

        //if (GameControl.control.currentCharacterState != characterControl.CharacterState.Normal)
        //    moveSpeed = 100;
        //else if(moveSpeed != origMoveSpeed)
        //    moveSpeed = origMoveSpeed;

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
        if (GameControl.control.currentEntranceGateway == null) return;

        Vector3 entrance = GameControl.control.currentEntranceGateway.entrancePos;

        transform.position = new Vector3(entrance.x, entrance.y, transform.position.z);
        Vector3 currentPosition = new Vector3(GameControl.control.currentPosition.x, GameControl.control.currentPosition.y, transform.position.z);
        var currentRoom = GameControl.control.currentRoom;

        if (currentPosition.x - horizontalSize <= currentRoom.roomLimits.minX)
        {
            //transform.Translate(new Vector3(horizontalSize, 0));
            currentX = currentRoom.roomLimits.minX + horizontalSize;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            outOfBoundsX = true;
        }
        else if (currentPosition.x + horizontalSize >= currentRoom.roomLimits.maxX)
        {
            //transform.Translate(new Vector3(-horizontalSize, 0));
            currentX = currentRoom.roomLimits.maxX - horizontalSize;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            outOfBoundsX = true;
        }
        if (currentPosition.y + verticalSize >= currentRoom.roomLimits.minY)
        {
            //transform.Translate(new Vector3(0, -verticalSize));
            currentY = currentRoom.roomLimits.minY - verticalSize;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }
        else if (currentPosition.y - verticalSize <= currentRoom.roomLimits.maxY)
        {
            //transform.Translate(new Vector3(0, verticalSize));
            currentY = currentRoom.roomLimits.maxY + verticalSize;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }

    }

    public IEnumerator Fade()
    {
        if (blackCanvas.color.a < 1)
        {
            blackCanvas.color = clear;
            while (blackCanvas.color.a < 1)
            {
                //blackCanvas.color = Color.Lerp(clear, black, 2f);
                blackCanvas.color += black * (Time.deltaTime / timeToFade);
                yield return null;
            }
            blackCanvas.color = black;
        }
        else
        {
            blackCanvas.color = black;
            while (blackCanvas.color.a > 0)
            {
                //blackCanvas.color = Color.Lerp(clear, black, 2f);
                blackCanvas.color -= black * (Time.deltaTime / timeToFade);
                yield return null;
            }
            blackCanvas.color = clear;
        }
            //blackCanvas.color = Color.Lerp(black, clear, 2f);
    }
}
