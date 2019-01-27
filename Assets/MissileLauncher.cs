using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
  public GameObject missile;
  public int ammo = 10;

  public float refireTime = 1f;
  private float currentRefireTime = 0f;

  public bool launchSeeking = false;
  public string seekingTag = "Enemy";

  void Update() {
    if(currentRefireTime > 0) {
      currentRefireTime -= Time.deltaTime;
    }
  }

  public bool Shoot(Rigidbody2D rb, GameObject target) {
    if(currentRefireTime > 0f || this.ammo == 0) {
      return false;
    }
    Debug.Log("Shooting" + this);
    Vector3 v = this.transform.position + ((Vector3)rb.velocity.normalized* 0.10f);
    this.ammo -= 1;
    var m = Instantiate(missile, v, this.transform.rotation);
    m.layer = this.gameObject.layer;
    m.GetComponent<Missile>().target = target;
    m.GetComponent<Rigidbody2D>().velocity = rb.velocity;
    this.currentRefireTime = refireTime;
    return true;
  }
  public bool Shoot(Rigidbody2D rb) {
    if(currentRefireTime > 0f || this.ammo == 0) {
      return false;
    }
    Debug.Log("Shooting" + this);
    Vector3 v = this.transform.position + ((Vector3)rb.velocity.normalized* 0.10f);
    this.ammo -= 1;
    var m = Instantiate(missile, v, this.transform.rotation);
    m.layer = this.gameObject.layer;
    if(this.launchSeeking)
      m.GetComponent<Missile>().target = GameObject.FindGameObjectWithTag("Enemy");
    m.GetComponent<Rigidbody2D>().velocity = rb.velocity;
    this.currentRefireTime = refireTime;
    return true;
  }
}
