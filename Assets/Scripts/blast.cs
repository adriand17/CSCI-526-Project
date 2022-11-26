using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast : MonoBehaviour
{
    private static blast instance;

    [SerializeField] public GridManager gridManager;
    [SerializeField] private Camera mainCamera;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

   IEnumerator DisableBlast()
   {
    yield return new WaitForSeconds(1f);
    gameObject.SetActive(false);
   }

    public static void ShowBlast_static(Vector2 location)
    {
        // Debug.Log(location);
        // Debug.Log(instance.gridManager.getHeight());
        // Debug.Log(instance.gridManager.getWidth());
        float offX = location.x - instance.gridManager.getWidth()/2f;
        float offY = location.y - instance.gridManager.getHeight()/2f;
        // Debug.Log((offX, offY));
        instance.gameObject.transform.localPosition = new Vector3(offX*60f, offY*50f, 0);
        // instance.gameObject.transform.anchoredPosition = new Vector3(location.x, location.y, 0);
        instance.gameObject.SetActive(true);
        instance.StartCoroutine(instance.DisableBlast());
    }
}