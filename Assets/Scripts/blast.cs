using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast : MonoBehaviour
{
    private static blast instance;

    [SerializeField] public Grid grid;
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
        instance.gameObject.SetActive(true);
        instance.StartCoroutine(instance.DisableBlast());
    }
}
