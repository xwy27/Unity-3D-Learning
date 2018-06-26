using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
  public const int maxHealth = 100;
  [SyncVar(hook ="OnChangeHealth")]
  public int currntHealth = maxHealth;
  public RectTransform healthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void HealthDown(int amount) {
    if (!isServer) {
      return;
    }
    currntHealth -= amount;
    if (currntHealth <= 0) {
      currntHealth = maxHealth;
      RpcRespwan();
    }
  }

  void OnChangeHealth(int health) {

    healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
  }

  [ClientRpc]
  void RpcRespwan() {
    if (isLocalPlayer) {
      transform.position = Vector3.zero;
    }
  }
}
