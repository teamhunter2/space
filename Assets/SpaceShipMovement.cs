using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbody;
    public float boostRemaining = 20;
    private float maxBoost;
    public float rotZ = 2f;
    public float rotAc = 2f;


    private float orignalMass;
    public float thrust;
    void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.maxBoost = this.boostRemaining;
        this.orignalMass = this.rigidbody.mass;
        this.updateMass();
        this.updateThrust();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) {
            this.SendMessageUpwards("FireBoosters", Input.GetKey(KeyCode.Space), SendMessageOptions.DontRequireReceiver);
            this.updateThrust();
        }   
        if(Input.GetKey(KeyCode.LeftArrow))
            this.transform.Rotate(new Vector3(0, 0, rotAc), Space.Self);
        else if(Input.GetKey(KeyCode.RightArrow))
            this.transform.Rotate(new Vector3(0, 0, rotAc * -1), Space.Self);

        if(Input.GetKey(KeyCode.UpArrow)) {
            this.rigidbody.AddRelativeForce(new Vector2(0, thrust * Time.deltaTime), ForceMode2D.Impulse);
        }
        
    }

    private void updateThrust() {
        this.thrust = this.GetComponent<ThrustGenerator>().calculateThrust();
    }
    private void updateMass() {
        List<Rigidbody2D> children = new List<Rigidbody2D>();
        this.GetComponentsInChildren<Rigidbody2D>(true, children);
        float weight = 0;
        for(int i = 0; i < children.Count; i++) {
            weight += children[i].mass;
        }
        this.rigidbody.mass = weight;
    }
}
