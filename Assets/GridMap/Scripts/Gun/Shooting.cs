using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shooting : MonoBehaviour
{
  
    public Transform firepoint;
    public GameObject bulletPreFab;
    public float bulletForce=20f;
    private float rotationSpeed = 20f;
    public Material material;
    LaserBeam laser;

    private IEnumerator coroutine;


    
    void Update()
    {
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        transform.Rotate(0, 0, -rotation);


        if(Input.GetButtonDown("Fire1"))
        {
            DrawLaser();
        }
        
    }

    void ShootBullet() {
        GameObject bullet=Instantiate(bulletPreFab,firepoint.position,firepoint.rotation);
        Rigidbody2D rb=bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firepoint.up*bulletForce,ForceMode2D.Impulse);
    }

    void DrawLaser() {
        laser = new LaserBeam(firepoint.position, firepoint.up);
        coroutine = WaitAndDisappear(1.0f);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator WaitAndDisappear(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Destroy");
        Destroy(GameObject.Find("Laser Beam"));
    }



}
