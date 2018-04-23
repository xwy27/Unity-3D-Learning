# UFO

[TOC]

简单的打飞碟游戏

**[运动学 Video 演示](http://www.iqiyi.com/w_19rxyfmzhp.html)**
**[物理运动 Video 演示](http://www.iqiyi.com/w_19ryb0juc5.html)**

**[个人github](https://github.com/xwy27/Unity-3D-Learning)**

## 飞碟游戏_V2.0

### 规则

1. 按下空格发射飞碟
1. 点击飞碟得分
1. 飞碟落地则扣分
1. 难度随次数增加

### 细节

#### 代码文件目录

+ Controller
    + Interface.cs  
        *Interfaces and Enum variables*
    + FirstSceneController.cs  
        *FirstController logic codes*
    + PhysicActionController.cs  
        *physical action Controller*
    + CCActionController.cs  
        *action Controller*
    + UFOFactory.cs  
        *UFO controller for creating and recycling UFO*
    + UserAction.cs  
        *User click or space press controller*
+ Model
    + GameModel.cs  
        *Score model*
    + UFOModel.cs  
        *UFO model*
+ View
    + UI.cs  
        *Game hint, score and status presentation*

#### 制作过程

这次学习了物理运动，将其加载到飞碟的运动上。但是对于之前学过的运动方法，并不想直接放弃。  
于是 adapter 模式出现了。也就是说，我们需要抽象出一个接口，然后物理运动与之前的运动方法都继承这个接口，达到代码复用的目的。

对于之前的代码，都没有什么要更改的，这就是代码架构带来的好处。我们需要做的事情，抽象接口，书写物理运动过程，就好啦。  

接口代码

```cs
public interface IActionController {
    int GetRound();
    int GetUFONum();
    void SendUFO(List<GameObject> usingUFOs);
    void DestroyUFO(GameObject UFO);
    void SceneUpdate();
}
```

我们来对比看看两者的区别。

+ 基础运动代码(CCActionManager.cs)
    ```cs
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
            //Ensure the rigidbody component does not disturb the original sport
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
    ```

+ 物理运动代码(PhysicActionManager.cs)
    ```cs
    public class PhysicActionManager : MonoBehaviour, IActionController {
        public int Round { get; set; }
        public int UFONum { get; private set; }
        private UFOModel ufoModel = new UFOModel();

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
                usingUFOs[i].transform.position = new Vector3(startPos.x, startPos.y + i, startPos.z);

                Rigidbody rigibody;
                rigibody = usingUFOs[i].GetComponent<Rigidbody>();
                rigibody.WakeUp();
                rigibody.useGravity = true;
                rigibody.AddForce(ufoModel.startDirection * Random.Range(ufoModel.UFOSpeed * 5, ufoModel.UFOSpeed * 8) / 5, 
                    ForceMode.Impulse);

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
            Reset(Round);
        }

        private void Update() {
            if (inUseUFOs != null) {
                for (int i = 0; i < inUseUFOs.Count; i++) {
                    //Debug.Log(inUseUFOs[i].transform.localScale.y);
                    if (inUseUFOs[i].transform.position.y <= 1f) {
                        //Debug.Log(inUseUFOs[i].transform.position.y);
                        FirstSceneControllerBase.GetFirstSceneControllerBase().DestroyUFO(inUseUFOs[i]);
                        FirstSceneControllerBase.GetFirstSceneControllerBase().SubScore();
                    }
                }
                if (inUseUFOs.Count == 0) {
                    FirstSceneControllerBase.GetFirstSceneControllerBase().SetSceneStatus(SceneStatus.Waiting);
                }
            }
        }
    }
    ```
二者代码量差不多，但是从视频效果可以看出，明显物理运动的方法，更接近现实，而且感官效果更好啊。如果基础运动代码要达到这个效果，代码量是几何倍数增加。点赞物理运动方法!