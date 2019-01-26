using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public float thrust = 1f;
  public float rotationSpeed = 100f;

  void Update()
  {
    Transform player = GameManager.instance.player;
    Vector3 target = player.position - transform.position;
    float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

    transform.position += (player.position - transform.position).normalized * (Time.deltaTime * thrust);
  }
}
