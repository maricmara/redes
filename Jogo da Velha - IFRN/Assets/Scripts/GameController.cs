using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Cell[] cells;
    public Text infoText;
    public Button restartButton;

    [Header("Tela de Vitória")]
    public GameObject winPanel;
    public Text winText;

    private string currentPlayer = "X";
    private string[] board = new string[9];
    public bool IsGameActive { get; private set; }

    public string CurrentPlayer => currentPlayer;

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        InitializeGame();
    }

    void InitializeGame()
    {
        currentPlayer = "X";
        IsGameActive = true;
        infoText.text = "Turno de " + currentPlayer;
        winPanel.SetActive(false);

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
            cells[i].ResetCell();
        }
    }

    // Recebe a célula que foi clicada e busca seu índice no array
    public void OnCellSelected(Cell cell)
    {
        int index = System.Array.IndexOf(cells, cell);

        if (index < 0)
        {
            Debug.LogError("Célula clicada não encontrada no array!");
            return;
        }

        Debug.Log($"Selecionada célula {index}");

        if (!IsGameActive)
        {
            Debug.LogWarning("Jogo não está ativo!");
            return;
        }

        if (!string.IsNullOrEmpty(board[index]))
        {
            Debug.LogWarning($"Célula {index} já preenchida!");
            return;
        }

        board[index] = currentPlayer;
        cells[index].SetText(currentPlayer);
        cells[index].ResetClicked();

        if (CheckWin(out int[] winningCombo))
        {
            HighlightWinningCells(winningCombo);
            ShowWinPanel(currentPlayer + " Venceu!");
            return;
        }

        if (CheckDraw())
        {
            ShowWinPanel("Empate!");
            return;
        }

        currentPlayer = currentPlayer == "X" ? "O" : "X";
        infoText.text = "Turno de " + currentPlayer;
    }

    bool CheckWin(out int[] comboWin)
    {
        int[][] combos = new int[][]
        {
            new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8},
            new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8},
            new[] {0,4,8}, new[] {2,4,6}
        };

        foreach (var combo in combos)
        {
            if (!string.IsNullOrEmpty(board[combo[0]]) &&
                board[combo[0]] == board[combo[1]] &&
                board[combo[1]] == board[combo[2]])
            {
                comboWin = combo;
                return true;
            }
        }

        comboWin = null;
        return false;
    }

    bool CheckDraw()
    {
        foreach (var cell in board)
            if (string.IsNullOrEmpty(cell))
                return false;

        return true;
    }

    void HighlightWinningCells(int[] combo)
    {
        foreach (int index in combo)
        {
            cells[index].Highlight();
        }
    }

    void ShowWinPanel(string message)
    {
        IsGameActive = false;
        winText.text = message;
        winPanel.SetActive(true);
        infoText.text = "Jogo Finalizado";
    }

    void RestartGame()
    {
        InitializeGame();
    }
}
