using UnityEngine;

public class LaserStatus {
    private const int MAX_LASER_LEVEL = 8;
    
    private const float laserFireRate = 1f;

    private int currentLaserLevel = 8;
    //Store laser engergy with deltaTime
    private float laserEnergy = 8.0f;
    public bool isFiring;

    // Furthest distance the laser can reach.
    private const int range = 100;

    public int maxRange() {
        return range;
    }

    public bool canFire() {
        return currentLaserLevel != 0;
    }

    public void decreaseLaserLevel() {
        if (laserEnergy > 0) {
            laserEnergy -= getFireRate();
        }        
    }

    public void increaseLaserLevel() {
        if (laserEnergy < MAX_LASER_LEVEL) {
            laserEnergy += getFireRate();
        }        
    }

    private float getFireRate() {
        return laserFireRate * Time.deltaTime;
    }

    public int getCurrentReflectLevel() {
        return currentLaserLevel;
    }

    public int getMaxReflectNumber() {
        return MAX_LASER_LEVEL;
    }

    public void updateLaserEnergyLevel() {
        if (isFiring) {
            //decreaseLaserLevel();
        } else {
            //increaseLaserLevel();
        }
        currentLaserLevel = (int)laserEnergy;

    }
}