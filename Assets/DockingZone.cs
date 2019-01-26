using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingZone : MonoBehaviour
{
    // Start is called before the first frame update
    bool playerDocked = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D colider) {
      if(colider.tag == "Player" && !playerDocked) {
        Rigidbody2D rb = colider.gameObject.GetComponent<Rigidbody2D>();
        if(rb.angularVelocity < 0.2f && rb.velocity.x < 0.2f && rb.velocity.y < 0.2f)
        {
          Debug.Log("Has docked");
          rb.velocity = new Vector2(0, 0);
          playerDocked = true;
        }
      }
    }

    void OnTriggerExit2D(Collider2D colider) {
      if(colider.tag == "Player") {
        Debug.Log("Undocked");
        playerDocked = false;
      }
    }
}
