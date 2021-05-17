using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoCount;

    [SerializeField]
    private List<Sprite> _liveSprite;

    [SerializeField]
    private Image _liveDisplayImage;

    [SerializeField]
    private Text _gameoverText;

    private int _totalAmmoCount = 100;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Slider _slider;
  
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCount.text = "Total Ammo:" + _totalAmmoCount; 

        _liveDisplayImage.sprite = _liveSprite[3];
        _gameoverText.gameObject.SetActive(false);

        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager cannot be null");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) 
    {
        _liveDisplayImage.sprite = _liveSprite[currentLives];

        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameoverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        _gameManager.GameOver();

        StartCoroutine(FlashingGameOverRoutine());
    }

    IEnumerator FlashingGameOverRoutine()
    {
        while (true)
        {
            _gameoverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameoverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    /// <summary>
    /// Total Ammo Count
    /// </summary>
    /// <returns></returns>
    public int TotalAmmoCount()
    {
        return _totalAmmoCount;
    }

    /// <summary>
    /// Update the Ammo Count
    /// </summary>
    /// <param name="ammoCount"></param>
    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCount.text = "Total Ammo:" + ammoCount;
        if (ammoCount==0)
        {
            StartCoroutine(FlashingTotalAmmoCount());
        }
    }

    /// <summary>
    /// Flashing Ammo Count when Count =0
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashingTotalAmmoCount()
    {
        while (true)
        {
            _ammoCount.text = "Total Ammo: 0";
            yield return new WaitForSeconds(0.5f);
            _ammoCount.text = "";
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void UpdateThrusterScale(float elaspedTime, float thrusterScale)
    {
        if (_slider.maxValue != thrusterScale)
        {
            _slider.maxValue = thrusterScale;
        }

        thrusterScale = thrusterScale - elaspedTime;
        _slider.value = thrusterScale;

        if (thrusterScale > 9.0f && thrusterScale <= 10.0f)
        {
            _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }
        else if (thrusterScale > 7.0f && thrusterScale <= 9.0f)
        {
            _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
        else if (thrusterScale > 5.0f && thrusterScale <= 7.0f)
        {
            _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.cyan;
        }
        else if (thrusterScale > 3.0f && thrusterScale <= 5.0f)
        {
            _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.blue;
        }
        else if (thrusterScale <= 3.0f )
        {
            _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
    }

}
