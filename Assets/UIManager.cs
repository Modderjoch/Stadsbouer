using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerOneInput;
    [SerializeField] private TMP_InputField playerTwoInput;

    [HideInInspector] public string playerOneName;
    [HideInInspector] public string playerTwoName;

    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    [SerializeField] private TextMeshProUGUI playerTwoScoreText;

    [SerializeField] private TextMeshProUGUI timerText;

    public List<GameObject> activePlayer;

    public void StorePlayerNames()
    {
        playerOneName = playerOneInput.text;
        playerTwoName = playerTwoInput.text;

        DataManager.Instance.players.Add(playerOneName);
        DataManager.Instance.players.Add(playerTwoName);

        Debug.Log("Player names are: " + playerOneName + " " + playerTwoName + ".");
    }

    private void Start()
    {
        if(DataManager.Instance.players.Count != 0)
        {
            GameObject playerOne = GameObject.Find("PlayerOne");
            GameObject playerTwo = GameObject.Find("PlayerTwo");

            playerOne.GetComponent<TextMeshProUGUI>().text = DataManager.Instance.players[0];
            playerTwo.GetComponent<TextMeshProUGUI>().text = DataManager.Instance.players[1];
        }
        else
        {
            Debug.Log("Player name is empty");
        }
    }

    public void ActivePlayerUI()
    {
        if(DataManager.Instance.currentPlayerIndex == 0)
        {
            activePlayer[0].SetActive(true);
            activePlayer[1].SetActive(false);
        }
        else
        {
            activePlayer[1].SetActive(true);
            activePlayer[0].SetActive(false);
        }
    }

    public void UpdateScoreUI(int currentPlayer)
    {
        switch (currentPlayer)
        {
            case 0:
                playerOneScoreText.text = DataManager.Instance.playerOneScore.ToString();
                break;
            case 1:
                playerTwoScoreText.text = DataManager.Instance.playerTwoScore.ToString();
                break;
            default:
                Debug.Log("No active player/score attribute found.");
                break;
        }
    }

    public void UpdateTimerUI(string timer)
    {
        timerText.text = timer;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
