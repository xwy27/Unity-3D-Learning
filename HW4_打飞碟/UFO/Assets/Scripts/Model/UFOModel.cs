/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/17 14:37:57
 *  Filename    ：UFOModel
 *  Version     : V1.0.1  
 *  Description : UFO datas
 **************************************************/

using UnityEngine;

public class UFOModel {
    public Color UFOColor;
    public Vector3 startPos;
    public Vector3 startDirection;
    public float UFOSpeed;

    public void Reset(int round) {
        UFOSpeed = 0.1f;
        if (round % 2 == 1) {
            UFOColor = Color.red;
            startPos = new Vector3(-5f, 3f, -15f);
            startDirection = new Vector3(3f, 8f, 5f);
        } else {
            UFOColor = Color.green;
            startPos = new Vector3(5f, 3f, -15f);
            startDirection = new Vector3(-3f, 8f, 5f);
        }
        for (int i = 1; i < round; i++) {
            UFOSpeed *= 1.1f;
        }
    }
}