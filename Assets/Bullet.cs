using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 0.5f;
    void Start()
    {
        this.GetComponent<Rigidbody2D>()
            .AddRelativeForce(new Vector2(0, Speed), ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
