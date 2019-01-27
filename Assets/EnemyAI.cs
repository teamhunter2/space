using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public SpaceShipMovement controller;
  public SpaceShipMovement targetController;
  public Rigidbody2D rb;
  public Rigidbody2D targetRb;
  private Transform target;
  public GameObject hud;

  private GameObject hudInstance;
  private bool isDead = false;
  void Start()
  {
    target = GameManager.instance.player;
    targetController = target.GetComponent<SpaceShipMovement>();



    rb = GetComponent<Rigidbody2D>();
    targetRb = target.GetComponent<Rigidbody2D>();
    if (hud != null)
    {
      hudInstance = GameObject.Instantiate(hud, Camera.main.transform);
      hudInstance.transform.parent = Camera.main.transform;
      hudInstance.GetComponent<HUDController>().target = this.transform;
    }
  }

  void OnDestroyed()
  {
    if (hudInstance != null)
    {
      Destroy(hudInstance);
    }
  }

  private int getMovementRatio(float distance)
  {
    distance = distance > 10 ? 10 : distance;
    distance = distance < 0 ? 0 : distance;
    return (int)(distance - 0) * (1 - 32) / (1000 - 0) + 32;
  }

  void Update()
  {
    if(isDead)
      return;
    Vector3 diff = target.position - transform.position;
    diff.Normalize();

    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * 1f);
    if (this.rb.rotation > 0.5f)
    {
      controller.AddIntent(ShipMovement.RotateLeft);
      controller.RemoveIntent(ShipMovement.RotateRight);
    }
    else if (this.rb.rotation < 0.5f)
    {
      controller.AddIntent(ShipMovement.RotateRight);
      controller.RemoveIntent(ShipMovement.RotateLeft);
    }

    float distance = Vector2.Distance(transform.position, target.position);


    if (Time.frameCount % 32 == 0)
    {
      if (distance < 10f)
      {
        if (Random.Range(0, 100) > 75)
        {
          controller.ShootMissile();
        }
      }
    }

    if (Time.frameCount % getMovementRatio(distance) == 0)
    {
      if (distance > 5f)
      {
        controller.MoveForwards();
      }

      if (Vector2.Distance(transform.position, target.position) < 5f)
      {
        controller.MoveBackwards();
      }
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if(col.tag == "Projectile") {
      this.gameObject.tag = "Attachable";
      Destroy(this);
      Destroy(this.GetComponent<HUDController>());
      foreach(ShipMovement s in (ShipMovement[])System.Enum.GetValues(typeof(ShipMovement)))
      {
        controller.RemoveIntent(s);
      }
      
    }
  }
}
