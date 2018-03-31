using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pos : MonoBehaviour {
    public Transform Earth;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(7, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Earth.position;
	}
}
