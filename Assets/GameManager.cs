using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        if (DataManager.Instance.players != null)
        {
            DataManager.Instance.currentPlayerIndex = Random.Range(0, 2);
            int i = DataManager.Instance.currentPlayerIndex;
            GameObject.Find("StartingPlayer").GetComponent<TextMeshProUGUI>().text = "The starting player is: " + DataManager.Instance.players[i];

            if(uiManager != null) { uiManager.ActivePlayerUI(); }
        }
    }

    public void NextPlayer()
    {
        if(DataManager.Instance.currentPlayerIndex >= DataManager.Instance.players.Count - 1)
        {
            DataManager.Instance.currentPlayerIndex--;
        }
        else
        {
            DataManager.Instance.currentPlayerIndex++;
        }

        if (uiManager != null) { uiManager.ActivePlayerUI(); }
        Debug.Log("Current active player is: " + DataManager.Instance.players[DataManager.Instance.currentPlayerIndex]);
    }
}
