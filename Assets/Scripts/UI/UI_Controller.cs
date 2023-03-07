using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller Instance;

    #region Serialized Fields
    [SerializeField] GameObject popupPanel;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI[] answers;
    [Space(10)]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI incrementText;

    [Space(10)]
    [SerializeField] GameObject foodImage;
    [SerializeField] Transform takeFoodPanel;

    [Space(10)]
    [SerializeField] Slider volumeSlider;

    [Header("Loading Scene")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBar;
    [SerializeField] float waitTime;

    [Header("Panels")]
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject shopPanel;

    #endregion

    public bool hasAnswered;
    CustomerController currentCustomer;
    Question randomQuestion;
    List<GameObject> takenFoodList;
    Color currentColor;
    AudioSource gameMusic;

    #region Initializations
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
        incrementText.gameObject.SetActive(false);
    }
    private void Start()
    {
        hasAnswered = false;
        takenFoodList = new List<GameObject>();
        currentColor = incrementText.fontMaterial.GetColor("_FaceColor");
        moneyText.text = $"Coin : {PlayerController.Instance.currentMoney}";
        volumeSlider.value = StartingSceneController.Instance.volume;
    }

    #endregion

    public void OnClickOnOrder(CustomerController _currentCustomer)
    {
        SetQuestion();
        popupPanel.SetActive(true);
        currentCustomer = _currentCustomer;
        PlayerController.Instance.canMove = false;
    }

    public void OnVolumeChange()
    {
        AudioManager.Instance.gameMusic.volume = volumeSlider.value;
    }

    private void SetQuestion()
    {
        randomQuestion = QuestionController.Instance.CreateRandomQuestion();
        questionText.text = randomQuestion.question;
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].text = randomQuestion.answers[i];
        }
    }


    IEnumerator LoadSceneAsync()
    {
        loadingPanel.SetActive(true);

        float startTime = Time.time;
        while (Time.time - startTime < waitTime)
        {
            loadingBar.fillAmount = (Time.time - startTime) / waitTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync("Starting Scene");
    }

public void OnAnswerClick(int i)
    {
        if (hasAnswered)
            return;
        if(i == randomQuestion.rightAnswerIndex)
        {
            AudioManager.Instance.PlayWinSound();
            answers[i].transform.parent.GetComponent<Image>().color = Color.green;
            StartCoroutine(WaitandClose());
            StartCoroutine(currentCustomer.WaitforOrder());
        }
        else
        {
            AudioManager.Instance.PlayLoseSound();
            answers[i].transform.parent.GetComponent<Image>().color = Color.red;
            StartCoroutine(WaitandClose());
            StartCoroutine(currentCustomer.Leave(4));
        }

        StartCoroutine(WaitandReset(i));
        hasAnswered = true;
    }

    IEnumerator WaitandClose()
    {
        yield return new WaitForSeconds(1.5f);
        popupPanel.SetActive(false);
        PlayerController.Instance.canMove = true;
        currentCustomer.TurnOffOrder();
    }

    IEnumerator WaitandReset(int i)
    {
        yield return new WaitForSeconds(1.51f);
        answers[i].transform.parent.GetComponent<Image>().color = Color.white;
        hasAnswered = false;
    }

    public void CreateFoodImage(Sprite _foodSprite, int _id)
    {
        GameObject spawnedImage = Instantiate(foodImage, takeFoodPanel);
        spawnedImage.GetComponent<Image>().sprite = _foodSprite;
        spawnedImage.GetComponent<ID>().id = _id;
        takenFoodList.Add(spawnedImage);
    }

    public void RemoveFromList(int _removeID)
    {
        for(int i = 0; i < takenFoodList.Count; i++)
        {
            if(takenFoodList[i].GetComponent<ID>().id == _removeID)
            {
                GameObject removingImage = takenFoodList[i];
                takenFoodList.RemoveAt(i);
                Destroy(removingImage);
            }
        }
    }

    public IEnumerator EarnMoney(float _currentMoney, float _increment)
    {
        moneyText.text = $"Coin : {_currentMoney + _increment}";
        incrementText.gameObject.SetActive(true);
        incrementText.text = $"+{_increment}";
        StartCoroutine(FadeCoroutine(3));
        yield return new WaitForSeconds(3);
        incrementText.gameObject.SetActive(false);
    }

    IEnumerator FadeCoroutine(float fadeTime)
    {
        float waitTime = 0;
        while (waitTime < 1)
        {
            incrementText.fontMaterial.SetColor("_FaceColor", Color.Lerp(currentColor, Color.clear, waitTime));
            yield return null;
            waitTime += Time.deltaTime / fadeTime;

        }

    }

#region On Button clicks
    public void OnExitButtonClick()
    {
        StartingSceneController.Instance.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("gameVolume", StartingSceneController.Instance.volume);
        PlayerController.Instance.canMove = false;
        StartCoroutine(LoadSceneAsync());
    }

    public void OnSettingsButtonClick()
    {
        settingsPanel.SetActive(true);
        PlayerController.Instance.canMove = false;
    }

    public void OnContinueButtonClick()
    {
        settingsPanel.SetActive(false);
        PlayerController.Instance.canMove = true;
    }
    public void OnShopButtonClick()
    {
        shopPanel.SetActive(true);
        PlayerController.Instance.canMove = false;
    }

    public void OnCloseButtonClick()
    {
        shopPanel.SetActive(false);
        PlayerController.Instance.canMove = true;
    }
#endregion
}
