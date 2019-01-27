using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
  Seeking,
  Chasing,
  Attacking,
  Avoiding,
  Retreating,
  Dead
}

public class EnemyAI_V2 : MonoBehaviour
{
  private GameObject target;
  private Rigidbody2D targetRb;
  private Rigidbody2D rb;
  private GameObject hud;
  public AIState state;

  private SpaceShipMovement controller;
  public float rotationSpeed = 64f;

  public float seekSpeed = 10f;
  public float seekMinDistance = 10f;

  public float chaseMinSpeed = 1f;

  public float minChaseDistance = 3f;
  public float maxChaseDistance = 10f;

  private int attackCount = 0;
  private Transform temporaryTarget;

  private GameObject hudInstance;
  void Start()
  {
    target = GameObject.FindGameObjectWithTag("Player");
    controller = GetComponent<SpaceShipMovement>();
    targetRb = target.GetComponent<Rigidbody2D>();
    rb = GetComponent<Rigidbody2D>();
    SpawnHUD();
    state = AIState.Seeking;
  }

  void SpawnHUD()
  {
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

  void RotateTowardsTarget(Transform targetTransform = null)
  {
    targetTransform = targetTransform == null ? target.transform : targetTransform;
    Vector3 diff = targetTransform.position - transform.position;
    diff.Normalize();
    float z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, z - 90), Time.deltaTime * rotationSpeed);
  }

  void Update()
  {
    if (state != AIState.Dead)
    {
      switch (state)
      {
        case AIState.Seeking:
          Seek();
          break;
        case AIState.Chasing:
          Chase();
          break;
        case AIState.Attacking:
          Attack();
          break;
        case AIState.Avoiding:
          break;
        case AIState.Retreating:
          Retreat();
          break;
        default:
          state = AIState.Seeking;
          break;
      }
    }
  }

  float GetDistanceToTarget(Transform targetTransform = null)
  {
    targetTransform = targetTransform == null ? target.transform : targetTransform;
    return Mathf.Abs(Vector2.Distance(transform.position, targetTransform.position));
  }

  void Seek()
  {
    RotateTowardsTarget();
    float distance = GetDistanceToTarget();

    if (distance < seekMinDistance)
    {
      this.state = AIState.Chasing;
      return;
    }

    this.transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * seekSpeed);

    /* Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * seekDistance, Color.green);
    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), seekDistance); */
  }

  public Vector3 RandomPointOnUnitCircle(float radius)
  {
    float angle = Random.Range(0f, Mathf.PI * 2);
    float x = Mathf.Sin(angle) * radius;
    float y = Mathf.Cos(angle) * radius;

    return new Vector3(x, y, 0f) + transform.position;

  }

  void Chase()
  {
    RotateTowardsTarget();
    float distance = GetDistanceToTarget();

    if (distance > maxChaseDistance)
    {
      Debug.Log("Distance to great, Seeking");
      this.state = AIState.Seeking;
      return;
    }

    if (distance < minChaseDistance)
    {
      Debug.Log("Distance OK, Attacking");
      this.state = AIState.Attacking;
      return;
    }

    Debug.Log("Chasing");

    /* var localVelocity = transform.InverseTransformDirection(targetRb.velocity);
    var velocity = localVelocity.y;
    velocity = velocity > 0 ? velocity : chaseMinSpeed; */

    transform.position = Vector2.MoveTowards(transform.position, (Vector2)target.transform.position + targetRb.velocity, Time.deltaTime);
  }

  void Attack()
  {
    RotateTowardsTarget();
    float distance = GetDistanceToTarget();
    Debug.Log("Attacking");

    if (distance > minChaseDistance)
    {
      state = AIState.Seeking;
    }

    Vector2 targetPos = minChaseDistance * Vector3.Normalize(transform.position - target.transform.position) + target.transform.position;

    transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 0.1f);

    if (controller != null)
    {
      if (Random.Range(0, 100) > 75)
      {
        controller.ShootMissile();
        attackCount++;
      }
    }

    if (Random.Range(0, 100) > 75)
    {
      if (attackCount > 5)
      {
        attackCount = 0;
        state = AIState.Retreating;
      }
    }
  }

  void Avoid()
  {

  }

  void Retreat()
  {
    Debug.Log("Retreating");
    if (temporaryTarget == null)
    {
      Vector2 position = RandomPointOnUnitCircle(maxChaseDistance);
      GameObject temp = new GameObject();
      temp.transform.position = position;
      temporaryTarget = temp.transform;
    }

    float distance = GetDistanceToTarget(temporaryTarget);
    RotateTowardsTarget(temporaryTarget);
    transform.position = Vector2.MoveTowards(transform.position, (Vector2)temporaryTarget.transform.position, Time.deltaTime * 3f);

    if (distance < 1f)
    {
      state = AIState.Seeking;
      temporaryTarget = null;
      return;
    }
  }
}
