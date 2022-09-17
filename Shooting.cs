using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{


    public Transform rightpivot;
    public Transform firepoint;
    public GameObject bulletPreFab;
    public float bulletForce=20f;
    private LineRenderer laserRenderer;
    private int laserDistance = 100;
    private int numberReflectMax = 2;
    private Vector3 pos = new Vector3();
    private Vector3 directLaser = new Vector3();
    
    public int CountLaser = 1;
    public Transform Square;

    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();

    }



    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetButton("Fire2"))
        {
            DrawLaser();
        }
    }


        void Shoot()
        {
           

            GameObject bullet = Instantiate(bulletPreFab, firepoint.position, firepoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firepoint.up * bulletForce, ForceMode2D.Impulse);

        }
        void DrawLaser()
        {
        bool loopActive = true;
        int CountLaser = 1;
        pos = transform.position;
            directLaser = Camera.main.WorldToScreenPoint(Input.mousePosition - pos);
            laserRenderer.positionCount = CountLaser;
            laserRenderer.SetPosition(0, pos);
            float angle = Mathf.Atan2(directLaser.y, directLaser.x)*180/Mathf.PI;
            laserRenderer.SetPosition(1, Square.position + new Vector3(Mathf.Cos(angle) * 100, Mathf.Sin(angle) * 100, 0) * 100);
            Square.transform.rotation = Quaternion.AngleAxis(angle,new Vector3(0,0,1)) ;
   
            while (loopActive)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, directLaser, laserDistance);
                if (hit)
                {
                    CountLaser++;
                    laserRenderer.positionCount = CountLaser;
                    pos = (Vector2)directLaser.normalized + hit.normal;
                    directLaser = Vector3.Reflect(directLaser, hit.point);
                    laserRenderer.SetPosition(CountLaser - 1, hit.point);

                }
                else
                {
                    CountLaser++;
                    laserRenderer.positionCount = CountLaser;
                    laserRenderer.SetPosition(CountLaser - 1, pos + (directLaser.normalized * laserDistance));
                    loopActive = false;


                }
                if (CountLaser > numberReflectMax)
                {
                    loopActive = false;
                }
            }

        }

    }
