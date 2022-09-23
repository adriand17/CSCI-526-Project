using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _camera;

    public void handleGunPosition() {
        float height = 2f * Camera.main.orthographicSize;
        Vector3 pos = _camera.transform.position + new Vector3(0.0f, -height / 2f, 0f);
        pos.z = -5f;

        _gunObject.transform.position = pos;
    }
}