using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatrolData : ScriptableObject {
  public float speed = 1.0f;
  public List<Vector3> patrolPoints = new List<Vector3>();
}
