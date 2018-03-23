#pragma warning disable CS0618 // 类型或成员已过时

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayer : MonoBehaviour {
    private bool playing = true;
    private bool turn = true;
    private Player[,] symbol = new Player[3, 3];
    private int a_x = -1;
    private int a_y = -1;

    // Use this for initialization
    void Start() {
        Reset();
    }

    private void OnGUI() {
        //position parameters
        int bHeight = 100;
        int bWidth = 100;
        float height = Screen.height * 0.5f - 150;
        float width = Screen.width * 0.5f - 150;

        //UI Sytle parameters
        GUIStyle tStyle = new GUIStyle {
            fontSize = 50,
            fontStyle = FontStyle.Bold
        };
        GUIStyle mStyle = new GUIStyle {
            fontSize = 25,
            fontStyle = FontStyle.Bold
        };
        mStyle.normal.textColor = Color.red;

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
            msg = (winner == Player.player1 ? "You Win!" : "Computer Wins!");
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
                    if (playing) {
                        if (turn) {
                            if (GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "")) {
                                symbol[i, j] = Player.player1;
                                turn = false;
                            }
                        } else {
                            GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "");

                            a_x = a_y = -1;
                            CanWin();
                            if (a_x == -1 && a_y == -1) {
                                Block();
                            }
                            if (a_x == -1 && a_y == -1) {
                                RandomStep();
                            }
                            if (a_x != -1 && a_y != -1 && symbol[a_x, a_y] == Player.player0) {
                                symbol[a_x, a_y] = Player.player2;
                            }
                            turn = true;
                        }
                    }
                }
            }
        }
        //Enable the reset button
        GUI.enabled = true;
    }

    private void CanWin() {
        //cross line to win
        if (symbol[1, 1] == Player.player2) {
            if (symbol[0, 0] == Player.player2 &&
                symbol[2, 2] == Player.player0) {
                a_x = 2;
                a_y = 2;
                return;
            }
            if (symbol[2, 2] == Player.player2 &&
                symbol[0, 0] == Player.player0) {
                a_x = 0;
                a_y = 0;
                return;
            }
            if (symbol[2, 0] == Player.player2 &&
                symbol[0, 2] == Player.player0) {
                a_x = 0;
                a_y = 2;
                return;
            }
            if (symbol[0, 2] == Player.player2 &&
                symbol[2, 0] == Player.player0) {
                a_x = 2;
                a_y = 0;
                return;
            }
        }

        for (int i = 0; i < 3; ++i) {
            int row = i;
            int col = i;
            //row to win
            if (symbol[row, 0] == Player.player2) {
                if (symbol[row, 1] == Player.player2 &&
                    symbol[row, 2] == Player.player0) {
                    a_x = row;
                    a_y = 2;
                    return;
                }
                if (symbol[row, 2] == Player.player2 &&
                    symbol[row, 1] == Player.player0) {
                    a_x = row;
                    a_y = 1;
                    return;
                }
            }
            if (symbol[row, 1] == Player.player2) {
                if (symbol[row, 2] == Player.player2 &&
                    symbol[row, 0] == Player.player0) {
                    a_x = row;
                    a_y = 0;
                    return;
                }
            }
            //column to win
            if (symbol[0, col] == Player.player2) {
                if (symbol[1, col] == Player.player2 &&
                    symbol[2, col] == Player.player0) {
                    a_x = 2;
                    a_y = col;
                    return;
                }
                if (symbol[2, col] == Player.player2 &&
                    symbol[1, col] == Player.player0) {
                    a_x = 1;
                    a_y = col;
                    return;
                }
            }
            if (symbol[1, col] == Player.player2) {
                if (symbol[2, col] == Player.player2 &&
                    symbol[0, col] == Player.player0) {
                    a_x = 0;
                    a_y = col;
                    return;
                }
            }
        }
    }

    private void Block() {
        //cross line to win
        if (symbol[1, 1] == Player.player1) {
            if (symbol[0, 0] == Player.player1 &&
                symbol[2, 2] == Player.player0) {
                a_x = 2;
                a_y = 2;
                return;
            }
            if (symbol[2, 2] == Player.player1 &&
                symbol[0, 0] == Player.player0) {
                a_x = 0;
                a_y = 0;
                return;
            }
            if (symbol[2, 0] == Player.player1 &&
                symbol[0, 2] == Player.player0) {
                a_x = 0;
                a_y = 2;
                return;
            }
            if (symbol[0, 2] == Player.player1 &&
                symbol[2, 0] == Player.player0) {
                a_x = 2;
                a_y = 0;
                return;
            }
        }
        if (symbol[2, 2] == Player.player1) {
            if (symbol[0, 0] == Player.player1 &&
                symbol[1, 1] == Player.player0) {
                a_x = 1;
                a_y = 1;
                return;
            }
        }
        if (symbol[2, 0] == Player.player1) {
            if (symbol[0, 2] == Player.player1 &&
                symbol[1, 1] == Player.player0) {
                a_x = 1;
                a_y = 1;
                return;
            }
        }

        for (int i = 0; i < 3; ++i) {
            int row = i;
            int col = i;
            //row to win
            if (symbol[row, 0] == Player.player1) {
                if (symbol[row, 1] == Player.player1 &&
                    symbol[row, 2] == Player.player0) {
                    a_x = row;
                    a_y = 2;
                    return;
                }
                if (symbol[row, 2] == Player.player1 &&
                    symbol[row, 1] == Player.player0) {
                    a_x = row;
                    a_y = 1;
                    return;
                }
            }
            if (symbol[row, 1] == Player.player1) {
                if (symbol[row, 2] == Player.player1 &&
                    symbol[row, 0] == Player.player0) {
                    a_x = row;
                    a_y = 0;
                    return;
                }
            }
            //column to win
            if (symbol[0, col] == Player.player1) {
                if (symbol[1, col] == Player.player1 &&
                    symbol[2, col] == Player.player0) {
                    a_x = 2;
                    a_y = col;
                    return;
                }
                if (symbol[2, col] == Player.player1 &&
                    symbol[1, col] == Player.player0) {
                    a_x = 1;
                    a_y = col;
                    return;
                }
            }
            if (symbol[1, col] == Player.player1) {
                if (symbol[2, col] == Player.player1 &&
                    symbol[0, col] == Player.player0) {
                    a_x = 0;
                    a_y = col;
                    return;
                }
            }
        }
    }

    private void RandomStep() {
        List<int> row = new List<int>();
        List<int> col = new List<int>();
        int count = 0;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (symbol[i,j] == Player.player0) {
                    row.Add(i);
                    col.Add(j);
                    count++;
                }
            }
        }
        if (count != 0) {
            System.Random ran = new System.Random();
            int index = ran.Next(0, count);
            a_x = row[index];
            a_y = col[index];

        } else {
            a_x = a_y = -1;
        }
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
                symbol[i, j] = Player.player0;
            }
        }
        GUI.enabled = true;
    }
}
