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
  private AudioSource audioSource;

    private Rigidbody2D rb;
    public float rotAc = 2f;
    public GameObject missile;

    private float orignalMass;
    public float thrust;

    private float originalZoom;
    private float wantedZoom;

    public List<Animation> thrusters;
    public List<MissileLauncher> Launchers;
    private List<ShipMovement> intents = new List<ShipMovement>();
    private bool skipPlayerInput = false;
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.orignalMass = this.GetComponent<Rigidbody2D>().mass;
        this.updateMass();
        this.updateThrust();
        this.updateZoom();
        this.updateLaunchers();
        this.originalZoom = Camera.main.orthographicSize;
        if(this.gameObject.tag == "Player")
            this.AdjustIntents();
        else
            this.skipPlayerInput = true;

        this.GetComponentsInChildren<Animation>(false, thrusters);
  }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)(this.rb.velocity));
        if(!skipPlayerInput) {
            AdjustIntents();
            ApplyForces();
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, wantedZoom + this.originalZoom, Time.deltaTime);
            if(Input.GetKeyDown(KeyCode.Space))
                ShootMissile();

            if(Input.GetKey(RotateLeft))
                //this.transform.Rotate(new Vector3(0, 0, rotAc), Space.Self);
                this.rb.AddTorque(thrust * 4);
            else if(Input.GetKey(RotateRight))
                //this.transform.Rotate(new Vector3(0, 0, rotAc * -1), Space.Self);
                this.rb.AddTorque(-thrust * 4);
        }
    
        if (this.intents.Count > 0)
        {
            if (this.audioSource && !this.audioSource.isPlaying)
            {
                this.audioSource.Play();
            }
        }
    }

    private void updateThrust() {
        List<ThrustGenerator> thrustersW = new List<ThrustGenerator>();
        this.GetComponentsInChildren<ThrustGenerator>(true, thrustersW);
        this.thrust = 0;
        foreach(ThrustGenerator th in thrustersW) {
            this.thrust += th.ThrustGenerated;
        }
    }

    private void updateMass() {
        List<Rigidbody2D> children = new List<Rigidbody2D>();
        this.GetComponentsInChildren<Rigidbody2D>(true, children);
        float weight = 0;
        for(int i = 0; i < children.Count; i++) {
            weight += children[i].mass;
            children[i].mass = 0;
        }
        this.GetComponent<Rigidbody2D>().mass = weight;
    }

private void updateZoom() {
        this.wantedZoom = this.GetComponent<Zoomable>().GetZoomIncrease();
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
    private void AdjustIntents() {
        if(Input.GetKeyDown(Forward) || Input.GetKeyUp(Forward)) {
            if(Input.GetKey(Forward)) {
                AddIntent(ShipMovement.Forward);
            } else {
                RemoveIntent(ShipMovement.Forward);
            }
        }
        if(Input.GetKeyDown(Backward) || Input.GetKeyUp(Backward)) {
            if(Input.GetKey(Backward)) {
                AddIntent(ShipMovement.Backward);
            } else {
                RemoveIntent(ShipMovement.Backward);
            }
        }
        if(Input.GetKeyDown(Left) || Input.GetKeyUp(Left)) {
            if(Input.GetKey(Left)) {
                AddIntent(ShipMovement.Left);
            } else {
                RemoveIntent(ShipMovement.Left);
            }
        }
        if(Input.GetKeyDown(Right) || Input.GetKeyUp(Right)) {
            if(Input.GetKey(Right)) {
                AddIntent(ShipMovement.Right);
            } else {
                RemoveIntent(ShipMovement.Right);
            }
        }
        if(Input.GetKeyDown(RotateLeft) || Input.GetKeyUp(RotateLeft)) {
            if(Input.GetKey(RotateLeft)) {
                AddIntent(ShipMovement.RotateLeft);
            } else {
                RemoveIntent(ShipMovement.RotateLeft);
            }
        }
        if(Input.GetKeyDown(RotateRight) || Input.GetKeyUp(RotateRight)) {
            if(Input.GetKey(RotateRight)) {
                AddIntent(ShipMovement.RotateRight);
            } else {
                RemoveIntent(ShipMovement.RotateRight);
            }
        }
    }
    private void updateLaunchers() {
        this.GetComponentsInChildren<MissileLauncher>(true, this.Launchers);
    }

    private void SendToThrusters(ShipMovement s, bool status) {
        if(thrusters != null) {
            foreach(Animation a in thrusters) {
                a.UpdateThrusterStatus(s, status);
            }
        }
    }
    private void AddToThrusters(object o) {
        Debug.Log(o);
        Animation _thruster = (Animation)o;
        thrusters.Add(_thruster);
    }

    public void ShootMissile() {
        foreach(MissileLauncher m in Launchers) {
            if(m.Shoot(this.rb)) {
                break;
            }
        }
    }

    public void AddIntent(ShipMovement s) {
        if(!intents.Contains(s))
        {
            intents.Add(s);
            SendToThrusters(s, true);
        }
    }
    public void RemoveIntent(ShipMovement s) {
        if(intents.Contains(s)) {
            intents.Remove(s);
            SendToThrusters(s, false);
        }
    }
    public void MoveForwards() {
        this.RemoveIntent(ShipMovement.Backward);
        this.AddIntent(ShipMovement.Forward);
        this.rb.AddRelativeForce(new Vector2(0, thrust), ForceMode2D.Impulse);
    }

    public void MoveBackwards() {
        this.RemoveIntent(ShipMovement.Forward);
        this.AddIntent(ShipMovement.Backward);
        this.rb.AddRelativeForce(new Vector2(0, -thrust), ForceMode2D.Impulse);
    }
    public void SetSkipPlayerInput() {
        this.skipPlayerInput = true;
    }
}
