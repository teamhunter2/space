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

  void Update()
  {
    Vector3 diff = target.position - transform.position;
    diff.Normalize();

    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * 100f);
    if(this.rb.rotation > 0.5f) {
      controller.AddIntent(ShipMovement.RotateLeft);
      controller.RemoveIntent(ShipMovement.RotateRight); 
    } else if (this.rb.rotation < 0.5f) {
      controller.AddIntent(ShipMovement.RotateRight);
      controller.RemoveIntent(ShipMovement.RotateLeft); 
    }

    if(Time.frameCount % 256 == 0) {
      if(Vector2.Distance(transform.position, target.position) < 10f) {
        controller.ShootMissile();
      }
    }
    if (Time.frameCount % 15 == 0)
    {

      
      if (Vector2.Distance(transform.position, target.position) > 2f)
      {
        controller.MoveForwards();
      }

      if (Vector2.Distance(transform.position, target.position) < 2f)
      {
        controller.MoveBackwards();
      }
    }
  }
}
