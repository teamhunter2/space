using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float forwardMomentum = 1f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, forwardMomentum), ForceMode2D.Impulse);
        this.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(0, 2), new Vector2(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        Destroy(this.gameObject, 0.001f);
        GameObject newAst1 = Instantiate(
            this.gameObject,
            col.collider.transform,
            true);
        newAst1.transform.localScale = newAst1.transform.localScale * 0.5f;
        GameObject newAst2 = Instantiate(
            this.gameObject,
            col.collider.transform,
            true);
        newAst2.transform.localScale = newAst2.transform.localScale * 0.5f;

    }
}
