using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartingSceneUIController : MonoBehaviour
{
    public static StartingSceneUIController Instance;

    [SerializeField] internal Slider slider;
    [SerializeField] internal Toggle[] toggles;
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject startButton;
    [SerializeField] float waitTime;
    float progress;
    int toggleCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        slider.value = StartingSceneController.Instance.volume;
        toggleCount = 0;
    }

    public void OnStartButtonClick()
    {
        StartingSceneController.Instance.volume = slider.value;
        PlayerPrefs.SetFloat("gameVolume", StartingSceneController.Instance.volume);

        for (int i = 0; i < 4; i++)
            if (toggles[i].isOn)
                StartingSceneController.Instance.operationsSelected[i] = true;
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        loadingPanel.SetActive(true);

        float startTime = Time.time;
        while(Time.time - startTime < waitTime)
        {
            loadingBar.fillAmount = (Time.time - startTime) / waitTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync("Restaurant Scene");
    }

    public void OnVolumeChange()
    {
        AudioManager.Instance.gameMusic.volume = slider.value;
    }

    public void OnToggleClick()
    {
        AudioManager.Instance.OnButtonClick();
        foreach(Toggle toggle in toggles)
        {
            if (toggle.isOn)
                toggleCount++;
        }
        if (toggleCount > 0)
            startButton.SetActive(true);
        else
            startButton.SetActive(false);
        toggleCount = 0;
    }
}
