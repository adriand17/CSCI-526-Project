using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _camera;

    void Awake() {
        Instance = this;
    }

    public void handleGunPosition() {
        float height = 2f * Camera.main.orthographicSize;
        Vector3 pos = _camera.transform.position + new Vector3(0.0f, -height / 2f, 0f);
        pos.z = -5f;

        _gunObject.transform.position = pos;
    }

}