using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Cell : NetworkBehaviour
{
    public int index;
    private Button button;
    private Text buttonText;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!IsOwner) return;
        GameManager.Instance.TryMakeMoveServerRpc(index);
    }

    public void SetMark(string mark)
    {
        buttonText.text = mark;
        button.interactable = false;
    }
}