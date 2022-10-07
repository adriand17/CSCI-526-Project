using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnergy
{
    float _currentEnergy;
    float _currentMaxEnergy;
    float _energyRegenSpeed;
    bool _pauseEnergyRegen = false;

    // Properties
    public float Energy{
        get{
            return _currentEnergy;
        }
        set{
            _currentEnergy = value;
        }
    }

    public float MaxEnergy{
        get{
            return _currentMaxEnergy;
        }
        set{
            _currentMaxEnergy = value;
        }
    }

    public float EnergyRegenSpeed{
        get{
            return _energyRegenSpeed;
        }
        set{
            _energyRegenSpeed = value;
        }
    }

    public bool PauseEnergyRegen{
        get{
            return _pauseEnergyRegen;
        }
        set{
            _pauseEnergyRegen = value;
        }
    }



    // Constructor
    public UnitEnergy(float energy, float maxEnergy, float energyRegenSpped, bool pauseEnergyRegen){
        _currentEnergy = energy;
        _currentMaxEnergy = maxEnergy;
        _energyRegenSpeed = energyRegenSpped;
        _pauseEnergyRegen = PauseEnergyRegen;
    }

    // Methods
    public void UseEnergy(float energyAmount){
        if (_currentEnergy > 0){
            _currentEnergy -= energyAmount * Time.deltaTime;
        }
    }

    public void RegenEnergy(){
        if (_currentEnergy < _currentMaxEnergy && !_pauseEnergyRegen){
            _currentEnergy += _energyRegenSpeed * Time.deltaTime;
        }
    }
}
