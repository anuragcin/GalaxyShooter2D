using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private List<Sprite> _liveSprite;

    [SerializeField]
    private Image _liveDisplayImage;

    [SerializeField]
    private Text _gameoverText;

    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;

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

}
