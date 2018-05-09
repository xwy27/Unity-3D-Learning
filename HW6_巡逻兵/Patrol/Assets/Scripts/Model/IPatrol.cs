using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatrol : MonoBehaviour {
  public bool isEnabled = false;
  public float speed = 1.0f;

  // flag for if dropped off by player
  public bool dropOff = false;

  // create the patrol path by several rectangles
  public int curPointIndex;
  private Vector3 nextPoint;
  public List<Vector3> patrolPoints;

  // Player discovered, chase player
  public bool discover = false;
  public Transform chasePlayer;

  // Action for chasing player
  public Action<IPatrol, Transform> OnDiscoverPlayer;
  // Action for catching  player
  public Action<IPatrol> OnCatchPlayer;
  // Action for dropping off player
  public Action<IPatrol> OnDropPlayer;

  /**
   * Set patrol data from patrolData
   * @return patrol itself
   */
  public IPatrol SetFromData(IPatrolData data) {
    patrolPoints = data.patrolPoints;
    speed = data.speed;
    discover = false;
    dropOff = false;
    return this;
  }

  /**
   * Enable the patrol
   * @return patrol itself
   */
  public IPatrol StartPatrol() {
    discover = false;
    chasePlayer = null;
    nextPoint = patrolPoints[curPointIndex];
    isEnabled = true;
    return this;
  }

  /**
   * Initialize the position
   * @return patrol itself
   */
  public IPatrol InitialPosition() {
    transform.position = patrolPoints[0];
    return this;
  }

  /**
   * Reset all actions
   * @return patrol itself
   */
  public IPatrol ClearActions() {
    OnCatchPlayer = null;
    OnDropPlayer = null;
    OnDiscoverPlayer = null;
    return this;
  }

  /**
   * initialize red color
   * @return patrol itself
   */
  public IPatrol InitialColor() {
    Renderer render = GetComponent<Renderer>();
    render.material.shader = Shader.Find("Transparent/Diffuse");
    render.material.color = Color.red;
    return this;
  }

  /**
   * change green after dropping off player
   * @return patrol itself
   */
  public IPatrol ChangeColor() {
    Renderer render = GetComponent<Renderer>();
    render.material.shader = Shader.Find("Transparent/Diffuse");
    render.material.color = Color.green;
    return this;
  }

  /**
   * move to next patrol point
   */
  public void ChangeDirection() {
    if (++curPointIndex == patrolPoints.Count) {
      curPointIndex -= patrolPoints.Count;
    }
    nextPoint = patrolPoints[curPointIndex];
  }

  // Use this for initialization
  void Start() {
    isEnabled = true;
    curPointIndex = 0;
  }

  // Update is called once per frame
  void Update() {
    if (isEnabled) {
      Vector3 target = discover ? chasePlayer.position : nextPoint;
      transform.localPosition = Vector3.MoveTowards(
          transform.position, target, speed * Time.deltaTime);

      if (!discover &&
          Vector3.Distance(transform.position, nextPoint) < 0.5f) {
        ChangeDirection();
      }

      if (discover && Vector3.Distance(transform.position, target) < 1f) {
        if (OnCatchPlayer != null) {
          OnCatchPlayer(this);
        }
      }
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag.Contains("Player")) {
      if (OnDiscoverPlayer != null) {
        OnDiscoverPlayer(this, other.transform);
      }
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.gameObject.tag.Contains("Player")) {
      if (OnDropPlayer != null) {
        OnDropPlayer(this);
      }
    }
  }
}
