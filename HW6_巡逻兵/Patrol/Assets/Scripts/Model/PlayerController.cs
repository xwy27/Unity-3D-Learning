using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  private GameObject player;
  public float speed = 2.0f;

  [Header("Player Prefab")]
  public GameObject playerPrefab;

  public void initialPos() {
    player = Instantiate(playerPrefab) as GameObject;
    player.transform.position = new Vector3(0, 1.0f, 0);
    player.tag = "Player";
  }

  // Use this for initialization
  void Start() {
    //player = Instantiate(playerPrefab) as GameObject;
    player.name = "Player";
  }

  void FixedUpdate() {
    float translationZ = Input.GetAxis("Vertical");
    float translationX = Input.GetAxis("Horizontal");

    Vector3 temp = new Vector3(translationX, 0, translationZ);

    player.transform.Translate(temp.normalized / 1.5f);
  }
}
