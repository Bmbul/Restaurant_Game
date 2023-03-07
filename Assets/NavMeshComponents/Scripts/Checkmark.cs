using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkmark : MonoBehaviour
{
    [SerializeField] GameObject hard;
    [SerializeField] GameObject easy;

    public void EasyButtonClicked()
    {
        AudioManager.Instance.OnButtonClick();
        hard.SetActive(false);
        easy.SetActive(true);
        StartingSceneController.Instance.GameDifficulty = Difficulties.easy;
    }

    public void HardButtonClicked()
    {
        AudioManager.Instance.OnButtonClick();
        easy.SetActive(false);
        hard.SetActive(true);
        StartingSceneController.Instance.GameDifficulty = Difficulties.hard;
    }



}