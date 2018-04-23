/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/17 9:06:51
 *  Filename    ：Scene
 *  Version     : V1.0.1  
 *  Description : Action Control
 **************************************************/

using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : MonoBehaviour, IActionController {
    public int Round { get; set; }
    public int UFONum { get; private set; }
    private UFOModel ufoModel = new UFOModel();
    private Vector3 target;

    List<GameObject> inUseUFOs;

    public void Reset(int round) {
        Round = round;
        UFONum = round;
        ufoModel.Reset(round);
    }

    public int GetRound() {
        return Round;
    }

    public int GetUFONum() {
        return UFONum;
    }

    public void SendUFO(List<GameObject> usingUFOs) {
        inUseUFOs = usingUFOs;
        Reset(Round);
        for (int i = 0; i < usingUFOs.Count; i++) {
            usingUFOs[i].GetComponent<Renderer>().material.color = ufoModel.UFOColor;

            var startPos = ufoModel.startPos;
            usingUFOs[i].transform.position = startPos;

            FirstSceneControllerBase.GetFirstSceneControllerBase().SetSceneStatus(SceneStatus.Shooting);
        }
    }

    public void DestroyUFO(GameObject UFO) {
        UFO.GetComponent<Rigidbody>().Sleep();
        UFO.GetComponent<Rigidbody>().useGravity = false;
        UFO.transform.position = new Vector3(0f, -99f, 0f);
    }

    public void SceneUpdate() {
        Round++;
        Reset(Round);
    }

    private void Start() {
        Round = 1;
        target = new Vector3(-3f, 10f, -2f);
        Reset(Round);
    }

    private void Update() {
        if (inUseUFOs != null) {
            for (int i = 0; i < inUseUFOs.Count; i++) {
                //Debug.Log(inUseUFOs[i].transform.localScale.y);
                if (inUseUFOs[i].transform.position == target) {
                    //Debug.Log(inUseUFOs[i].transform.position.y);
                    FirstSceneControllerBase.GetFirstSceneControllerBase().DestroyUFO(inUseUFOs[i]);
                    FirstSceneControllerBase.GetFirstSceneControllerBase().SubScore();
                } else {
                    inUseUFOs[i].transform.position = Vector3.MoveTowards(inUseUFOs[i].transform.position, target, 5 * Time.deltaTime);
                }
            }
            if (inUseUFOs.Count == 0) {
                FirstSceneControllerBase.GetFirstSceneControllerBase().SetSceneStatus(SceneStatus.Waiting);
            }
        }
    }
}