using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {
    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform EarthShadow;
    public Transform Moon;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;

    // Use this for initialization
    void Start () {
        Sun.position = Vector3.zero;
        Mercury.position = new Vector3(4, 0, 0);
        Venus.position = new Vector3(5, 0, 0);
        Earth.position = new Vector3(7, 0, 0);
        Moon.position = new Vector3(7.4f, 0, 0.4f);
        Mars.position = new Vector3(9, 0, 0);
        Jupiter.position = new Vector3(12, 0, 0);
        Saturn.position = new Vector3(16, 0, 0);
        Uranus.position = new Vector3(19, 0, 0);
        Neptune.position = new Vector3(21, 0, 0);
        EarthShadow.position = Earth.position;
    }
	
	// Update is called once per frame
	void Update () {
        EarthShadow.position = Earth.position;
    }
}
