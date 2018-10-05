# Unity-3D-Learning

1. HomeWork_1
    1. 基础概念
        有关 Unity 的相关名词和概念理解
    1. Tic-Tac-Toe
        亮点：
        + Single player
        + Two players
1. HomeWork_2

    学习了 Prehab，Material 等相关操作，但时间匆忙，本次未补充进阶操作qwq

    1. 太阳系
        一个简易但并不那么精确的太阳系模型
    1. 牧师与魔鬼
        点击 GameObject 进行操作
1. HomeWork_3

    1. 对牧师与魔鬼游戏进行改进，添加动作管理器。
    1. 美化 UI
        1. Terrain
        1. Water Environment

1. HomeWork_4

    1. 打飞碟游戏
        1. 空格发射飞碟，点击飞碟得分
        1. 难度随次数增加
    1. My Component
        自定义 Editor 并制成预制

1. HomeWork_5

    1. 打飞碟游戏V2.0

1. HomeWork_6

    1. 躲避游戏
        1. 玩家通过方向按键控制
        1. 被巡逻兵追到则失败
        1. 甩开巡逻兵，则该巡逻兵不再追逐

1. HomeWork_7

    1. ParticleRing
        模仿 [remember](http://i-remember.fr/en) 的粒子光环效果。 其中，鼠标悬浮光环中心，光环会收缩，移开后恢复

1. HomeWork_8

    1. GUI 系统
        GUI 信纸滚动条，可自动根据内容扩展长度。
        [Blog](https://xwy27.github.io/Unity-3d/UI-BulletinBoard/)

1. HomeWork_9

    1. 智能AI
        重写 [牧师与魔鬼](https://github.com/xwy27/Unity-3D-Learning/tree/master/HW2_%E7%89%A7%E5%B8%88%E4%B8%8E%E9%AD%94%E9%AC%BC)，添加状态图转换的寻路，达到 AI 效果
        1. 角色颜色区分：绿色牧师，红色魔鬼，棕色船只
        1. 根据当前状态给出最短正确路径的下一状态 交由代码运算，而非直接根据手写状态图进行简单的 if-else 给出
        优点：游戏状态增加时，只需更改状态变化的逻辑代码，而不必重写状态图
