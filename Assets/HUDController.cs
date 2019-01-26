using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
  public Transform target;
  private Renderer r;
  private Renderer targetR;

  void Start()
  {
    r = GetComponent<Renderer>();
    targetR = target.GetComponent<Renderer>();

    r.enabled = false;
    float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
    float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;

    transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
    transform.localScale = new Vector3(0.4f, 0.4f, 1f);
  }

  void Update()
  {
    float distance = Vector2.Distance(target.position, transform.position);

    r.enabled = !targetR.isVisible;

    Vector3 diff = target.position - transform.position;
    diff.Normalize();

    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * 1000f);
  }
}
