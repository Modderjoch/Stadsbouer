using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class CardData : ScriptableObject
{
    [Header("Images")]
    public Sprite cardImage;
    public Sprite cardBackground;
    public Sprite diceImage;
    [Header("Data")]
    public string cardName;
    public int cardID;
    [Header("Values")]
    public int purchaseValue;
    public int diceValue;
    public int prestigeValue;
    [Header("Abilities")]
    public CardType cardType;
    public CardAbility cardAbility;
    public GameObject buildingModel;

    [HideInInspector] public int deckPos;
    [HideInInspector] public int player;
   

    // Custom enum for card type
    public enum CardType
    {
        Gain,
        Take,
        Profit
    }

    // Custom class for card ability
    [System.Serializable]
    public class CardAbility
    {
        public bool hasAbility;
        public int abilityAmount;
        public string abilityDescription;

        public void Ability(string abilityType, int amount)
        {
            switch (abilityType)
            {
                case "self":
                    if(DataManager.Instance.currentPlayerIndex == 0)
                    {
                        DataManager.Instance.playerOneMoney += amount;
                    }
                    else { DataManager.Instance.playerTwoMoney += amount; }
                    break;
                case "other":
                    if (DataManager.Instance.currentPlayerIndex == 0)
                    {
                        DataManager.Instance.playerTwoMoney += amount;
                    }
                    else { DataManager.Instance.playerOneMoney += amount; }
                    break;
            }
        }
    }
}
