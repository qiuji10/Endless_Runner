using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameEnd;

    [Header("Score")]
    public float score;
    public float bonusAdd;
    public int coins;
    public bool isPlaying;

    [Header("In-Game Text UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinsText;

    [Header("End-Game Text UI")]
    [SerializeField] private TMP_Text endScoreText;
    [SerializeField] private TMP_Text endCoinsText;
    [SerializeField] private TMP_Text totalMoneyText;
    [SerializeField] private GameObject newHighscoreObj;

    [Header("Parent UI")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject endgameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject PowerUpButton;

    [Header("Sound Effect")]
    public AudioData sfx, bgm;

    private Database data;

    public GameObject PupButton { get => PowerUpButton; set => PowerUpButton = value; }
    public Database Data { get => data; set => data = value; }

    private void Start()
    {
        data = GetComponent<Database>();
        AudioManager.instance.PlayBGM(bgm, "op_bgm");
    }

    private void Update()
    {
        if (isPlaying)
        {
            score = score + bonusAdd + Time.deltaTime;
            scoreText.text = ((int)(score * 100)).ToString();
        }
    }

    [Button]
    public void StartGame()
    {
        if (OnGameStart != null)
        {
            OnGameStart.Invoke();
        }

        menuUI.SetActive(false);
        gameplayUI.SetActive(true);

        StartCoroutine(ShowPUButton());
    }

    public void UpdateCoin()
    {
        AudioManager.instance.PlaySFX(sfx, "collect");
        coins++;
        coinsText.text = coins.ToString();
    }

    [Button]
    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    [Button]
    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    [Button]
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        newHighscoreObj.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void ShowShop()
    {
        shopUI.SetActive(true);
        menuUI.SetActive(false);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void Lose()
    {
        AudioManager.instance.StopBGM();
        if (OnGameEnd != null)
        {
            OnGameEnd.Invoke();
        }

        if ((int)(score * 100) > data.playerData.highscore)
        {
            newHighscoreObj.SetActive(true);
            data.playerData.highscore = (int)(score * 100);
        }

        data.playerData.money += coins;

        endScoreText.text = scoreText.text;
        endCoinsText.text = coins.ToString();
        totalMoneyText.text = data.playerData.money.ToString();

        data.SaveGame();
        data.Assigner();
        gameplayUI.SetActive(false);
        endgameUI.SetActive(true);
    }

    private IEnumerator ShowPUButton()
    {
        PowerUpButton.SetActive(true);
        if (data.playerData.powerupJump <= 0)
        {
            PowerUpButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            PowerUpButton.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (data.playerData.powerupBonus <= 0)
        {
            PowerUpButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            PowerUpButton.transform.GetChild(1).gameObject.SetActive(true);
        }

        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX(sfx, "sigh");
        yield return new WaitForSeconds(4);
        isPlaying = true;
        AudioManager.instance.PlayBGM(bgm, "gameplay_bgm");
        yield return new WaitForSeconds(5);
        PowerUpButton.SetActive(false);
    }
}
