using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public static AudioClip _startingSong;
    [SerializeField] Text _highScoreTextView;
    [SerializeField] Text _bestTimeTextView;
    [SerializeField] public ParticleSystem _backgroundParticles;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        _bestTimeTextView.text = "Best Time: " + bestTime.ToString("F2");
        _backgroundParticles.Emit(100);
    }

    public void ResetHighScore(int resetScore)
    {
        PlayerPrefs.SetInt("HighScore", resetScore);
        PlayerPrefs.SetFloat("BestTime", 0f);
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        _bestTimeTextView.text = "Best Time: " + bestTime.ToString("F2");
        Debug.Log("New high score: " + highScore);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
