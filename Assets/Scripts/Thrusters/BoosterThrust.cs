using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterThrust : ThrustGenerator
{
    public KeyCode BoostButton = KeyCode.Space;
    public float normalThrust = 1f;
    public float boostedThrust = 1f;

    public void Start() {
        this.thrust = this.normalThrust;
    }

    public void Update() {
        if(Input.GetKeyUp(BoostButton) || Input.GetKeyDown(BoostButton)) {
            this.FireThrusters(Input.GetKey(BoostButton));
            this.SendMessageUpwards("updateThrust", null, SendMessageOptions.DontRequireReceiver);
        }
    }
    public void FireThrusters(bool fireing) {
        bool isFiring = (bool)fireing;
        switch(isFiring) {
            case true:
                this.thrust = boostedThrust;
                break;
            case false:
                this.thrust = normalThrust;
                break;
        }
    }
}
