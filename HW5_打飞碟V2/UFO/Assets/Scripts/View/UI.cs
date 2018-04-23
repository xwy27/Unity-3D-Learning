/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/17 9:49:16
 *  Filename    ：UI
 *  Version     : V1.0.1  
 *  Description : Present the score and game statu
 **************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    GameObject scoreText;
    GameObject gameStatuText;
    IScore score = FirstSceneControllerBase.GetFirstSceneControllerBase() as IScore;
    IQueryStatus gameStatu = FirstSceneControllerBase.GetFirstSceneControllerBase() as IQueryStatus;

    // Use this for initialization
    void Start() {
        scoreText = GameObject.Find("Score");
        gameStatuText = GameObject.Find("GameStatu");
    }

    // Update is called once per frame
    void Update() {
        string score = Convert.ToString(this.score.GetScore());
        if (gameStatu.QueryGameStatus() == GameStatus.Lose) {
            gameStatuText.GetComponent<Text>().text = "Game Over!";
        } else if (gameStatu.QueryGameStatus() == GameStatus.Win) {
            gameStatuText.GetComponent<Text>().text = "You Win!";
        }
        scoreText.GetComponent<Text>().text = "Press space to send UFO.\n" +
            "Click UFO to gain Score.\n\n\n" +
            "Score: " + score;
    }
}
