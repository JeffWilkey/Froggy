using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject homePrefab;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI livesText;
    public TMPro.TextMeshProUGUI gameOverText;
    private Player player;
    private Home[] homes;
    private int score;
    private int lives;
    private int time;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        homes = FindObjectsOfType<Home>(); 
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewLevel();
        ResetTimer();
    }

    public void Died()
    {
        SetLives(lives - 1);
        if (lives > 0)
        {
            Invoke(nameof(ResetTimer), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }
    
    public void HomeOccupied()
    {

        player.gameObject.SetActive(false);

        int bonusPoints = time * 20;
        SetScore(score + bonusPoints + 50);
        ResetTimer();
        player.Respawn();
    }

    public void GameOver()
    {
        player.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        
        StopAllCoroutines();
        StartCoroutine(CheckForPlayAgain());
    }

   private IEnumerator CheckForPlayAgain()
    {
        bool playAgain = false;

        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                playAgain = true;
            }

            yield return null;
        }

        NewGame();
        player.Respawn();
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "Lives: " + lives.ToString();
    }

    private IEnumerator Timer(int duration)
    {
        time = duration;
        timeText.text = time.ToString();

        while (time > 0)
        {
            yield return new WaitForSeconds(1);

            time--;
            timeText.text = time.ToString();
        }

        player.Death();
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].SetOccupied(false);
        }
        gameOverText.gameObject.SetActive(false);
        ResetTimer();
    }
}
