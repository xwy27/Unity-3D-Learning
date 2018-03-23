#pragma warning disable CS0618 // 类型或成员已过时

using UnityEngine;

public enum Player { player0, player1, player2};

public class TwoPlayer : MonoBehaviour {
    private bool playing = true; 
    private bool turn = true;
    private Player[,] symbol = new Player [3,3];
    public Texture2D img;

    // Use this for initialization
    void Start () {
        Reset();
    }

    private void OnGUI() {
        //position parameters
        int bHeight = 100;
        int bWidth = 100;
        float height = Screen.height * 0.5f - 150;
        float width = Screen.width * 0.5f - 150;

        //UI Style parameters
        GUIStyle tStyle = new GUIStyle {
            fontSize = 50,
            fontStyle = FontStyle.Bold
        };
        GUIStyle mStyle = new GUIStyle {
            fontSize = 25,
            fontStyle = FontStyle.Bold
        };
        mStyle.normal.textColor = Color.red;

        GUIStyle bgStyle = new GUIStyle();
        bgStyle.normal.background = img;
        GUI.Label(new Rect(0, 0, 1024, 781), "", bgStyle);



        //winner parameters
        Player winner = Check();
        string msg = "";

        //Back button
        if (GUI.Button(new Rect(width - bWidth * 2, height - bHeight * 1.5f, bWidth / 2, bHeight / 2), "<—")) {
            Application.LoadLevel("Welcome");
        }

        //Reset button
        if (GUI.Button(new Rect(width + bWidth / 2, height + 3.5f * bHeight, bWidth * 2, bHeight / 2), "Reset")) {
            Reset();
            return;
        }

        //Check if someone wins
        if (winner != Player.player0) {
            msg = (winner == Player.player1 ? "Player1(X) Wins!" : "Player2(O) Wins!");
            GUI.Label(new Rect(width + 50, height - 75, 100, 100), msg, mStyle);
            playing = !playing;
            GUI.enabled = false;
        }

        GUI.Label(new Rect(width + 20, height - 150, 100, 100), "Tic Tac Toe", tStyle);
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (symbol[i, j] == Player.player1) {
                    GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "X");
                } else if (symbol[i, j] == Player.player2) {
                    GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "O");
                } else {
                    if (GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "")) {
                        if (playing) {
                            symbol[i, j] = turn ? Player.player1 : Player.player2;
                            turn = !turn;
                        }
                    }
                }
            }
        }
        //Enable the reset button
        GUI.enabled = true;
    }

    private Player Check() {
        //Row check
        for (int i = 0; i < 3; ++i) {
            if (symbol[i, 0] != Player.player0 &&
                symbol[i, 0] == symbol[i, 1] &&
                symbol[i, 1] == symbol[i, 2]) {
                return symbol[i, 0];
            }
        }
        //Column check
        for (int j = 0; j < 3; ++j) {
            if (symbol[0, j] != Player.player0 &&
                symbol[0, j] == symbol[1, j] &&
                symbol[1, j] == symbol[2, j]) {
                return symbol[0, j];
            }
        }
        //Cross line check
        if (symbol[1, 1] != Player.player0) {
            if (symbol[1, 1] == symbol[0, 0] && symbol[1, 1] == symbol[2, 2] ||
                symbol[1, 1] == symbol[0, 2] && symbol[1, 1] == symbol[2, 0]) {
                return symbol[1, 1];
            }
        }
        return Player.player0;
    }

    // Reset the screen
    private void Reset() {
        playing = true;
        turn = true;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                symbol[i,j] = Player.player0;
            }
        }
        GUI.enabled = true;
    }
}
