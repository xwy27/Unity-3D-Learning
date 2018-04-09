using MyNamespace;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction{
    private Vector3 terrainPos;
    private UserGUI userGUI;
    private FirstSceneActionManager actionManager;

    public CoastController rightCoastCtrl;
    public CoastController leftCoastCtrl;
    public BoatController boatCtrl;
    public MyNamespace.CharacterController[] characters;

    void Start() {
        actionManager = GetComponent<FirstSceneActionManager>();
    }

    void Awake() {
        Director director = Director.GetInstance();
        director.CurrentSecnController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characters = new MyNamespace.CharacterController[6];
        LoadResources();
    }

    public void LoadResources() {
        terrainPos = new Vector3(-100, -7, -100);
        Terrain terrain = Instantiate(Resources.Load("Prefab/Terrain", typeof(Terrain)),
                            terrainPos, Quaternion.identity, null) as Terrain;
        terrain.name = "terrain";

        rightCoastCtrl = new CoastController("right");
        leftCoastCtrl = new CoastController("left");

        boatCtrl = new BoatController();

        for (int i = 0; i < 3; ++i) {
            MyNamespace.CharacterController temp = new MyNamespace.CharacterController("priest" + i);
            temp.SetPosition(rightCoastCtrl.GetEmptyPosition());
            temp.GetOnCoast(rightCoastCtrl);
            rightCoastCtrl.GetOnCoast(temp);
            characters[i] = temp;
        }

        for (int i = 0; i < 3; ++i) {
            MyNamespace.CharacterController temp = new MyNamespace.CharacterController("devil" + i);
            temp.SetPosition(rightCoastCtrl.GetEmptyPosition());
            temp.GetOnCoast(rightCoastCtrl);
            rightCoastCtrl.GetOnCoast(temp);
            characters[i + 3] = temp;
        }
    }

    private int CheckGameOver() {
        int rightPriest = 0;
        int rightDevil = 0;
        int leftPriest = 0;
        int leftDevil = 0;
        int status = 0;

        rightPriest += rightCoastCtrl.GetCharacterNum()[0];
        rightDevil += rightCoastCtrl.GetCharacterNum()[1];
        leftPriest += leftCoastCtrl.GetCharacterNum()[0];
        leftDevil += leftCoastCtrl.GetCharacterNum()[1];

        // Win
        if (leftPriest + leftDevil == 6) {
            status = 2; 
        }
        
        if (boatCtrl.boat.Location == Location.right) {
            rightPriest += boatCtrl.GetCharacterNum()[0];
            rightDevil += boatCtrl.GetCharacterNum()[1];
        } else {
            leftPriest += boatCtrl.GetCharacterNum()[0];
            leftDevil += boatCtrl.GetCharacterNum()[1];
        }

        // Lose
        if ((rightPriest < rightDevil && rightPriest > 0) ||
            (leftPriest < leftDevil && leftPriest > 0)) {
            status = 1;
        }

        return status;
    }

    public void MoveBoat() {
        if (boatCtrl.IsEmpty()) return;
        //修改船的移动动作和过程处理
        actionManager.MoveBoat(boatCtrl);
        boatCtrl.SetPos();
        UserGUI.status = CheckGameOver();
    }

    public void CharacterClicked(MyNamespace.CharacterController characterCtrl) {
        if (characterCtrl.character.IsOnBoat) {
            CoastController tempCoast = (boatCtrl.boat.Location == Location.right ? rightCoastCtrl : leftCoastCtrl);
            boatCtrl.GetOffBoat(characterCtrl.character.Name);
            actionManager.MoveCharacter(characterCtrl, tempCoast.GetEmptyPosition());
            characterCtrl.GetOnCoast(tempCoast);
            tempCoast.GetOnCoast(characterCtrl);
        } else {
            CoastController tempCoast = characterCtrl.character.Coast;
            if (tempCoast.coast.Location != boatCtrl.boat.Location) return;     // Boat at another side

            if (boatCtrl.GetEmptyIndex() == -1) return;                         // Boat is full

            tempCoast.GetOffCoast(characterCtrl.character.Name);
            actionManager.MoveCharacter(characterCtrl, boatCtrl.GetEmptyPosition());
            characterCtrl.GetOnBoat(boatCtrl);
            boatCtrl.GetOnBoat(characterCtrl);

        }
        UserGUI.status = CheckGameOver();
    }

    public void Restart() {
        boatCtrl.Reset();
        rightCoastCtrl.Reset();
        leftCoastCtrl.Reset();
        for (int i = 0; i < 6; ++i) {
            characters[i].Reset();
        }
    }
}