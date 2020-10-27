using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [Header("Text Objects")]
    [SerializeField] Text _currentScoreTextView1;
    [SerializeField] Text _currentScoreTextView2;
    [SerializeField] Text _highScoreTextView;
    [SerializeField] Text _bestTimeTextView;
    [SerializeField] Text _currentTimeTextView1;
    [SerializeField] Text _currentTimeTextView2;

    [Header("Panels")]
    [SerializeField] GameObject popupPanel;
    [SerializeField] GameObject winPanel;

    public int _currentScore;
    public bool _levelPaused = false;
    public bool _winState = false;

    float _time = 0f;

    private void Awake()
    {
        ResumeGame();
    }

    void Update()
    {
        //Debug.Log(PlayerController._playerHealth);
        if (Input.GetKeyDown(KeyCode.Escape) && _levelPaused == false && _winState == false && Time.timeScale != 0)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _levelPaused == true && PlayerController._playerHealth > 0 && _winState == false && Time.timeScale != 0)
        {
            ResumeGame();
        }
        if (_winState == true)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Debug.Log("Did");
                RestartLevel();
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                ExitLevel();
            }
        }
        _time += Time.deltaTime;
        _currentTimeTextView1.text = "Time: " + _time.ToString("F2");
    }

    public void ExitLevel()
    {
        if (PlayerController._playerHealth <= 0)
        {
            Time.timeScale = 1;
            PlayerController._playerHealth = 3f;
        }
        _winState = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void IncreaseScore(int scoreIncrease)
    {
        _currentScore += scoreIncrease;
        _currentScoreTextView1.text = "Score: " + _currentScore.ToString();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        popupPanel.SetActive(true);
        _levelPaused = true;
    }

    public void ResumeGame()
    {
        if (PlayerController._playerHealth > 0)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            popupPanel.SetActive(false);
            _levelPaused = false;
        }
    }

    public void RestartLevel()
    {
        PlayerController._playerHealth = 3f;
        ResumeGame();
        _winState = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (_currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", _currentScore);
            Debug.Log("New high score: " + _currentScore);
        }
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        if (_time < bestTime || bestTime == 0f)
        {
            PlayerPrefs.SetFloat("BestTime", _time);
        }
        _currentScoreTextView1.gameObject.SetActive(false);
        _currentTimeTextView1.gameObject.SetActive(false);
        winPanel.SetActive(true);
        _winState = true;
        ApplyChangeToText();
    }

    public static void LoseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // losePanel.SetActive(true);
        GameObject.Find("Canvas").transform.Find("YouDied_pnl").gameObject.SetActive(true);
    }


    public void ApplyChangeToText()
    {
        _currentScoreTextView2.text = "Your Score: " + _currentScore.ToString();
        _currentTimeTextView2.text = "Your time: " + _time.ToString("f2");
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = "High Score: " + highScore.ToString();
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        _bestTimeTextView.text = "Best Time: " + bestTime.ToString("F2");
    }
}
