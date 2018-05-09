---
title: Unity-Patrol
date: 2018-05-09 21:34:32
tags: Unity-3d
category: Unity-3d
---
# 躲避游戏(智能巡逻兵)

[作业要求](https://pmlpml.github.io/unity3d-learning/07-model-and-animation)
[个人github](https://github.com/xwy27/Unity-3D-Learning)
[演示视频](http://www.iqiyi.com/w_19rxsuoort.html)


<!-- TOC -->

- [躲避游戏(智能巡逻兵)](#躲避游戏智能巡逻兵)
  - [概述](#概述)
  - [制作巡逻兵](#制作巡逻兵)
  - [制作巡逻路径](#制作巡逻路径)
  - [制作巡逻兵工厂](#制作巡逻兵工厂)
  - [发布订阅者模式](#发布订阅者模式)
  - [制作玩家](#制作玩家)
  - [杂项](#杂项)

<!-- /TOC -->

## 概述

这次的游戏是基于智能巡逻兵作业的"困难"躲避游戏。水平有限，UI 较为简陋。  
圆球为玩家，通过上下左右方向键控制；立方体为巡逻兵，当玩家出现在侦察范围会自动追击，追到则失败。  
玩家**摆脱巡逻兵**，巡逻兵变色，依旧巡逻，但**不会再次追逐玩家**。  
多次点击开始按钮可以生成更多巡逻兵。  
巡逻兵的巡逻路线采用随机生成矩形，然后再在边上随机选点构成多边形，作为巡逻路线。

## 制作巡逻兵

首先，给一个立方体挂载一个 spehere collider，并设置半径为 10 和勾选 trigger 作为触发器，用来探测玩家。  
然后编写其模板代码。  
每个巡逻兵，需要发现玩家的标志变量，存储巡逻路径的变量，追踪玩家动作，巡逻动作。  
巡逻兵平常进行巡逻动作，按照存储的巡逻路径，按顺序巡逻。当发现玩家，停止动作，执行追逐玩家动作。如果被甩掉，则变色，并继续执行巡逻动作，但不会再次追逐玩家。  
*代码中的注释可以辅助理解。*

```cs
// flag for if dropped off by player
public bool dropOff = false;

// create the patrol path by several rectangles
public int curPointIndex;
private Vector3 nextPoint;
public List<Vector3> patrolPoints;

// Player discovered, chase player
public bool discover = false;
public Transform chasePlayer;

// Action for chasing player
public Action<IPatrol, Transform> OnDiscoverPlayer;
// Action for catching  player
public Action<IPatrol> OnCatchPlayer;
// Action for dropping off player
public Action<IPatrol> OnDropPlayer;

/**
  * Set patrol data from patrolData
  * @return patrol itself
  */
public IPatrol SetFromData(IPatrolData data) {
    patrolPoints = data.patrolPoints;
    speed = data.speed;
    discover = false;
    dropOff = false;
    return this;
}

/**
  * Enable the patrol
  * @return patrol itself
  */
public IPatrol StartPatrol() {
    discover = false;
    chasePlayer = null;
    nextPoint = patrolPoints[curPointIndex];
    isEnabled = true;
    return this;
}

/**
  * Initialize the position
  * @return patrol itself
  */
public IPatrol InitialPosition() {
    transform.position = patrolPoints[0];
    return this;
}

/**
  * Reset all actions
  * @return patrol itself
  */
public IPatrol ClearActions() {
    OnCatchPlayer = null;
    OnDropPlayer = null;
    OnDiscoverPlayer = null;
    return this;
}

/**
  * initialize red color
  * @return patrol itself
  */
public IPatrol InitialColor() {
    Renderer render = GetComponent<Renderer>();
    render.material.shader = Shader.Find("Transparent/Diffuse");
    render.material.color = Color.red;
    return this;
}

/**
  * change green after dropping off player
  * @return patrol itself
  */
public IPatrol ChangeColor() {
    Renderer render = GetComponent<Renderer>();
    render.material.shader = Shader.Find("Transparent/Diffuse");
    render.material.color = Color.green;
    return this;
}

/**
  * move to next patrol point
  */
public void ChangeDirection() {
    if (++curPointIndex == patrolPoints.Count) {
        curPointIndex -= patrolPoints.Count;
    }
    nextPoint = patrolPoints[curPointIndex];
}

// Use this for initialization
void Start () {
    isEnabled = true;
    curPointIndex = 0;
}

// Update is called once per frame
void Update () {
if (isEnabled) {
        Vector3 target = discover ? chasePlayer.position : nextPoint;
        transform.localPosition = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);

        if (!discover &&
            Vector3.Distance(transform.position, nextPoint) < 0.5f) {
            ChangeDirection();
        }

        if (discover && Vector3.Distance(transform.position, target) < 1f) {
            if (OnCatchPlayer != null) {
                OnCatchPlayer(this);
            }
        }
    }
}

private void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag.Contains("Player")) {
        if (OnDiscoverPlayer != null) {
            OnDiscoverPlayer(this, other.transform);
        }
    }
}

private void OnTriggerExit(Collider other) {
    if (other.gameObject.tag.Contains("Player")) {
        if (OnDropPlayer != null) {
            OnDropPlayer(this);
        }
    }
}
```

## 制作巡逻路径

巡逻路径，采取生成随机点列表作为路径，让巡逻兵从一个点，运动到另一个点，作为巡逻动作。  
关于随机点生成，我们首先生成**一个随机的矩形**左下角点和边长，然后算出其余三个顶点。接着在它的四条边，**每条边随机选取一个点**，作为巡逻路径点，这样可以得到一个随机的凸四边形。  
这里支持生成凸多边形，只要在边上随机取点时，每次多取一个，然后按顺序加入巡逻列表，那么最多可以生成凸八边形。  
*这个巡逻路径，耗时比较久，也就没时间考虑 UI 了。*

```cs
public float positionRange = 20.0f;
public float defaultSideLength = 5.0f;
public float yPosition = 1.0f;

/**
  * Genrate the patrol path from a random rectangle
  * by selecting points on its side randomly.
  * @return {List<Vector2>} selected points.
  */
public List<Vector3> GetRandomRect(int sides = 4, float sideLength = 0) {
    List<Vector3> rect = new List<Vector3>();

    if (sideLength == 0) {
        sideLength = defaultSideLength;
    }

    sideLength = Random.Range(10f, 20f);
    Vector3 leftDown = new Vector3(
        Random.Range(-positionRange, positionRange), yPosition, Random.Range(-positionRange, positionRange));
    Vector3 rightDown = leftDown + Vector3.right * sideLength;
    Vector3 rightUp = leftDown + Vector3.forward * sideLength;
    Vector3 leftUp = rightDown + Vector3.forward * sideLength;

    Vector3 temp = leftDown + Vector3.forward * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);
    temp = leftUp + Vector3.right * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);
    temp = rightUp + Vector3.forward * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);

    if (sides >= 4) {
        temp = rightDown + Vector3.right * (-sideLength) * Random.Range(0f, 0.5f);
        rect.Add(temp);
        if (sides == 5) {
            temp = rightDown + Vector3.right * (-sideLength) * Random.Range(0f, 0.5f);
            rect.Add(temp);
        }
    }
    return rect;
}
```

## 制作巡逻兵工厂

工厂模式，不是什么陌生的事情了。这里只有巡逻兵的工厂，因为玩家只有一个，所以也只需要给巡逻兵配备工厂了。没有就从闲置资源获得，不够就生产，大致就是这个逻辑了。

```cs
private GeneratePatrolPath pathGenerator;
  private List<GameObject> inUse;
  private List<GameObject> notUse;
  int patrolCount = 0;

  [Header("Patrol Prefab")]
  public GameObject patrolPrefab;

  // Use this for initialization
  void Start() {
    pathGenerator = Singleton<GeneratePatrolPath>.Instance;
    inUse = new List<GameObject>();
    notUse = new List<GameObject>();
  }

  public void GeneratePatrol(int num) {
    for (int i = 0; i < num; ++i) {
      IPatrolData patrolData = ScriptableObject.CreateInstance<IPatrolData>();
      patrolData.speed = Random.Range(5f, 15f);
      patrolData.patrolPoints = pathGenerator.GetRandomRect();

      GameObject patrol = null;
      if (notUse.Count > 0) {
        patrol = notUse[0];
        notUse.RemoveAt(0);
      } else {
        patrolCount++;
        patrol = Instantiate(patrolPrefab) as GameObject;
        patrol.name = "Patrol" + patrolCount.ToString();
      }
      patrol.GetComponent<IPatrol>().SetFromData(patrolData).ClearActions().InitialColor().InitialPosition().StartPatrol();

      patrol.GetComponent<IPatrol>().OnCatchPlayer += GameEvents.CatchPlayer;
      patrol.GetComponent<IPatrol>().OnDiscoverPlayer += GameEvents.ChasePlayer;
      patrol.GetComponent<IPatrol>().OnDropPlayer += GameEvents.DropPlayer;

      inUse.Add(patrol);
    }
  }

  public void ReleaseAllPatrols() {
    for (int i = inUse.Count - 1; i >= 0; --i) {
      GameObject obj = inUse[i];
      obj.GetComponent<IPatrol>().InitialColor().InitialPosition().isEnabled = false;
      notUse.Add(obj);
      inUse.RemoveAt(i);
    }
  }
```

## 发布订阅者模式

对一个事件做出反应的东西往往有很多个，那个时候，将事件的感知和处理逻辑写在一起，是一种很难以维护的方式。

比较典型一点的构造应当是：多个订阅者拥有自己的对同一类事件的各自的事件处理逻辑，然后在每个订阅者实例化的时候，向一个专门负责事件发布与接收的地方注册自己的事件处理函数（回调函数），然后发布者在感知并产生事件之后，向同样的负责事件分发的地方传送自己的事件，然后该事件依次进入不同订阅者的逻辑中，实施其自己的工作。

因为对于这个游戏来说，只有巡逻兵对事件发生感知并发生事件，事件逻辑已经足够简单，所以发布者和订阅者都可以简化成一个：发布者是每个巡逻兵，订阅者是SceneController。所以在通过这部分代码理解发布订阅模式时，多方订阅的部分可能需要自己继续想象。  
基于本次游戏的逻辑简单性，简化了事件分发的逻辑，直接用了一个static类把事件处理逻辑写在了上面，实际上并不完全是上面说的结构。

```cs
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
```

## 制作玩家

给一个圆球挂载碰撞器和刚体并做成预制。

>在这里，我要提出一个 de 了两天的 bug! 碰撞发生的条件是双方具有碰撞器，且一方具有刚体，触发的条件是双方具有碰撞器，一方具有刚体，一方的碰撞器勾选了 isTrigger。我一开始就按照这个去设计的巡逻兵与玩家的预制。但是，触发可以触发，碰撞却完全没有被检测到。在 onCollisionEnter中的输出语句也没有执行，甚至两个物体撞在了一起，玩家"进入"了巡逻兵内部。查找了很多资料，比如连续检测，连续动态检测，降低速度等等，都没有解决这个问题。

玩家的逻辑代码很简单，主要是移动函数:

```cs
void FixedUpdate() {
  float translationZ = Input.GetAxis("Vertical");
  float translationX = Input.GetAxis("Horizontal");

  Vector3 temp = new Vector3(translationX, 0, translationZ);

  player.transform.Translate(temp.normalized / 1.5f);
}
```

## 杂项

剩下的都是以前写过的代码，就不一一贴出来了。具体前往[github](https://github.com/xwy27/Unity-3D-Learning)查看。