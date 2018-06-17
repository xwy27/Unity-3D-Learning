using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MyNamespace {
  public class UserGUI : MonoBehaviour {
    private string guide = "Boat carries two people each time.\n" +
        "One person must drive the boat.\n" +
        "Click on person or boat to move them.\n" +
        "Priests killed when less than devils on one side.\n" +
        "Keep all priests alive! \n" +
        "Good luck!\n";
    private string hint = "";

    private IUserAction action;
    private GUIStyle textStyle;
    private GUIStyle hintStyle;
    private GUIStyle prietStyle;
    private GUIStyle devilStyle;
    private GUIStyle btnStyle;

    public CharacterController characterCtrl;
    public static IState state = new IState(0, 0, 3, 3, false, null);
    public static IState endState = new IState(3, 3, 0, 0, true, null);
    public static int status;
    public static FirstController controller;

    // Use this for initialization
    void Start() {
      status = 0;
      action = Director.GetInstance().CurrentSecnController as IUserAction;
      controller = Director.GetInstance().CurrentSecnController as FirstController;
    }

    // Update is called once per frame
    void OnGUI() {
      textStyle = new GUIStyle {
        fontSize = 40,
        alignment = TextAnchor.MiddleCenter
      };
      hintStyle = new GUIStyle {
        fontSize = 15,
        fontStyle = FontStyle.Normal
      };
      prietStyle = new GUIStyle {
        fontSize = 15,
        fontStyle = FontStyle.Normal,
      };
      prietStyle.normal.textColor = new Color(0, 255, 0);
      devilStyle = new GUIStyle {
        fontSize = 15,
        fontStyle = FontStyle.Normal
      };
      devilStyle.normal.textColor = new Color(255, 0, 0);
      btnStyle = new GUIStyle("button") {
        fontSize = 30
      };
      GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 230, 100, 50),
          "Priest-And-Devil", textStyle);
      GUI.Label(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 185, 100, 50),
          "Cube: Periest", prietStyle);
      GUI.Label(new Rect(Screen.width / 2 + 20, Screen.height / 2 - 185, 100, 50),
          "Syphere: Devil", devilStyle);
      GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 210, 100, 50),
         hint, hintStyle);
      if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), "Guide", btnStyle)) {
        UnityEditor.EditorUtility.DisplayDialog("Guide", guide, "Got it");
      }
      if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 90, 100, 50), "Tips", btnStyle)) {
        //Debug.Log("StateRight: " + state.rightDevils + " " + state.rightPriests);
        //Debug.Log("StateLeft: " + state.leftDevils + " " + state.leftPriests);

        IState temp = IState.bfs(state, endState);
        //Debug.Log("NextRight: " + temp.rightDevils + " " + temp.rightPriests);
        //Debug.Log("NextLeft: " + temp.leftDevils + " " + temp.leftPriests);
        hint = "Hint:\n" + "Right:  Devils: " + temp.rightDevils + "   Priests: " + temp.rightPriests +
          "\nLeft:  Devils: " + temp.leftDevils + "   Priests: " + temp.leftPriests;
        //int priestsOffset = temp.leftPriests - state.leftPriests;
        //int devilsOffset = temp.leftDevils - state.leftDevils;
        //Debug.Log("offset: " + priestsOffset + " " + devilsOffset);
        //controller.AIMove(priestsOffset, devilsOffset);
      }
      if (status == 1) {
        // GameOver
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "GameOver!", textStyle);
        if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", btnStyle)) {
          status = 0;
          action.Restart();
        }
      } else if (status == 2) {
        // Win
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Win!", textStyle);
        if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", btnStyle)) {
          status = 0;
          action.Restart();
        }
      }
    }

    public void SetCharacterCtrl(CharacterController _characterCtrl) {
      characterCtrl = _characterCtrl;
    }

    void OnMouseDown() {
      if (status != 1) {
        if (gameObject.name == "boat") {
          action.MoveBoat();
          // update state
          int rightPriest = controller.rightCoastCtrl.GetCharacterNum()[0];
          int rightDevil = controller.rightCoastCtrl.GetCharacterNum()[1];
          int leftPriest = controller.leftCoastCtrl.GetCharacterNum()[0];
          int leftDevil = controller.leftCoastCtrl.GetCharacterNum()[1];
          bool location = controller.boatCtrl.boat.Location == Location.left ? true : false;
          int pcount = controller.boatCtrl.GetCharacterNum()[0];
          int dcount = controller.boatCtrl.GetCharacterNum()[1];
          if (location) {
            leftPriest += pcount;
            leftDevil += dcount;
          } else {
            rightPriest += pcount;
            rightDevil += dcount;
          }
          state = new IState(leftPriest, leftDevil, rightPriest, rightDevil, location , null);
        } else {
          action.CharacterClicked(characterCtrl);
        }
      }
    }
  }
}
