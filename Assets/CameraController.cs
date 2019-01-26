using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform target;
  private Vector3 velocity = Vector3.zero;
  public float smoothTime = 0.05F;

  void Update()
  {
    if (target != null)
    {
      Vector3 v = new Vector3(target.position.x, target.position.y, this.transform.position.z);
      transform.position = Vector3.SmoothDamp(transform.position, v, ref velocity, smoothTime);
    }
  }
}
