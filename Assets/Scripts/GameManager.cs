using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
  public static GameManager instance = null;
  private Transform _player;
  public List<GameObject> enemies;

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

  void Update()
  {
    if (Input.GetKey(KeyCode.Return))
    {
      SpawnRandomEnemy();
    }
  }

  public static Vector3 RandomPointOnUnitCircle(float radius)
  {
    float angle = Random.Range(0f, Mathf.PI * 2);
    float x = Mathf.Sin(angle) * radius;
    float y = Mathf.Cos(angle) * radius;

    return new Vector3(x, y, 0f) + instance.player.transform.position;

  }

  public void SpawnRandomEnemy()
  {
    GameObject enemeny = enemies[(int)Random.Range(0, enemies.Count)];
    GameObject.Instantiate(enemeny, RandomPointOnUnitCircle(100f), enemeny.transform.rotation);
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
