using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{

    private bool isMoving;
    private Vector3 originPos, targetPos;
    private float timeToMove = 0.2f;
    private Tile currentTile = null;
    private Tile destinationTile = null;

    /*public float movementSpeed = 5f;
    public Transform movePoint;*/

    // Start is called before the first frame update
    void Start()
    {
        //isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);

        // if (Vector3.Distance(transform.position, movePoint.position) <= .05f) { }

        /*  if (Input.GetKey(KeyCode.W) && !isMoving)
          {
              StartCoroutine(MovePlayer(Vector3.up));
          }
          if (Input.GetKey(KeyCode.A) && !isMoving)
          {
              StartCoroutine(MovePlayer(Vector3.left));
          }
          if (Input.GetKey(KeyCode.S) && !isMoving)
          {
              StartCoroutine(MovePlayer(Vector3.down));
          }
          if (Input.GetKey(KeyCode.D) && !isMoving)
          {
              StartCoroutine(MovePlayer(Vector3.right));
          }*/

        if (!isMoving)
        {
            //determine which direction to go
            if(currentTile.underTile != null)
            {
                destinationTile = currentTile.underTile;
                StartCoroutine(MovePlayer(Vector3.down));
            }
            if (currentTile.leftTile != null)
            {
                destinationTile = currentTile.leftTile;
                StartCoroutine(MovePlayer(Vector3.down));
            }
            if (currentTile.rightTile != null)
            {
                destinationTile = currentTile.rightTile;
                StartCoroutine(MovePlayer(Vector3.down));
            }

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

        
        transform.position = destinationTile.transform.position;
        currentTile = destinationTile;

        isMoving = false;
    }
}
