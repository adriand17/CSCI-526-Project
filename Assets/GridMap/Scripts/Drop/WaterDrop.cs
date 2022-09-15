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
        Debug.Log(currentTile.underTile.transform.position);
       
        findDestination();
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
            //determine which direction to go
            //Debug.Log(direction);
            if (currentTile.underTile != null && currentTile.underTile._isPassable)
            {
                ///if possible move down
                direction = Direction.Down;
                destinationTile = currentTile.underTile;
                StartCoroutine(MovePlayer(Vector3.down));
            }
            else if ((currentTile.underTile == null || !currentTile.underTile._isPassable)  && direction == Direction.Down)
            {
                //if cannot move down anymore move left
                //Debug.Log("cannot move down anymore");

                Debug.Log("hit the ground");
                if (currentTile.leftTile != null && currentTile.leftTile._isPassable)
                {
                    direction = Direction.Left;
                    destinationTile = currentTile.leftTile;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else if(currentTile.rightTile != null && currentTile.rightTile._isPassable)
                {
                    direction = Direction.Right;
                    destinationTile = currentTile.rightTile;
                    StartCoroutine(MovePlayer(Vector3.right));
                }

            }
            else if ((currentTile.leftTile != null && currentTile.leftTile._isPassable && direction == Direction.Left) || 
                (currentTile.rightTile != null && currentTile.rightTile._isPassable && direction == Direction.Right))
            {
                //if currently moving horizonatally, keep moving the same direction
                //Debug.Log("keep directions!");
                if (direction == Direction.Left)
                {
                    destinationTile = currentTile.leftTile;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
                else
                {
                    destinationTile = currentTile.rightTile;
                    StartCoroutine(MovePlayer(Vector3.right));
                }
               
            }
            else if (((currentTile.leftTile == null ||  (currentTile.leftTile != null && !currentTile.leftTile._isPassable)) && direction == Direction.Left) || 
                ((currentTile.rightTile == null || (currentTile.rightTile != null && !currentTile.rightTile._isPassable)) && direction == Direction.Right))
            {
                //if currently moving horizonatally, but cannot move towards that direction anymore, move towards is opposite direction
                //Debug.Log("change directions!");
                if (direction == Direction.Left)
                {
                    direction = Direction.Right;                   
                    destinationTile = currentTile.rightTile;
                    StartCoroutine(MovePlayer(Vector3.right));
                }
                else
                {
                    direction = Direction.Left;
                    destinationTile = currentTile.leftTile;
                    StartCoroutine(MovePlayer(Vector3.left));
                }
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
