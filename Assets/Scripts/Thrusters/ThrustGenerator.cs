using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustGenerator : MonoBehaviour
{
  public float thrust = 0f;


  public float ThrustGenerated {
    get {
      return this.thrust;
    }
  }
  public float calculateThrust() {
    List<ThrustGenerator> thrusters = new List<ThrustGenerator>();
    this.GetComponentsInChildren(false, thrusters);
    
    float _thrust = 0;
    for(int i = 0; i < thrusters.Count; i++) {
      _thrust += thrusters[i].ThrustGenerated;
    }
    return _thrust;
  }

}