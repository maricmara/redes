using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public Cell[] cells;
    public Text statusText;

    private NetworkVariable<int> currentTurn = new NetworkVariable<int>(0); // 0 = X, 1 = O
    private NetworkList<int> boardState;

    void Awake()
    {
        Instance = this;
        boardState = new NetworkList<int>(new int[9]);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        boardState.OnListChanged += UpdateBoardUI;
        UpdateStatusText();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryMakeMoveServerRpc(int index, ServerRpcParams rpcParams = default)
    {
        if (boardState[index] != -1) return;

        ulong clientId = rpcParams.Receive.SenderClientId;
        if (clientId != NetworkManager.Singleton.ConnectedClientsList[currentTurn.Value].ClientId) return;

        boardState[index] = currentTurn.Value;
        currentTurn.Value = 1 - currentTurn.Value;
    }

    void UpdateBoardUI(NetworkListEvent<int> change)
    {
        for (int i = 0; i < boardState.Count; i++)
        {
            if (boardState[i] == 0) cells[i].SetMark("X");
            else if (boardState[i] == 1) cells[i].SetMark("O");
        }
        UpdateStatusText();
    }

    void UpdateStatusText()
    {
        string player = currentTurn.Value == 0 ? "X" : "O";
        statusText.text = $"Vez de: {player}";
    }
}
