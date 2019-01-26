using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lockTo;
    public Camera c;
    private float depth;
    void Start()
    {
        this.c = this.GetComponent<Camera>();
        this.depth = c.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Tab)) {
            this.c.orthographicSize = this.depth * 2;
        } else if(Input.GetKeyUp(KeyCode.Tab)) {
            this.c.orthographicSize = Mathf.Lerp(this.c.orthographicSize, this.depth*5, Time.deltaTime / 100);
        }*/
        this.transform.position = new Vector3(lockTo.transform.position.x, lockTo.transform.position.y, this.transform.position.z);
    }
}
