using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GameEvents {
  public static void ChasePlayer(IPatrol patrol, Transform player) {
    if (Singleton<SceneController>.Instance.isGameOver || patrol.dropOff) {
      return;
    }
    patrol.discover = true;
    patrol.chasePlayer = player;
  }

  public static void CatchPlayer(IPatrol patrol) {
    if (Singleton<SceneController>.Instance.isGameOver || patrol.dropOff) {
      return;
    }
    Singleton<SceneController>.Instance.isGameOver = true;
  }

  public static void DropPlayer(IPatrol patrol) {
    if (Singleton<SceneController>.Instance.isGameOver || patrol.dropOff) {
      return;
    }
    patrol.ChangeColor();
    Singleton<SceneController>.Instance.AddScore((int)patrol.speed);
    patrol.discover = false;
    patrol.chasePlayer = null;
    patrol.dropOff = true;
  }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
  protected static T instance;

  public static T Instance {
    get {
      if (instance == null) {
        instance = (T)FindObjectOfType(typeof(T));
        if (instance == null) {
          Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
        }
      }
      return instance;
    }
  }
}