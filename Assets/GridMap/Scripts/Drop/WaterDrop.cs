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
            if (currentTile.underTile != null && currentTile.underTile._isPassable && !currentTile.underTile._hasWater)
            {
                /// Lower tile is clear, should start falling.
                
                currentTile._filledWater.SetActive(false);
                
                /// If possible move down.
                direction = Direction.Down;
                destinationTile = currentTile.underTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(MovePlayer(Vector3.down));
            }
            else if ((currentTile.underTile == null || !currentTile.underTile._isPassable) && direction == Direction.Down)
            {
                /// If cannot move down anymore, then move left.
                //Debug.Log("cannot move down anymore");

                //Debug.Log("hit the ground");
                if (currentTile.leftTile != null && currentTile.leftTile._isPassable && !currentTile.leftTile._hasWater)
                {
                    currentTile._filledWater.SetActive(false);
                    direction = Direction.Left;
                    destinationTile = currentTile.leftTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else if (currentTile.rightTile != null && currentTile.rightTile._isPassable && !currentTile.rightTile._hasWater)
                {
                    currentTile._filledWater.SetActive(false);
                    direction = Direction.Right;
                    destinationTile = currentTile.rightTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.right));
                }
                else
                {
                    currentTile._filledWater.SetActive(true);
                }

            }
            else if ((currentTile.leftTile != null && currentTile.leftTile._isPassable && direction == Direction.Left && !currentTile.leftTile._hasWater) ||
                (currentTile.rightTile != null && currentTile.rightTile._isPassable && direction == Direction.Right && !currentTile.rightTile._hasWater))
            {
               
                /// If currently moving horizonatally, keep moving in the same direction.
                //Debug.Log("keep directions!");
                if (direction == Direction.Left)
                {
                    currentTile._filledWater.SetActive(false);
                    destinationTile = currentTile.leftTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else
                {
                    currentTile._filledWater.SetActive(false);
                    destinationTile = currentTile.rightTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.right));
                }
            }
            else if ((currentTile.leftTile == null || (currentTile.leftTile != null && (!currentTile.leftTile._isPassable || currentTile.leftTile._hasWater)) && direction == Direction.Left) ||
                ((currentTile.rightTile == null || (currentTile.rightTile != null && (!currentTile.rightTile._isPassable || currentTile.leftTile._hasWater)) && direction == Direction.Right)))
            {
                /// If currently moving horizonatally, but cannot move towards that direction anymore, move in the opposite direction.
                //Debug.Log("change directions!");
                if (direction == Direction.Left && currentTile.rightTile != null && currentTile.rightTile._isPassable)
                {
                    currentTile._filledWater.SetActive(false);
                    direction = Direction.Right;
                    destinationTile = currentTile.rightTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.right));
                }
                else if (currentTile.leftTile != null && currentTile.leftTile._isPassable)
                {
                    currentTile._filledWater.SetActive(false);
                    direction = Direction.Left;
                    destinationTile = currentTile.leftTile;
                    currentTile._hasWater = false;
                    destinationTile._hasWater = true;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else
                {
                    currentTile._filledWater.SetActive(true);
                }
            }
            else
            {
                currentTile._filledWater.SetActive(true);
            }
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
