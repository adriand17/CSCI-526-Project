using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private GameObject _gunObject;
    [SerializeField] private GameObject _gunPrefab;
    public void buildGun() {
        _gunObject = Instantiate(_gunPrefab, new Vector3(0, 0), Quaternion.identity);
    }

    public void handleGunPosition() {
        float w = Screen.width;
        float objcheight = _gunObject.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(w / 2f, 0.0f, 0f));
        pos.z = -5f;
        pos.y -= objcheight / 5;
        _gunObject.transform.position = pos;
    }
}