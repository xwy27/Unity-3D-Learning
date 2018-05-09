using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
  private int score;
  private int dropOffPatrolNum = 0;

  public int patrolNum = 5;
  public IPatrolFactory factory;
  public PlayerController player;

  public bool isGameOver = false;
  public bool isWin = false;

  void Rest() {
    score = 0;
    dropOffPatrolNum = 0;
    isGameOver = false;
    isWin = false;
    factory.GeneratePatrol(patrolNum);
  }

  public void GameOver() {
    dropOffPatrolNum = 0;
    isGameOver = true;
    factory.ReleaseAllPatrols();
  }

  public void YouWin() {
    dropOffPatrolNum = 0;
    isWin = true;
    factory.ReleaseAllPatrols();
  }

  public void AddScore(int curScore) {
    if (patrolNum < curScore) {
      score += curScore;
      dropOffPatrolNum++;
      if (dropOffPatrolNum == patrolNum) {
        YouWin();
      }
    }
  }

  private void OnGUI() {
    if (GUI.Button(new Rect(20, 20, 50, 20), "Start")) {
      Rest();
    }
    GUI.Label(new Rect(100, 20, 150, 20), "Score:" + score.ToString());
    if (isGameOver) {
      GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 150, 20), "GameOver");
      GameOver();
      score = 0;
    }
    if (isWin) {
      GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 150, 20), "You Win!");
      score = 0;
    }
  }

  // Use this for initialization
  void Start() {
    factory = Singleton<IPatrolFactory>.Instance;
    player = Singleton<PlayerController>.Instance;
    player.initialPos();
  }
}
