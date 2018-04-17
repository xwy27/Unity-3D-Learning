/**************************************************
 *  Author      ：xwy (xuwy27@mail2.sysu.edu.cn)
 *  Time        ：2018/4/17 9:35:02
 *  Filename    ：GameModel
 *  Version     : V1.0.1  
 *  Description : Score Model
 **************************************************/

public class GameModel : IScore {

    public int Score { get; private set; }
    public int Round { get; private set; }
    private static GameModel gameModel;

    private GameModel() {
        Round = 1;
    }

    public static GameModel GetGameModel() {
        return gameModel ?? (gameModel = new GameModel());
    }

    public void AddScore() {
        Score += 10;
        if (CheckUpdate()) {
            Round++;
            FirstSceneControllerBase.GetFirstSceneControllerBase().Update();
        }
    }

    public void SubScore() {
        Score -= 10;
        if (Score < 0) {
            FirstSceneControllerBase.GetFirstSceneControllerBase().SetGameStatus(GameStatus.Lose);
        }
    }

    public bool CheckUpdate() {
        return Score >= Round * 20;
    }

    public int GetScore() {
        return GameModel.GetGameModel().Score;
    }
}