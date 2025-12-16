using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _livesImages;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private GameObject _gameOverGO;
    [SerializeField]
    private Text _restartText;

    // Start is called before the first frame update
    void Start()
    {
        _text.text = "Score: " + 0;
        _gameOverGO.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    public void UpdateScore(int points)
    {
        _text.text = "Score: " + points;
    }

    public void UpdateLeives(int currentLives)
    {
        if (currentLives > 0 && currentLives < _livesSprites.Length)
        { 
            _livesImages.sprite = _livesSprites[currentLives];
        }
    }

    public void TurnGameOverScreen()
    {
        StartCoroutine(TextFlickering());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator TextFlickering()
    {
        while (true)
        {
            _gameOverGO.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverGO.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
