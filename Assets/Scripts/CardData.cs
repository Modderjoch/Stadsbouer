using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class CardData : ScriptableObject
{
    [Header("Images")]
    public Sprite cardImage;
    public Sprite cardBackground;
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
        public string abilityDescription;

        public void Ability(string abilityType, int amount)
        {
            switch (abilityType)
            {
                case "self":
                    if(DataManager.Instance.currentPlayerIndex == 0)
                    {
                        DataManager.Instance.playerOneMoney += 5;
                    }
                    else { DataManager.Instance.playerTwoMoney += 5; }
                    break;
                case "other":
                    if (DataManager.Instance.currentPlayerIndex == 0)
                    {
                        DataManager.Instance.playerTwoMoney += 5;
                    }
                    else { DataManager.Instance.playerOneMoney += 5; }
                    break;
            }
        }
    }
}
