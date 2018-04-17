/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/17 9:49:16
 *  Filename    ：UserAction
 *  Version     : V1.0.1  
 *  Description : User action listener
 **************************************************/

using UnityEngine;

public class UserAction : MonoBehaviour {
    public GameObject planePrefab;
    
    GameStatus gameStatus;
    SceneStatus SceneStatus;

    IUserInterface uerInterface;
    IQueryStatus queryStatus;
    IScore changeScore;

    // Use this for initialization
    void Start() {
        GameObject plane = Instantiate(planePrefab);
        plane.transform.position = new Vector3(0f, 0f, 70f);

        gameStatus = GameStatus.Play;
        SceneStatus = SceneStatus.Waiting;
        uerInterface = FirstSceneControllerBase.GetFirstSceneControllerBase() as IUserInterface;
        queryStatus = FirstSceneControllerBase.GetFirstSceneControllerBase() as IQueryStatus;
        changeScore = FirstSceneControllerBase.GetFirstSceneControllerBase() as IScore;
    }

    // Update is called once per frame
    void Update() {
        gameStatus = queryStatus.QueryGameStatus();
        SceneStatus = queryStatus.QuerySceneStatus();

        if (gameStatus == GameStatus.Play) {
            if (SceneStatus == SceneStatus.Waiting && Input.GetKeyDown("space")) {
                uerInterface.SendUFO();
            }
            if (SceneStatus == SceneStatus.Shooting && Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "UFO") {
                    uerInterface.DestroyUFO(hit.collider.gameObject);
                    changeScore.AddScore();
                }
            }
        }
    }
}

