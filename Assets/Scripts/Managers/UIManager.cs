using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;

public class UIManager : Singleton<UIManager>
{
    
    [SerializeField] private TMP_Text scoreText;
    public TMP_Text highscoreText;
    [SerializeField] private TMP_Text enemyCountText;

    private void Start()
    {
        ResetUI();
        highscoreText.text = "Highscore: " + _SAVE.GetHighestScore().ToString();
    }

    private void ResetUI()
    {
        UpdateScore(0);
        UpdateEnemyCount(0);
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = "Score Count: " + _score;
        highscoreText.text = "Highscore: " + _SAVE.GetHighestScore().ToString();
    }

    public void UpdateEnemyCount(int _enemyCount)
    {
        /*
        if (_enemyCount >= 3)
            enemyCountText.color = Color.red;
        else
            enemyCountText.color = Color.white;
        */
        string textColor = _enemyCount >= 3 ? "<color=red>" : "<color=white>";
        //enemyCountText.color = _enemyCount >= 3 ? Color.red : Color.white;
        enemyCountText.text = "Enemy Count: " + textColor + _enemyCount +"</color>";
    }
}
