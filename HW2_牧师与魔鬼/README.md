---
title: Unity-牧师与魔鬼
date: 2018-03-29 14:18:56
tags: Unity-3d
category: Unity-3d
---
# Homework_2

Priests and Devils

<!--more-->
**[个人github](https://github.com/xwy27/Unity-3D-Learning/tree/master/HW2_%E7%89%A7%E5%B8%88%E4%B8%8E%E9%AD%94%E9%AC%BC)**

阅读以下游戏脚本

Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many ways. Keep all priests alive! Good luck!

[play the game](http://www.flash-game.net/game/2535/priests-and-devils.html)

程序要求：

1. 回答
    + 列出游戏中提及的事物(Objects)
    > Priests, devils, stone, river, boat
    + 用表格列出玩家动作表(规则表。*动作越少越好*)
        |动作|结果|
        |-------|---------------|
        |点击人物|上/下船, 上/下岸|
        |点击船只|过河|
1. 限制
    + 请将游戏中对象做成预制
    + 在 GenGameObjects 中创建长方形、正方形、球及其色彩代表游戏中的对象
    + 使用 C# 集合类型有效组织对象
    + 整个游戏仅主摄像机和一个 Empty 对象，其他对象必须代码动态生成
    + 不许出现 Find 游戏对象，SendMessage 这类突破程序结构的通讯耦合语句
    + 请使用课件架构图编程，不接受非 MVC 结构程序
    + 注意细节，如：船未靠岸，牧师与魔鬼上下船运动中，不接受用户事件

实现思路：

1. 分离出 MVC 框架
    >Model: Coast, Boat, Character
    >
    >View: UserGUI
    >
    >Controller: BoatController, CoastController, CharacterController, FirstController, Director
1. 给定 Model 属性和值域

    枚举变量是很好的状态标记辅助变量，可以避免记忆整数标识的状态，而采用易懂的变量名替代。
    ```cs
    public enum Location { left, right }

    public class Character {
        public Moveable mScript;
        public GameObject Role { get; set; }
        public CoastController Coast { get; set; }
        public bool IsOnBoat { get; set; }
        public string Name {
            get {
                return Role.name;
            }
            set {
                Role.name = value;
            }
        }

        public Character(string _name) {
            //set properties
            if (_name.Contains("priest")) {
                // Instantiate priest
            } else {
                // Instantiate devil
            }
        }
    }

    public class Coast {
        readonly GameObject coast;
        readonly Vector3 departure;
        public readonly Vector3 destination;
        public readonly Vector3[] positions;

        public CharacterController[] characters;
        public Location Location { get; set; }

        public Coast(string _location) {
            //set properties
            if (_location == "right") {
                // Instantiate right coast
                coast.name = "departure";
                Location = Location.right;
            } else {
                // Instantiate left coast
                coast.name = "destination";
                Location = Location.left;
            }
        }
    }

    public class Boat {
        public readonly Moveable mScript;
        public readonly Vector3 departure;
        public readonly Vector3 destination;
        public readonly Vector3[] departures;
        public readonly Vector3[] destinations;
        public CharacterController[] passenger = new CharacterController[2];

        public GameObject _Boat { get; set; }
        public Location Location { get; set; }

        public Boat() {
            // Instantiate boat gameobject
        }
    }
    ```
1. View 实现
    + 物体预制
        见 github 代码项目的 prehab 文件夹
    + UserGUI
       UserGUI 代码见 [github](https://github.com/xwy27/Unity-3D-Learning/tree/master/HW2_%E7%89%A7%E5%B8%88%E4%B8%8E%E9%AD%94%E9%AC%BC)。
1. 实现各 Controller 和 Director

    Director 采取单例模式
    ```cs
    public class Director : System.Object {
        private static Director _instance;
        public ISceneController CurrentSceneController { get; set; }

        public static Director GetInstance() {
            // variable ?? (some code)
            // 这是个语法糖，表示判断前面的 variable 是否为空
            // 不为空则返回 variable
            // 为空则执行括号内的 some code
            return _instance ?? (_instance = new Director());
        }
    }
    ```

    Controller 代码见 [github](https://github.com/xwy27/Unity-3D-Learning/tree/master/HW2_%E7%89%A7%E5%B8%88%E4%B8%8E%E9%AD%94%E9%AC%BC)。

    其中，需要注意，上下船动作虽然简单，但也应该分解为，人物上/下船，船上/下人物，人物上/下岸，岸上/下人物。**因为这个动作的结果会对三个 GameObject 造成影响，所以我们要通知三个 GameObject 让其进行状态更新。** 这一点尤其要注意。
1. GameObject 运动脚本

    实现参考[博客](https://www.jianshu.com/p/07028b3da573)