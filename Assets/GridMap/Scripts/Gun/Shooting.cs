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
    private int numberReflectMax = 3;
    private Vector3 pos = new Vector3();
    private Vector3 directLaser = new Vector3();
    
    [SerializeField] private int CountLaser = 1;
    [SerializeField] private bool loopActive = false;
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
            loopActive = true;
            CountLaser = 1;
            pos = firepoint.position;
            directLaser = firepoint.up;
            laserRenderer.positionCount = CountLaser;
            laserRenderer.SetPosition(0, pos);
   
            while (loopActive)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, directLaser, laserDistance);
                if (!hit || !HandleHit(hit)) {
                    CountLaser++;
                    laserRenderer.positionCount = CountLaser;
                    laserRenderer.SetPosition(CountLaser - 1, pos + (directLaser.normalized * laserDistance));
                    loopActive = false;
                }
                CountLaser++; //TODO: Avoid infinite loop currently
                if (CountLaser > numberReflectMax)
                {
                    loopActive = false;
                }
            }

        }


        private bool HandleHit(RaycastHit2D hit) {
            bool handled = false;
            if (hit.collider.tag == TagConstant.ReflectWall) {
                    CountLaser++;
                    laserRenderer.positionCount = CountLaser;
                    pos = (Vector2)directLaser.normalized + hit.normal;
                    directLaser = Vector3.Reflect(directLaser, hit.point);
                    laserRenderer.SetPosition(CountLaser - 1, hit.point);
                    handled = true;
            } else if (hit.collider.tag == TagConstant.WaterDrop) {
                    Debug.Log("Hit water");
                    handleWaterHit();                    
            } else if (hit.collider.tag == TagConstant.Wall) {
                    Debug.Log("Hit Wall");
                    handled = true;
                    loopActive = false;
            }
            
            return handled;
        }

        void handleWaterHit() {

        }
    }
