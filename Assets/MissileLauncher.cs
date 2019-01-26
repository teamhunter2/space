using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
  public GameObject missile;
  public int ammo = 10;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (ammo > 0)
      {
        GameObject current = GameObject.Instantiate(missile);
      }
    }
  }
}
