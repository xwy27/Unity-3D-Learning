using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public Transform source;
    public float step;
    private float r_x;
    private float r_y;

	// Use this for initialization
	void Start () {
        r_x = Random.Range(0, 0.3f);
        r_y = Random.Range(0, 0.3f);
        while (r_y == 0) {
            r_y = Random.Range(-1, 1);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 axis;
        if (source.ToString() == "EarthShadow") {
            axis = new Vector3(0, 1, 0);
            Debug.Log("moon");
        } else {
            axis = new Vector3(r_x, r_y, 0);
        }
        transform.RotateAround(source.position, axis, step * Time.deltaTime);
    }
}
