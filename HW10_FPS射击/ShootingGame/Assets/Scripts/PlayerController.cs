using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
  public float rotationSpeed = 150.0f;
  public float moveSpeed = 3.0f;
  public GameObject bulletPrefab;
  public Transform bulletSpawn;
  public GameObject fpsCameraPrefab;
  public Transform healthBar;

  private float x, z;
	
  // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    if (!isLocalPlayer) {
      return;
    }
    PlayerMove();	
    if (Input.GetKeyDown(KeyCode.Space)) {
      CmdFire();
    }
	}

  void PlayerMove() {
    x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
    z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
    transform.Rotate(0, x, 0);
    transform.Translate(0, 0, z);
  }

  [Command]
  void CmdFire() {
    // create bullet gameobject
    var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
    // set bullet fire direction
    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
    // broadcast bullet to all client
    NetworkServer.Spawn(bullet);
    // destroy bullet after 2 seconds
    Destroy(bullet, 2.0f);
  }

  public override void OnStartLocalPlayer() {
    if (Camera.main != null) {
      GameObject.Destroy(Camera.main);
    }
    GameObject fpscamera = Instantiate(fpsCameraPrefab) as GameObject;
    fpscamera.tag = "MainCamera";
    fpscamera.transform.parent = this.transform;
    fpscamera.transform.localPosition = new Vector3(0, 0.5f, 0);
    fpscamera.transform.localRotation = Quaternion.identity;
    GetComponent<MeshRenderer>().material.color = Color.blue;
  }
}
