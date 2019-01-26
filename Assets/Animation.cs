using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    
    public Sprite[] sprites;
    public ShipMovement[] activateOn;
    private Dictionary<ShipMovement, bool> activated;


    public int currentSprite;

    public float transitionLength;
    private float timeSinceTransition = 0f;
    private SpriteRenderer s;
    private SpaceShipMovement ssm;
    public bool drawing = false;

    Vector3 superFlameScale;
    Vector3 normalFlameScale;

    void Start()
    {
        activated = new Dictionary<ShipMovement, bool>();
        foreach(ShipMovement direction in activateOn) {
            activated.Add(direction, false);
        }
        currentSprite = 0;
        s = this.GetComponent<SpriteRenderer>();
        ssm = this.GetComponentInParent<SpaceShipMovement>();

        superFlameScale = this.transform.localScale * 2;
        normalFlameScale = this.transform.localScale;
        s.sprite = this.sprites[0];
        this.enabled = true;
    }

    void adjustThrusterOn(ShipMovement sm, bool active) {

    }
    // Update is called once per frame
    void Update()
    {
        this.drawing = false;
        foreach(bool value in activated.Values) {
            if(value) {
                this.drawing = true;
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            this.enabled = true;
        } else if(Input.GetKeyUp(KeyCode.UpArrow))
            this.enabled = false;

        if(this.drawing) {
            this.timeSinceTransition += Time.deltaTime;
            if(timeSinceTransition >= transitionLength) {
                s.sprite = this.sprites[currentSprite % sprites.Length];
                this.currentSprite = (currentSprite + 1) % this.sprites.Length;
                timeSinceTransition = 0;
            }
        } else {
            s.sprite = null;
        }
    }

    public void UpdateThrusterStatus(ShipMovement movement, bool status) {
        foreach(ShipMovement s in activateOn) {
            if(s == movement) {
                this.activated.Remove(movement);
                this.activated.Add(movement, status);
            }
        }
    }
}
