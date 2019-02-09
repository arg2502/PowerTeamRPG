using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour {

    public BoxCollider2D triggerCollider;
    public BoxCollider2D boxCollider;
    Vector2 origBoxColliderSize;
    Vector2 origBoxColliderOffset;

    public enum Direction { UP, DOWN, LEFT, RIGHT }
    public Direction direction;

    private void Start()
    {
        origBoxColliderSize = boxCollider.size;
        origBoxColliderOffset = boxCollider.offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MovableOverworldObject>())
        {
            float start;
            float blockPos;

            if(direction == Direction.UP)
            {
                start = boxCollider.bounds.min.y;
                blockPos = collision.transform.position.y - collision.bounds.extents.y;
            }
            else if(direction == Direction.DOWN)
            {
                start = boxCollider.bounds.max.y;
                blockPos = collision.transform.position.y + collision.bounds.extents.y;
            }
            else if (direction == Direction.LEFT)
            {
                start = boxCollider.bounds.min.x;
                blockPos = collision.transform.position.x - collision.bounds.extents.x;
            }          
            else
            {
                start = boxCollider.bounds.max.x;
                blockPos = collision.transform.position.x + collision.bounds.extents.x;
            }
              
            var newSize = blockPos - start;


            if (direction == Direction.UP || direction == Direction.DOWN)
            {
                boxCollider.size = new Vector2(boxCollider.size.x, Mathf.Abs(newSize));
                boxCollider.offset = new Vector2(boxCollider.offset.x, newSize / 2);
            }
            else
            {
                boxCollider.size = new Vector2(Mathf.Abs(newSize), boxCollider.size.y);
                boxCollider.offset = new Vector2(newSize / 2, boxCollider.offset.y);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<MovableOverworldObject>())
        {
            boxCollider.size = origBoxColliderSize;
            boxCollider.offset = origBoxColliderOffset;
        }
    }

}
