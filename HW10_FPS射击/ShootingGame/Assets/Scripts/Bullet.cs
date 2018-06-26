using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnCollisionEnter(Collision collision) {
    var hit = collision.gameObject;
    var player = hit.GetComponent<PlayerController>();
    if (player != null) {
      var health = hit.GetComponent<Health>();
      health.HealthDown(10);
      Destroy(gameObject);
    }
  }
}
