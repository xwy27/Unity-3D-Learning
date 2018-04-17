/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/15 13:07:10
 *  Filename    ：Interface
 *  Version     : V1.0.1  
 *  Description : Interfaces and Enum variables
 **************************************************/

using UnityEngine;

public enum GameStatus {
    Play, Win, Lose
}

public enum SceneStatus {
    Waiting, Shooting
}

public interface IUserInterface {
    void SendUFO();
    void DestroyUFO(GameObject ufo);
}

public interface IScore {
    void AddScore();
    void SubScore();
    int GetScore();
}

public interface IQueryStatus {
    GameStatus QueryGameStatus();
    SceneStatus QuerySceneStatus();
}

public interface ISetStatus {
    void SetGameStatus(GameStatus gameStatus);
    void SetSceneStatus(SceneStatus scenceStatus);
}