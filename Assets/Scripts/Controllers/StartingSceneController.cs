using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulties { easy, hard };
public class StartingSceneController : MonoBehaviour
{
    public static StartingSceneController Instance;

    internal float volume;
    internal bool[] operationsSelected;
    internal Difficulties GameDifficulty;

    private void Awake()
    {
        operationsSelected = new bool[4];
        volume = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this); 
    }

    public void OnExitButtonClick()
    {
        AudioManager.Instance.OnButtonClick();
        Application.Quit();
    }
}
