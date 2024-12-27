using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance
    public TileBoard board;
    public CanvasGroup gameOver;
    [SerializeField] GameObject newGameButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        newGameButton.SetActive(true);
        hiscoreText.text = LoadHighScore().ToString();
        gameOver.alpha = 0;
        gameOver.interactable = false;
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        newGameButton.SetActive(false);
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = .5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        int hiscore = LoadHighScore();

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }
}
