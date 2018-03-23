#pragma warning disable CS0618 // 类型或成员已过时

using UnityEngine;

public class WelcomePage : MonoBehaviour {

    public Texture2D img;

    private void OnGUI() {
        //position parameters
        float height = Screen.height * 0.5f;
        float width = Screen.width * 0.5f;
        int bHeight = 100;
        int bWidth = 150;
        int tHeight = 100;
        int tWidth = 200;

        //UI Style parameters
        GUIStyle tStyle = new GUIStyle {
            fontSize = 50,
            fontStyle = FontStyle.Bold,
        };
        GUIStyle bgStyle = new GUIStyle();
        bgStyle.normal.background = img;


        GUI.Label(new Rect(0, 0, 1024, 781), "", bgStyle);
        GUI.Label(new Rect(width - tWidth / 2 - 35, height - tHeight * 2, tWidth, tHeight), "Tic Tac Toe!", tStyle);

        if (GUI.Button(new Rect(width - bWidth / 2 - 100, height - bHeight / 2, bWidth, bHeight), "One Player Mode")) {
            Application.LoadLevel("OnePlayerMode");
        }

        if (GUI.Button(new Rect(width - bWidth / 2 + 100, height - bHeight / 2, bWidth, bHeight), "Two Player Mode")) {
            Application.LoadLevel("TwoPlayersMode");
        }
    }
}
