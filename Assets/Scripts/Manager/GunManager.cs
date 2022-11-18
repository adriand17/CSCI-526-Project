using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private GameObject _gunObject;
    // private EnergyBar _gunBarPrefab;
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private EnergyBar _gunBarPrefab;
    [SerializeField] public float gunXPositionFromCenter = 0f;
    [SerializeField] public float gunYOffset= 0f;

    private Shooting shootingScript;
    private LaserStatus laserStatus;

    public void buildGun() {
        _gunObject = Instantiate(_gunPrefab, new Vector3(0, 0), Quaternion.identity);
        shootingScript = _gunObject.GetComponent<Shooting>();
        //LaserStatus laserStatus = new LaserStatus();
        //shootingScript.setLaserStatus(laserStatus);

        //buildGunBar();
    }

    public void handleGunPosition() {
        float w = Screen.width;
        float objcheight = _gunObject.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(w / 2f, 0.0f, 0f));
        pos.z = -5f;
        pos.y -= objcheight / 5;
        pos.y += gunYOffset;
        pos.x += gunXPositionFromCenter;

        _gunObject.transform.position = pos;
        //handleGunBarPosition();
    }

    // public void buildGunBar() {
    //     _gunBarObject = Instantiate(_gunBarPrefab, new Vector3(0, 0), Quaternion.identity);
    // }

    // public void handleGunBarPosition() {
    //     float w = Screen.width;
    //     float objcheight = _gunBarObject.GetComponent<SpriteRenderer>().bounds.size.y;
    //     Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0.0f, 0f));
    //     pos.z = -5f;
    //     //pos.y -= objcheight / 5;
    //     _gunBarObject.transform.position = pos;
    // }

    void Update() {
        int level = shootingScript.getLaserStatus().getCurrentReflectLevel();
        _gunBarPrefab.SetEnergy(level);
    }

}