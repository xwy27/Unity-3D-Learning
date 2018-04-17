/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/15 13:07:10
 *  Filename    ：UFOFactory
 *  Version     : V1.0.1  
 *  Description : Contorl UFOs' create and recycle
 **************************************************/

using System.Collections.Generic;
using UnityEngine;

public class UFOFactoryBase : System.Object {
    public GameObject UFOPrefab;

    private static UFOFactoryBase ufoFactory;
    List<GameObject> inUseUFO;
    List<GameObject> notUseUFO;

    private UFOFactoryBase() {
        notUseUFO = new List<GameObject>();
        inUseUFO = new List<GameObject>();
    }

    public static UFOFactoryBase GetFactory() {
        return ufoFactory ?? (ufoFactory = new UFOFactoryBase());
    }

    public List<GameObject> PrepareUFO(int UFOnum) {
        for (int i = 0; i < UFOnum; i++) {
            if (notUseUFO.Count == 0) {
                GameObject disk = Object.Instantiate(UFOPrefab);
                inUseUFO.Add(disk);
            } else {
                GameObject disk = notUseUFO[0];
                notUseUFO.RemoveAt(0);
                inUseUFO.Add(disk);
            }
        }
        return inUseUFO;
    }

    public void RecycleUFO(GameObject UFO) {
        int index = inUseUFO.FindIndex(x => x == UFO);
        notUseUFO.Add(UFO);
        inUseUFO.RemoveAt(index);
    }
}

public class UFOFactory : MonoBehaviour {
    UFOFactoryBase _ufoFactory;
    public GameObject ufoPrefab;

    void Awake() {
        _ufoFactory = UFOFactoryBase.GetFactory();
        _ufoFactory.UFOPrefab = ufoPrefab;
    }
}