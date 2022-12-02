using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast1 : MonoBehaviour
{
    //private static Blast instance;

    [SerializeField] public GridManager _gridManager;
    [SerializeField] public SpriteRenderer _renderer;
    //[SerializeField] private Camera mainCamera;
    
    // Start is called before the first frame update

    private void Awake()
    {
        //instance = this;
        //gameObject.SetActive(false);
    }

    public void Init(GridManager gridManager)
    {
        /// Calculate a random delay.
        //delay = Random.Range(0, TickInterval);
        Debug.Log("init explosion");
        this._gridManager = gridManager;
        /// Prevents particle hiding behind tile.
        _renderer.sortingLayerName = "ParticleLayer";
        Vector3 objectScale = transform.localScale;
        gameObject.transform.localScale = new Vector3((float)(objectScale.x * 2.5), (float)(objectScale.y * 2.5), 0.0f);
        StartCoroutine(DisableBlast());
    }

    IEnumerator DisableBlast()
   {
    yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
   }

    public static void ShowBlast_static(Vector2 location)
    {

       // var particle = Instantiate(_particlePrefab, new Vector3(location.x, location.y), Quaternion.identity);
        //particle.Init(type, tile, this);


        Debug.Log(location);
        //Debug.Log("height: " + instance.gridManager.getHeight());
        //Debug.Log("height: " + instance.gridManager.getWidth());
        //float offX = location.x - instance._gridManager.getWidth()/2f;
        //float offY = location.y - instance._gridManager.getHeight()/2f;
        //Debug.Log((offX, offY));
        //instance.gameObject.transform.localPosition = new Vector3(offX*50f, offY*50f, 0);
        // instance.gameObject.transform.anchoredPosition = new Vector3(location.x, location.y, 0);
        //instance.gameObject.transform.localPosition = location;
        //Debug.Log("instance location" +  instance.gameObject.transform.localPosition);
        //instance.gameObject.SetActive(true);
        //instance.StartCoroutine(instance.DisableBlast());
    }
}