---
title: Unity-太阳系
date: 2018-03-29 14:17:01
tags: Unity-3d
category: Unity-3d
---
# Homework_2

Solar System

<!--more-->

## 简答

1. 游戏对象运动的本质是什么？

    > 游戏对象坐标的变换

## 编程

1. 请用三种方法以上方法，实现物体的抛物线运动。
    1. 修改 transform 属性
        ```cs
        private float origin = Time.deltaTime;
        private bool flag = true;

        // Update is called once per frame
        void Update () {
            if (flag) {
                if (origin - 100 * Time.deltaTime < 0.00001) {
                    transform.position += Vector3.down * Time.deltaTime;
                    transform.position += Vector3.right * Time.deltaTime;
                } else if (origin - 200 * Time.deltaTime < 0.00001) {
                    transform.position += Vector3.up * Time.deltaTime;
                    transform.position += Vector3.right * Time.deltaTime;
                } else {
                    flag = false;
                }
                origin += Time.deltaTime;
            } else {
                if (origin - 100 * Time.deltaTime > 0.00001) { 
                    transform.position += Vector3.down * Time.deltaTime;
                    transform.position += Vector3.left * Time.deltaTime;
                } else if (origin > 0.00001) {
                    transform.position += Vector3.up * Time.deltaTime;
                    transform.position += Vector3.left * Time.deltaTime;
                } else {
                    flag = true;
                }
                origin -= Time.deltaTime;
            }
        }
        ```
    1. transform.Translate
        ```cs
        // Update is called once per frame
        void Update () {
            if (flag) {
                if (origin - 100 * Time.deltaTime < 0.00001) {
                    Vector3 target = Vector3.right * Time.deltaTime + Vector3.down * Time.deltaTime;
                    transform.Translate(target, Space.World);
                } else if (origin - 200 * Time.deltaTime < 0.00001) {
                    Vector3 target = Vector3.right * Time.deltaTime + Vector3.up * Time.deltaTime;
                    transform.Translate(target, Space.World);
                } else {
                    flag = false;
                }
                origin += Time.deltaTime;
            } else { /*backward*/ }
        }
        ```
    1. Vector3.MoveTowards
        ```cs
        private float step = Time.deltaTime;

        // Update is called once per frame
        void Update () {
            float step = Time.deltaTime;
            if (flag) {
                if (origin - 100 * Time.deltaTime < 0.00001) {
                    Vector3 target = transform.position + Vector3.right * Time.deltaTime + Vector3.down * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, target, step);
                } else if (origin - 200 * Time.deltaTime < 0.00001) {
                    Vector3 target = transform.position + Vector3.right * Time.deltaTime + Vector3.up * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, target, step);
                } else {
                    flag = false;
                }
                origin += Time.deltaTime;
            } else { /*backward*/ }
        }
        ```
    1. Vector3.Lerp
        ```cs
        void Update {
            if (flag) {
              if (origin - 100 * Time.deltaTime < 0.00001) {
                  Vector3 target = transform.position + Vector3.right * Time.deltaTime + Vector3.down * Time.deltaTime;
                  transform.position = Vector3.Lerp(transform.position, target, 1);
              } else if (origin - 200 * Time.deltaTime < 0.00001) {
                  Vector3 target = transform.position + Vector3.right * Time.deltaTime + Vector3.up * Time.deltaTime;
                  transform.position = Vector3.Lerp(transform.position, target, 1);
              } else {
                  flag = false;
              }
              origin += Time.deltaTime;
          } else { /*backward*/ }
        }
        ```