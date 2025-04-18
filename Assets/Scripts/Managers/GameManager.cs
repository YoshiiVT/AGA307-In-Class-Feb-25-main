using UnityEngine;

public enum GameState
{
    Start,
    Playing,
    Paused,
    GameOver
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    HurtMeHarder
}
public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private GameState gameState;
    public GameState GameState => gameState;
    [SerializeField]
    private Difficulty difficulty;
    [SerializeField]
    private int score;
    private int highscore;

    private void Start()
    {
        highscore = _SAVE.GetHighestScore();
    }
    public void AddScore(int _score)
    {
        score += _score;
        _UI.UpdateScore(score);
        _SAVE.SetScore(score);
    }
}
