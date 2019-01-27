using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachSalvage : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> canAttach = new List<GameObject>();
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z) && canAttach.Count > 0) {
            GameObject attachable = canAttach[0];
            attachable.tag = "Player";
            attachable.transform.parent = this.gameObject.transform;
            var ssm =attachable.GetComponent<SpaceShipMovement>();
            if(ssm)
                ssm.SetSkipPlayerInput();
            SetLayer(attachable.transform, this.gameObject.layer);
            foreach(Transform t in attachable.transform) {
                SetLayer(t, this.gameObject.layer);
            }
            
            attachable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            attachable.AddComponent(this.GetType());
            canAttach.Remove(canAttach[0]);

            this.SendMessageUpwards("updateMass");
            this.SendMessageUpwards("updateThrust");
            this.SendMessageUpwards("updateZoom");
            this.SendMessageUpwards("updateLaunchers");

            List<Animation>thrusters = new List<Animation>();
            this.GetComponentsInChildren<Animation>(true, thrusters);

            foreach(Animation th in thrusters) {
                this.SendMessageUpwards("AddToThrusters", th, SendMessageOptions.RequireReceiver);
            }
            
        }
    }

    void onCollisionEnter(Collision e) {
        if(e.gameObject.tag == "Player") {
            Physics.IgnoreCollision(e.collider, this.GetComponent<Collider>(), true);
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Attachable") {
            canAttach.Add(c.gameObject);
            Debug.Log("In range of " + c.ToString());
        }
    }

    void SetLayer(Transform t, int layer) {
        t.gameObject.layer = layer;
    }
    void OnTriggerExit2D(Collider2D c) {
        if(c.tag == "Attachable") {
            canAttach.Remove(c.gameObject);
            Debug.Log("Out of range of " + c.ToString());
        }
            
    }

    
}
