using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public enum ShipMovement
{
    Forward,
    Backward,
    Left,
    Right,
    RotateLeft,
    RotateRight
}
public class SpaceShipMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public KeyCode Forward = KeyCode.W;
    public KeyCode Backward = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode RotateLeft = KeyCode.Q;
    public KeyCode RotateRight = KeyCode.E;

    private Rigidbody2D rb;
    public float rotAc = 2f;


    private float orignalMass;
    public float thrust;

    public List<Animation> thrusters;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.orignalMass = this.GetComponent<Rigidbody2D>().mass;
        this.updateMass();
        this.updateThrust();
        this.AdjustThrusterAnimations();

        this.GetComponentsInChildren<Animation>(false, thrusters);
  }

    // Update is called once per frame
    void Update()
    {
        AdjustThrusterAnimations();
        ApplyForces();
        if(Input.GetKey(RotateLeft))
            this.transform.Rotate(new Vector3(0, 0, rotAc), Space.Self);
        else if(Input.GetKey(RotateRight))
            this.transform.Rotate(new Vector3(0, 0, rotAc * -1), Space.Self);
        
    }

    private void updateThrust() {
        List<ThrustGenerator> thrusters = new List<ThrustGenerator>();
        this.GetComponentsInChildren<ThrustGenerator>(false, thrusters);
        this.thrust = 1;
    }

    private void updateMass() {
        List<Rigidbody2D> children = new List<Rigidbody2D>();
        this.GetComponentsInChildren<Rigidbody2D>(true, children);
        float weight = 0;
        for(int i = 0; i < children.Count; i++) {
            weight += children[i].mass;
        }
        this.GetComponent<Rigidbody2D>().mass = weight;
    }

    private void ApplyForces() {
        if(Input.GetKey(Forward)) {
            this.rb.AddRelativeForce(new Vector2(0, thrust), ForceMode2D.Impulse);
        }
        if(Input.GetKey(Backward)) {
            this.rb.AddRelativeForce(new Vector2(0, (thrust/2)*-1), ForceMode2D.Impulse);
        }
        if(Input.GetKey(Right)) {
            this.rb.AddRelativeForce(new Vector2(thrust/2, 0), ForceMode2D.Impulse);
        }
        if(Input.GetKey(Left)) {
            this.rb.AddRelativeForce(new Vector2((thrust/2)*-1, 0), ForceMode2D.Impulse);
        }
    }
    private void AdjustThrusterAnimations() {
        if(Input.GetKeyDown(Forward) || Input.GetKeyUp(Forward)) {
            SendToThrusters(ShipMovement.Forward, Input.GetKey(Forward));
        }
        if(Input.GetKeyDown(Backward) || Input.GetKeyUp(Backward)) {
            SendToThrusters(ShipMovement.Backward, Input.GetKey(Backward));
        }
        if(Input.GetKeyDown(Left) || Input.GetKeyUp(Left)) {
            SendToThrusters(ShipMovement.Left, Input.GetKey(Left));
        }
        if(Input.GetKeyDown(Right) || Input.GetKeyUp(Right)) {
            SendToThrusters(ShipMovement.Right, Input.GetKey(Right));
        }
        if(Input.GetKeyDown(RotateLeft) || Input.GetKeyUp(RotateLeft)) {
            SendToThrusters(ShipMovement.RotateLeft, Input.GetKey(RotateLeft));
        }
        if(Input.GetKeyDown(RotateRight) || Input.GetKeyUp(RotateRight)) {
            SendToThrusters(ShipMovement.RotateRight, Input.GetKey(RotateRight));
        }
    }

    private void SendToThrusters(ShipMovement s, bool status) {
        foreach(Animation a in thrusters) {
            a.UpdateThrusterStatus(s, status);
        }
    }
}
