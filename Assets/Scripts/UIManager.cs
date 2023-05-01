using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMP_InputField playerOneInput;
    [SerializeField] private TMP_InputField playerTwoInput;

    [HideInInspector] public string playerOneName;
    [HideInInspector] public string playerTwoName;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI playerOneNameText;
    [SerializeField] private TextMeshProUGUI playerTwoNameText;

    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    [SerializeField] private TextMeshProUGUI playerTwoScoreText;

    [SerializeField] private TextMeshProUGUI playerOneMoneyText;
    [SerializeField] private TextMeshProUGUI playerTwoMoneyText;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI diceText;

    [SerializeField] private TextMeshProUGUI playerOneDeckText;
    [SerializeField] private TextMeshProUGUI playerTwoDeckText;

    [SerializeField] private TextMeshProUGUI playerOneDiscardText;
    [SerializeField] private TextMeshProUGUI playerTwoDiscardText;

    [Header("Buttons")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject playButton;

    [Header("Animations")]
    [SerializeField] private AnimationClip fadeIn;
    [SerializeField] private AnimationClip fadeOut;

    public List<GameObject> activePlayer;
    private void Start()
    {
        if (DataManager.Instance.players.Count != 0)
        {
            playerOneNameText.text = DataManager.Instance.players[0];
            playerTwoNameText.text = DataManager.Instance.players[1];
        }
        else
        {
            Debug.Log("Player name is empty");
        }
    }

    public void StorePlayerNames()
    {
        playerOneName = playerOneInput.text;
        playerTwoName = playerTwoInput.text;

        DataManager.Instance.players.Add(playerOneName);
        DataManager.Instance.players.Add(playerTwoName);
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

    public void UpdateUI(string uiToUpdate, string textToAdd)
    {
        switch (uiToUpdate)
        {
            case "Timer":
                timerText.text = textToAdd;
                break;
            case "Dice":
                diceText.text = textToAdd;
                break;
            case "Score":
                switch (int.Parse(textToAdd))
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
                break;
            case "Deck":
                switch (int.Parse(textToAdd))
                {
                    case 0:
                        playerOneDeckText.text = gameManager.deckOne.Count.ToString();
                        break;
                    case 1:
                        playerTwoDeckText.text = gameManager.deckTwo.Count.ToString();
                        break;
                    default:
                        Debug.Log("No deck found");
                        break;
                }
                break;
            case "Discard":
                switch (int.Parse(textToAdd))
                {
                    case 0:
                        playerOneDiscardText.text = gameManager.discardOne.Count.ToString();
                        break;
                    case 1:
                        playerTwoDiscardText.text = gameManager.discardTwo.Count.ToString();
                        break;
                    default:
                        Debug.Log("No discard found");
                        break;
                }
                break;
            case "Money":
                switch (int.Parse(textToAdd))
                {
                    case 0:
                        playerOneMoneyText.text = DataManager.Instance.playerOneMoney.ToString();
                        break;
                    case 1:
                        playerTwoMoneyText.text = DataManager.Instance.playerTwoMoney.ToString();
                        break;
                }
                break;

        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PausePlayButton()
    {
        if (playButton.activeSelf) { pauseButton.SetActive(true); playButton.SetActive(false); }
        else { playButton.SetActive(true); pauseButton.SetActive(false); }
    }

    public IEnumerator PopUp(int seconds, GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }
}
