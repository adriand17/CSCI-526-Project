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
    
    // Furthest distance the laser can reach.
    private static int maxRange = 100;
    
    // Maximum number of times laser can bounce.
    private static int maxReflections = 3;
    
    private Vector3 pos = new Vector3();

    private Vector3 directLaser = new Vector3();
    
    [SerializeField] private int countLaser = 1;
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
        countLaser = 1;
        pos = firepoint.position;
        directLaser = firepoint.up;
        // List of positions for line renderer to draw.
        List<Vector3> positions = new List<Vector3>();
        positions.Add(pos);

        for (int i = 0; i < Shooting.maxReflections; i++) {
            // Find the first opaque object hit by the laser.
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, directLaser, Shooting.maxRange);
            RaycastHit2D hit = Physics2D.Raycast(pos, directLaser, Shooting.maxRange);
            foreach (var obj in hits) {
                if (obj.collider.tag == TagConstant.ReflectWall || obj.collider.tag == TagConstant.Wall) {
                    hit = obj;
                    break;
                } else if (obj.collider.tag == TagConstant.WaterDrop) {
                    hit = obj;
                    break;
                }
            }
            
            if (hit.collider == null) {
                Debug.Log("hit.collider is null");
                /// Laser shoots off into space.
                positions.Add(pos + (directLaser.normalized * Shooting.maxRange));
                break;
            }
            Particle particle = hit.collider.gameObject.GetComponent<Particle>();
            if (particle == null) { 
                /// Laser shoots off into space.
                positions.Add(pos + (directLaser.normalized * Shooting.maxRange));
                break;
            }

            Debug.Log($"hit.point = {hit.point}");
            BlockType blockType = particle.getBlockType();
            if (blockType == BlockType.Water) {
                // Logs water position on death.
                string url = $"https://docs.google.com/forms/d/e/1FAIpQLSd02iSGLy70_8jzmnZtIZbMc4KJNCfetrs7eo3PnL4dFIE2Ww/formResponse?usp=pp_url&entry.1386653628={particle.tile.location.x}&entry.962467366={particle.tile.location.y}&entry.1845636193={particle.tile.location.z}&submit=Submit";
                StartCoroutine(GetRequest(url));
                Destroy(hit.collider.gameObject);
                break;
            } else if (blockType == BlockType.Bedrock || blockType == BlockType.Dirt) {
                positions.Add(hit.point);
                break;
            } else if (blockType == BlockType.Mirror) {
                positions.Add(hit.point);
                    
                directLaser = Vector3.Reflect(directLaser, hit.normal);
                    
                pos = hit.point + (hit.normal.normalized * 0.01f);
                continue;
            } else {
                Debug.Log("ERROR: Unknown block type");
                break;
            }
        }

        // Assign positions to line renderer.
        laserRenderer.positionCount = positions.Count;
        laserRenderer.SetPositions(positions.ToArray());
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
