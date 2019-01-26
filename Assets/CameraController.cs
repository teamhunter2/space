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
      transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
  }
}
