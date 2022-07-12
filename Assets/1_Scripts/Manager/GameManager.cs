using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;

    [Button]
    public void StartGame()
    {
        if (OnGameStart != null)
        {
            OnGameStart.Invoke();
        }
    }

    [Button]
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    [Button]
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
