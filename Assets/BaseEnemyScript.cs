using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D enemy) {
        if(enemy.gameObject.transform.parent != null)
            return;
        if(enemy.gameObject.tag != this.tag && enemy.gameObject.tag != "Projectile") {
        Debug.Log("Shooting at enemy");
        foreach(MissileLauncher m in this.GetComponentsInChildren<MissileLauncher>()) {
            if(m.Shoot(this.GetComponent<Rigidbody2D>(), enemy.gameObject))
                break;
        }
        }
    }
}
