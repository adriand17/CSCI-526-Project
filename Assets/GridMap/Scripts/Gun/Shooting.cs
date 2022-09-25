using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shooting : MonoBehaviour {

    public Transform rightpivot;
    public Transform firepoint;
    
    // Renders line from gun to target.
    private LineRenderer laserRenderer;
    
    private int laserDistance = 100;
    private int numberReflectMax = 3;
    private Vector3 pos = new Vector3();
    private Vector3 directLaser = new Vector3();
    
    [SerializeField] private int countLaser = 1;
    [SerializeField] private bool loopActive = false;
    private IEnumerator coroutineForDestoryLaser;
    private float waitTime = 0.5f;
    private bool lasering = false;
    public Transform Square;

    void Start() {
        laserRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetKeyDown("space") && !lasering) {
            lasering = true;
            DrawLaser();
            coroutineForDestoryLaser = WaitAndDisappear(waitTime);
            StartCoroutine(coroutineForDestoryLaser);
        }
    }

    // Stop rendering laser after set time.
    private IEnumerator WaitAndDisappear(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        laserRenderer.positionCount = 0;
        lasering = false;
    }
    
    void DrawLaser() {
        loopActive = true;
        countLaser = 1;
        pos = firepoint.position;
        directLaser = firepoint.up;
        laserRenderer.positionCount = countLaser;
        laserRenderer.SetPosition(0, pos);

        while (loopActive) {
            // Find the first opaque object hit by the laser.
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, directLaser, laserDistance);
            RaycastHit2D hit = Physics2D.Raycast(pos,directLaser,laserDistance);
            foreach (var obj in hits) {
                if (obj.collider.tag == TagConstant.ReflectWall || obj.collider.tag == TagConstant.Wall) {
                    hit = obj;
                    break;
                } else if (obj.collider.tag == TagConstant.WaterDrop) {
                    hit = obj;
                    break;
                }
            }
            
            if (!hit || !HandleHit(hit)) {
                countLaser++;
                laserRenderer.positionCount = countLaser;
                laserRenderer.SetPosition(countLaser - 1, pos + (directLaser.normalized * laserDistance));
                loopActive = false;
            }
            countLaser++; //TODO: Avoid infinite loop currently
            if (countLaser > numberReflectMax) {
                loopActive = false;
            }
        }
    }

    private bool HandleHit(RaycastHit2D hit) {
        if (hit.collider.tag == TagConstant.ReflectWall) {
            return false;
        } else if (hit.collider.tag == TagConstant.WaterDrop) {
            Particle particle = hit.collider.gameObject.GetComponent<Particle>();
            if (particle == null) { 
                Debug.Log("ERROR: Particle is null");
                return false;
            }
            switch (particle.getBlockType()) { 
                case BlockType.Water:
                    handleWaterHit(hit.collider.gameObject);
                    return true;
                
                case BlockType.Bedrock:
                    handleNonReflectLaser(hit);
                    loopActive = false;
                    return true;
                
                case BlockType.Dirt:
                    handleNonReflectLaser(hit);
                    loopActive = false;
                    return true;
                
                default:
                    Debug.Log("ERROR: Unknown block type");
                    return false;  
            }
        } else if (hit.collider.tag == TagConstant.Wall) {
            return false;
        } else { 
            Debug.Log("ERROR: Unknown tag");
            return false;
        }
    }

    void handleNonReflectLaser(RaycastHit2D hit) {
        countLaser++;
        laserRenderer.positionCount = countLaser;
        laserRenderer.SetPosition(countLaser - 1, hit.point);
    }
    
    void handleReflectLaser(RaycastHit2D hit) {
        countLaser++;
        laserRenderer.positionCount = countLaser;
        pos = (Vector2)directLaser.normalized + hit.normal;
        directLaser = Vector3.Reflect(directLaser, hit.point);
        laserRenderer.SetPosition(countLaser - 1, hit.point);
    }

    void handleWaterHit(GameObject water) {
        Particle particle = water.GetComponent<Particle>();
        if (particle == null) { 
            Debug.Log("ERROR: Particle is null");
            return;
        }
        Debug.Log(particle.tile.location);
            
        // TODO: log water position on death.
        string url = $"https://docs.google.com/forms/d/e/1FAIpQLSd02iSGLy70_8jzmnZtIZbMc4KJNCfetrs7eo3PnL4dFIE2Ww/formResponse?usp=pp_url&entry.1386653628={particle.tile.location.x}&entry.962467366={particle.tile.location.y}&entry.1845636193={particle.tile.location.z}&submit=Submit";
        StartCoroutine(GetRequest(url));
        Destroy(water);
    }

    IEnumerator GetRequest(string uri) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
