using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace {
    public class UserGUI : MonoBehaviour {
        private string guide = "Boat carries two people each time.\n" +
            "One person must drive the boat.\n" +
            "Click on person or boat to move them.\n" +
            "Priests killed when less than devils on one side.\n" +
            "Keep all priests alive! \n" +
            "Good luck!\n";

        private IUserAction action;
        private GUIStyle textStyle;
        private GUIStyle hintStyle;
        private GUIStyle btnStyle;

        public CharacterController characterCtrl;
        public static int status;

	    // Use this for initialization
	    void Start () {
            status = 0;
            action = Director.GetInstance().CurrentSecnController as IUserAction;
        }
	
	    // Update is called once per frame
	    void OnGUI () {
            textStyle = new GUIStyle {
                fontSize = 40,
                alignment = TextAnchor.MiddleCenter
            };
            hintStyle = new GUIStyle {
                fontSize = 15,
                fontStyle = FontStyle.Normal
            };
            btnStyle = new GUIStyle("button") {
                fontSize = 30
            };
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 230, 100, 50), 
                "Priest-And-Devil", textStyle);
            GUI.Label(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 185, 100, 50), 
                "Cube: Periest\tSyphere: Devil", hintStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), "Guide", btnStyle)) {
                UnityEditor.EditorUtility.DisplayDialog("Guide", guide, "Got it");
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
                } else {
                    action.CharacterClicked(characterCtrl);
                }
            }
        }
    }
}
