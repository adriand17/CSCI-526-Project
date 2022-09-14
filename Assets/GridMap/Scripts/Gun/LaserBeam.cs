using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class LaserBeam {
    

    Vector3 dir, pos;

    GameObject laserObj;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();
    private int CountLaser = 0;
    private int laserDistance = 100;
    private int numberReflectMax = 2;
    bool loopActive = true;



    public LaserBeam(Vector3 pos, Vector3 dir) {
        this.laser = new LineRenderer();
        this.laserObj = new GameObject();
        this.laserObj.name = "Laser Beam";
        this.pos = pos;
        this.dir = dir;

        this.laser = this.laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        //this.laser.material = material;
        this.laser.startColor = Color.green;
        this.laser.endColor = Color.green;
        CastRay(pos, dir, laser);
        //CastRay2D(pos, dir, laser);
    }

    void CastRay2D(Vector3 pos, Vector3 dir, LineRenderer laserRenderer) {
        laserIndices.Add(pos); 
        Debug.Log("LASER START ");
        while (loopActive) {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)pos, (Vector2)dir, laserDistance);
            Debug.Log((Vector2)pos + " "  + (Vector2)dir);
            
            if (hit)
            {
                Debug.Log("Hit inside");
                CountLaser++;
                laserRenderer.positionCount = CountLaser;
                pos = (Vector2)dir.normalized + hit.point;
                dir = Vector3.Reflect(dir, hit.point);
                laserRenderer.SetPosition(CountLaser - 1, pos);

            }
            else
            {
                Debug.Log("Hit inside2");
                CountLaser++;
                laserRenderer.positionCount = CountLaser;
                laserRenderer.SetPosition(CountLaser - 1, pos + (dir.normalized * laserDistance));
                loopActive = false;


            }
            UpdateLaser();
            if (CountLaser > numberReflectMax)
            {
                loopActive = false;
            }
        }
    }



    void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser) {
        laserIndices.Add(pos); 

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30, 1)) {
            Debug.Log("Hit inside");
            checkHit(hit, dir, laser);
        } else {
            laserIndices.Add(ray.GetPoint(30));
            UpdateLaser();
        }
    }

    void UpdateLaser() {
        int count = 0;
        laser.positionCount = laserIndices.Count;
        foreach (Vector3 idx in laserIndices) {
            laser.SetPosition(count, idx);
            count ++;
        }
    }

    void checkHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser) {
        if (hitInfo.collider.gameObject.tag == "Mirror" && CountLaser < numberReflectMax) {
            CountLaser ++;
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(pos, dir, laser);
        } else {
            laserIndices.Add(hitInfo.point); 
            UpdateLaser();
        }
    }


}