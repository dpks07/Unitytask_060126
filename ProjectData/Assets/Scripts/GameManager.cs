
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Toggle tg1, tg2, tg3;
    [SerializeField] Button playBtn;

    [SerializeField] GameObject mainPanel, gamePanel;
    [SerializeField] GridManager gridManager;


    [Header("GameOver")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text turnTxt;

    [Header("BestScore")]
    [SerializeField] TMP_Text bestTurnTxt;
    [SerializeField] TMP_Text bestComboTxt;


    public Vector2Int gridSize { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        tg1.onValueChanged.AddListener((b) => { if (b) SetLevel(1); });
        tg2.onValueChanged.AddListener((b) => { if (b) SetLevel(2); });
        tg3.onValueChanged.AddListener((b) => { if (b) SetLevel(3); });

        playBtn.onClick.AddListener(PlayGame);
        SetHighScore();
    }

    void SetLevel(int level)
    {
        switch (level)
        {
            case 1:
                gridSize = new Vector2Int(2, 3);
                GameDataLoader.Instance.SetLevelValue(1);
                break;

            case 2:
                gridSize = new Vector2Int(3, 4);
                GameDataLoader.Instance.SetLevelValue(2);
                break;

            case 3:
                gridSize = new Vector2Int(4, 6);
                GameDataLoader.Instance.SetLevelValue(3);
                break;
        }
        //Debug.Log("Grid Size1 : " + gridSize);
    }

    public void SetToggleOnStart(int v)
    {
        switch (v)
        {
            case 1:
                tg1.isOn = true;
                gridSize = new Vector2Int(2, 3);
                break;

            case 2:
                tg2.isOn = true;
                gridSize = new Vector2Int(3, 4);
                break;

            case 3:
                tg3.isOn = true;
                gridSize = new Vector2Int(4, 6);
                break;
        }
        //Debug.Log("Grid Size2 : " + gridSize);
    }


    void PlayGame()
    {
        //StartGame

        mainPanel.SetActive(false);
        gamePanel.SetActive(true);

        gridManager.SetGrid(gridSize);
    }

    public void OpenGameOverPanel(int turns)
    {
        gameOverPanel.SetActive(true);
        turnTxt.text = $"Turns : {turns}";
    }

    public void BackToHomePage()
    {
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        gridManager.ResetOptions();
        gameOverPanel.SetActive(false);

        SetHighScore();
    }

    public void SetHighScore()
    {
        int bestTurn = GameDataLoader.Instance.GetBestTurn();
        int bestCombo = GameDataLoader.Instance.GetHighMatch();
        if (bestTurn > 0)
        {
            bestTurnTxt.text = bestTurn.ToString();
        }
        else
        {
            bestTurnTxt.text = "-";
        }

        if (bestCombo > 0)
        {
            bestComboTxt.text = bestCombo.ToString();
        }
        else
        {
            bestComboTxt.text = "-";
        }

    }
}
