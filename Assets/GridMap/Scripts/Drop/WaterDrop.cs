using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }


    private bool isMoving;
    private Vector3 originPos, targetPos;
    private float timeToMove = 0.2f;
    private Tile currentTile = null;
    private Tile destinationTile = null;

    // Direction the drop is currently moving in.
    private Direction direction = Direction.Down;

    /*public float movementSpeed = 5f;
    public Transform movePoint;*/

    

    public void Init(Tile currentTile)
    {
        this.isMoving = false;
        this.currentTile = currentTile;
        //Debug.Log(currentTile.underTile.transform.position);
       
        //findDestination();
    }

    // Start is called before the first frame update
    void Start()
    {
        //isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            /// Determine which direction to go.
            //Debug.Log(direction);
            if (canMoveInDirection(Direction.Down))
            {
                /// If possible move down.
                moveInDirection(Direction.Down);
                return;
            }

            if (!canMoveInDirection(Direction.Left) && !canMoveInDirection(Direction.Right))
            {
                /// Settle as stagnant water.
                currentTile._filledWater.SetActive(true);
                return;
            }

            if (direction == Direction.Down) { 
                if (canMoveInDirection(Direction.Left))
                {
                    /// If possible move left.
                    moveInDirection(Direction.Left);
                }
                else if (canMoveInDirection(Direction.Right))
                {
                    /// If possible move right.
                    moveInDirection(Direction.Right);
                }
                return;
            }
            
            /// If currently moving horizonatally, keep moving in the same direction.
            if ((direction == Direction.Left))
            {
                if (canMoveInDirection(Direction.Left))
                {
                    /// If possible move left.
                    moveInDirection(Direction.Left);
                }
                else if (canMoveInDirection(Direction.Right))
                {
                    /// If possible move right.
                    moveInDirection(Direction.Right);
                }
                return;
            }
            
            /// If currently moving horizonatally, keep moving in the same direction.
            if ((direction == Direction.Right))
            {
                if (canMoveInDirection(Direction.Right))
                {
                    /// If possible move left.
                    moveInDirection(Direction.Right);
                }
                else if (canMoveInDirection(Direction.Left))
                {
                    /// If possible move right.
                    moveInDirection(Direction.Left);
                } 
                return;
            }
            
            currentTile._filledWater.SetActive(true);
        }
    }

    private bool canMoveInDirection(Direction direction) 
    { 
        switch (direction)
        {
            case Direction.Down:
                return currentTile.underTile != null && currentTile.underTile._isPassable && !currentTile.underTile._hasWater;
            
            case Direction.Left:
                return currentTile.leftTile != null && currentTile.leftTile._isPassable && !currentTile.leftTile._hasWater;
            
            case Direction.Right:
                return currentTile.rightTile != null && currentTile.rightTile._isPassable && !currentTile.rightTile._hasWater;
            
            default:
                /// TODO: REPORT ERROR
                return false;
        }
    }

    private void moveInDirection(Direction direction) 
    { 
        currentTile._filledWater.SetActive(false);
        this.direction = direction;
        
        switch (direction) { 
            case Direction.Down:
                destinationTile = currentTile.underTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(MovePlayer(Vector3.down));
                break;
            
            case Direction.Left:
                destinationTile = currentTile.leftTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(MovePlayer(Vector3.left));
                break;
            
            case Direction.Right:
                destinationTile = currentTile.rightTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(MovePlayer(Vector3.right));
                break;
            
            case Direction.Up:
                /// TODO: REPORT ERROR
                return;
        }
    }

    private void findDestination()
    {
        //determine which direction to go
        if (currentTile.underTile != null && currentTile.underTile._isPassable)
        {
            direction = Direction.Down;
            destinationTile = currentTile.underTile;
        }
        else if (currentTile.leftTile != null && currentTile.leftTile._isPassable)
        {
            direction = Direction.Left;
            destinationTile = currentTile.leftTile;
        }
        else if (currentTile.rightTile != null && currentTile.rightTile._isPassable)
        {
            direction = Direction.Right;
            destinationTile = currentTile.rightTile;
        }
    }


    private IEnumerator MovePlayer(Vector3 direction)
    {

        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;
        targetPos = originPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(destinationTile.transform.position.x, destinationTile.transform.position.y, -1);
        originPos = transform.position;
        currentTile = destinationTile;

        isMoving = false;
    }
}
