using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterThrust : ThrustGenerator
{
    public KeyCode BoostButton = KeyCode.Space;
    public float normalThrust = 1f;
    public float boostedThrust = 1f;

    private bool depleted = false;
    private bool firingBooster = false;
    public float boostTime = 5f;
    private float maxBoostTime;

    public float boostRechargeRate = 0.5f;
    public float boostReactivationDelay = 0.5f;
    public void Start() {
        this.thrust = this.normalThrust;
        this.maxBoostTime = this.boostTime;
    }

    public void Update() {
        if(firingBooster && !depleted) {
            boostTime -= Time.deltaTime;
            if(boostTime < 0) {
                depleted = true;
                firingBooster = false;
                this.FireThrusters(false);
            } 
        } else if (boostTime < maxBoostTime) {
            boostTime += Time.deltaTime * boostRechargeRate;
            if(depleted && boostTime > maxBoostTime * boostReactivationDelay) {
                depleted = false;
            }
        }
        if(Input.GetKeyUp(BoostButton) || Input.GetKeyDown(BoostButton)) {
            this.firingBooster = Input.GetKey(BoostButton);
            this.FireThrusters(Input.GetKey(BoostButton));
            this.SendMessageUpwards("updateThrust", null, SendMessageOptions.DontRequireReceiver);
        }
    }
    public void FireThrusters(bool fireing) {
        bool isFiring = (bool)fireing;
        switch(isFiring && !depleted) {
            case true:
                this.thrust = boostedThrust;
                break;
            case false:
                this.thrust = normalThrust;
                break;
        }
    }
}
