using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatrolFactory : MonoBehaviour {
  private GeneratePatrolPath pathGenerator;
  private List<GameObject> inUse;
  private List<GameObject> notUse;
  int patrolCount = 0;

  [Header("Patrol Prefab")]
  public GameObject patrolPrefab;

  // Use this for initialization
  void Start() {
    pathGenerator = Singleton<GeneratePatrolPath>.Instance;
    inUse = new List<GameObject>();
    notUse = new List<GameObject>();
  }

  public void GeneratePatrol(int num) {
    for (int i = 0; i < num; ++i) {
      IPatrolData patrolData = ScriptableObject.CreateInstance<IPatrolData>();
      patrolData.speed = Random.Range(5f, 15f);
      patrolData.patrolPoints = pathGenerator.GetRandomRect();

      GameObject patrol = null;
      if (notUse.Count > 0) {
        patrol = notUse[0];
        notUse.RemoveAt(0);
      } else {
        patrolCount++;
        patrol = Instantiate(patrolPrefab) as GameObject;
        patrol.name = "Patrol" + patrolCount.ToString();
      }
      patrol.GetComponent<IPatrol>().SetFromData(patrolData).ClearActions().InitialColor().InitialPosition().StartPatrol();

      patrol.GetComponent<IPatrol>().OnCatchPlayer += GameEvents.CatchPlayer;
      patrol.GetComponent<IPatrol>().OnDiscoverPlayer += GameEvents.ChasePlayer;
      patrol.GetComponent<IPatrol>().OnDropPlayer += GameEvents.DropPlayer;

      inUse.Add(patrol);
    }
  }

  public void ReleaseAllPatrols() {
    for (int i = inUse.Count - 1; i >= 0; --i) {
      GameObject obj = inUse[i];
      obj.GetComponent<IPatrol>().InitialColor().InitialPosition().isEnabled = false;
      notUse.Add(obj);
      inUse.RemoveAt(i);
    }
  }
}
