using UnityEngine;
using UnityEngine.UI;
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

    //public Image blackCanvas;
	public RawImage blackCanvas;
	public Material transitionMaterialPrefab;
	public Texture transitionEffect;
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
        transform.position = new Vector3(GameControl.control.currentPosition.x, GameControl.control.currentPosition.y, transform.position.z);
        StayWithinRoomAtStart();
        
                
        //black = new Color(0, 0, 0, 1);
        //clear = new Color(0, 0, 0, 0);
        //timeToFade = 0.5f;
        //blackCanvas.color = black;
		blackCanvas.material = new Material (transitionMaterialPrefab);
        StartCoroutine(Fade(true));
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        FollowTarget();
        StayWithinRoom();
	}

    void FollowTarget()
    {
        if (GameControl.control.currentCharacterState != characterControl.CharacterState.Normal)
            return;

        if (RoomTooSmallX())
            currentX = MidX;
        else if (!outOfBoundsX)
            currentX = followTarget.transform.position.x;
        if (RoomTooSmallY())
            currentY = MidY;
        else if (!outOfBoundsY)
            currentY = followTarget.transform.position.y;

        targetPos = new Vector3(currentX, currentY, transform.position.z);

        if (GameControl.control.currentCharacterState != characterControl.CharacterState.Normal)
            moveSpeed = 100;
        else if (moveSpeed != origMoveSpeed)
            moveSpeed = origMoveSpeed;

        //transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = targetPos;
    }

    float MidX { get { return (GameControl.control.currentRoom.roomLimits.minX + GameControl.control.currentRoom.roomLimits.maxX) / 2f; } }
    float MidY { get { return (GameControl.control.currentRoom.roomLimits.minY + GameControl.control.currentRoom.roomLimits.maxY) / 2f; } }
    bool RoomTooSmallX()
    {
        // horizontalSize -- radius of camera view in x direction
        // Mathf.Abs(minX+maxX) -- the total width of the room
        // if the diameter of the camera view is greater than the length of the room, then the entire room
        // can horizontally fit on screen -- we don't have to move the camera
        return (horizontalSize * 2 > Mathf.Abs(GameControl.control.currentRoom.roomLimits.minX) + Mathf.Abs(GameControl.control.currentRoom.roomLimits.maxX));

    }
    bool RoomTooSmallY()
    {

        // verticalSize -- radius of camera view in Y direction
        // Mathf.Abs(minY+maxY) -- the total height of the room
        // if the diameter of the camera view is greater than the height of the room, then the entire room
        // can vertically fit on screen -- we don't have to move the camera
        return (verticalSize * 2 > Mathf.Abs(GameControl.control.currentRoom.roomLimits.minY) + Mathf.Abs(GameControl.control.currentRoom.roomLimits.maxY));
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
        else if (currentPos.x + horizontalSize >= currentRoom.roomLimits.maxX)
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
        else if (currentPos.y - verticalSize <= currentRoom.roomLimits.maxY)
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

        if(RoomTooSmallX())
        {
            currentX = MidX;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }
        else if (currentPosition.x - horizontalSize <= currentRoom.roomLimits.minX)
        {
            currentX = currentRoom.roomLimits.minX + horizontalSize;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            outOfBoundsX = true;
        }
        else if (currentPosition.x + horizontalSize >= currentRoom.roomLimits.maxX)
        {
            currentX = currentRoom.roomLimits.maxX - horizontalSize;
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            outOfBoundsX = true;
        }

        if(RoomTooSmallY())
        {
            currentY = MidY;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        }
        else if (currentPosition.y + verticalSize >= currentRoom.roomLimits.minY)
        {
            currentY = currentRoom.roomLimits.minY - verticalSize;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }
        else if (currentPosition.y - verticalSize <= currentRoom.roomLimits.maxY)
        {
            currentY = currentRoom.roomLimits.maxY + verticalSize;
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
            outOfBoundsY = true;
        }

    }

    public IEnumerator Fade(bool show)
    {
		float targVal = show ? 1 : 0;
		float curVal = show ? 0 : 1;
		//float curVal = blackCanvas.material.GetFloat ("_Cutoff");
		blackCanvas.material.SetTexture ("_AlphaTex", transitionEffect);
        //if (blackCanvas.color.a < 1f)
        //{
            //blackCanvas.color = clear;
            //while (blackCanvas.color.a < 1)
            //{
            //    blackCanvas.color += black * (Time.deltaTime / timeToFade);
            //    yield return null;
            //}
            //blackCanvas.color = black;
        //}
        //else
        //{
            //blackCanvas.color = black;
            //while (blackCanvas.color.a > 0)
            //{
            //    blackCanvas.color -= black * (Time.deltaTime / timeToFade);
            //    yield return null;
            //}
            //blackCanvas.color = clear;
        //}

		while (curVal != targVal) {
			curVal = Mathf.MoveTowards(curVal, targVal, /*speed*/1f * Time.deltaTime);
			blackCanvas.material.SetFloat("_Cutoff", curVal);
			yield return null;
		}
    }
}
