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
    private float timeToMove = 0.2f;
    private Tile currentTile = null;
    private Tile destinationTile = null;

    // Direction the drop is currently moving in.
    private Direction direction = Direction.Down;

    public void Init(Tile currentTile)
    {
        this.isMoving = false;
        this.currentTile = currentTile;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /// Determine which direction to go.
        if (!isMoving)
        {
            /// If possible move down.
            if (canMoveInDirection(Direction.Down))
            {
                moveInDirection(Direction.Down);
                return;
            }

            /// If trapped, settle as stagnant water.
            if (!canMoveInDirection(Direction.Left) && !canMoveInDirection(Direction.Right))
            {
                currentTile._filledWater.SetActive(true);
                return;
            }

            /// Otherwise, choose a horizontal direction.
            switch (this.direction)
            {
                case Direction.Down:
                    moveInDirection(canMoveInDirection(Direction.Left) ? Direction.Left : Direction.Right);
                    return;
                case Direction.Left:
                    moveInDirection(canMoveInDirection(Direction.Left) ? Direction.Left : Direction.Right);
                    return;
                case Direction.Right:
                    moveInDirection(canMoveInDirection(Direction.Right) ? Direction.Right : Direction.Left);
                    return;
            }
        }
    }

    private bool canMoveInDirection(Direction direction) 
    { 
        Tile adj = null;
        switch (direction)
        {
            case Direction.Down:
                adj = currentTile.underTile;
                break;

            case Direction.Left:
                adj = currentTile.leftTile;
                break;
            
            case Direction.Right:
                adj = currentTile.rightTile;
                break;
            
            default:
                /// TODO: REPORT ERROR
                return false;
        }

        return adj != null && adj.isPassable() && !adj._hasWater;
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
                StartCoroutine(animateMove(Vector3.down));
                break;
            
            case Direction.Left:
                destinationTile = currentTile.leftTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(animateMove(Vector3.left));
                break;
            
            case Direction.Right:
                destinationTile = currentTile.rightTile;
                currentTile._hasWater = false;
                destinationTile._hasWater = true;
                StartCoroutine(animateMove(Vector3.right));
                break;
            
            case Direction.Up:
                /// TODO: REPORT ERROR
                return;
        }
    }

    /// Async coroutine to animate the drop moving.
    private IEnumerator animateMove(Vector3 direction)
    {
        this.isMoving = true;
        float elapsedTime = 0;
        Vector3 originPos = transform.position;
        Vector3 targetPos = originPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(destinationTile.transform.position.x, destinationTile.transform.position.y, -1);
        originPos = transform.position;
        this.currentTile = destinationTile;

        this.isMoving = false;
    }
}
