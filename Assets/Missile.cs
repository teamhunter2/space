using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
  Rigidbody2D rb;
  public float force = 7f;
  private float inertiaBulder = 0.9f;
  public float fuel = 1f;
  public GameObject explosion;

  public GameObject target;
  public GameObject fire;
  private CapsuleCollider2D boxCollider;
  private Vector2 boxColliderOrg;
  private bool isInvisible = false;
  private Transform player;
  // Start is called before the first frame update
  void Start()
  {
    rb = this.GetComponent<Rigidbody2D>();
    boxCollider = this.GetComponent<CapsuleCollider2D>();
    boxCollider.enabled = false;

    player = GameObject.FindGameObjectWithTag("Player").transform;
  }

  // Update is called once per frame
  void Update()
  {
    if (this.fuel > 0)
    {
      if(this.target) {
        Debug.DrawLine(this.transform.position, this.target.transform.position + (Vector3)this.target.GetComponent<Rigidbody2D>().velocity);
        Vector3 diff = (target.transform.position + (Vector3)this.target.GetComponent<Rigidbody2D>().velocity) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * 200f);
      }
      this.fuel -= Time.deltaTime;
      this.rb.AddRelativeForce(new Vector2(0, (1f * force * (1f - inertiaBulder))));
      this.inertiaBulder *= 0.5f;
      if (this.fuel < 0)
      {
        //this.rb.AddTorque(0.25f);
        this.GetComponentInChildren<TrailRenderer>().time = 0.2f;
        this.enabled = false;
        if (Vector2.Distance(this.transform.position, player.transform.position) > 100f)
        {
          Debug.Log("Missile Destroyed");
          Destroy(this.gameObject, 1f);
        }
      }
    }
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    this.enabled = false;
    this.rb.velocity = new Vector2(0, 0);
    this.rb.rotation = 0;
    if (explosion != null)
    {
      GameObject.Instantiate(explosion, transform);
    }
    AudioSource[] srces = GetComponents<AudioSource>();
    for (int i = 0; i < srces.Length; i++)
    {
      srces[i].Stop();
    }
    GetComponent<SpriteRenderer>().material.color = new Color(0f, 0f, 0f, 0f);

    /* Destroy(col.otherCollider.gameObject); */
    Destroy(this, 10f);
  }

  void OnBecameInvisible()
  {
    this.isInvisible = true;
  }

  void OnTriggerExit2D(Collider2D col)
  {
    if (col.gameObject.tag == "Player")
    {
      this.boxCollider.enabled = true;
    }
  }
}
