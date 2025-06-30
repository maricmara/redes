using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;
    private Image buttonImage;
    private GameController gameController;

    private bool clicked = false;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
        gameController = FindObjectOfType<GameController>();
        button.interactable = true; 
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (clicked) return;

        Debug.Log($"Célula clicada: '{buttonText.text}'");

        if (buttonText.text == "" && gameController.IsGameActive)
        {
            clicked = true;
            gameController.OnCellSelected(this);  // passa a si mesma!
        }
    }

    public void ResetClicked()
    {
        clicked = false;
    }

    public void SetText(string value)
    {
        buttonText.text = value;
        button.interactable = false;

        // Cor diferente para X e O
        if (value == "X")
            buttonImage.color = new Color(0.8f, 0.3f, 0.3f); // vermelho claro
        else if (value == "O")
            buttonImage.color = new Color(0.3f, 0.3f, 0.8f); // azul claro
        else
            buttonImage.color = Color.white; // padrão branco caso outro valor
    }

    public void ResetCell()
    {
        buttonText.text = "";
        buttonText.color = Color.black;
        button.interactable = true;
        buttonImage.color = Color.white;
        clicked = false;
    }

    public void Highlight()
    {
        buttonImage.color = new Color(0.3f, 1f, 0.3f);
        buttonText.color = Color.black;
    }
}
