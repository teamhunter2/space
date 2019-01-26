using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
  public static GameManager instance = null;
  private Transform _player;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);

    InitGame();
  }

  void InitGame()
  {
    // Do initialization stuff here
  }

  public Transform player
  {
    get
    {
      if (this._player == null)
      {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
          this._player = player.GetComponent<Transform>();
          Debug.Log("Got player");
        }
        else
        {
          Debug.Log("Player not found!");
        }
      }

      return this._player;
    }
  }
}
