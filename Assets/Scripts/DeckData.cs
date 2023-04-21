using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Custom/Deck")]
public class DeckData : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();

    // Add a card to the deck
    public void AddCard(CardData card)
    {
        cards.Add(card);
    }

    // Remove a card from the deck
    public void RemoveCard(CardData card)
    {
        cards.Remove(card);
    }

    // Shuffle the deck
    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardData temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    // Draw a card from the deck
    public CardData DrawCard()
    {
        if (cards.Count > 0)
        {
            CardData drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }
        else
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }
    }

    // Check if the deck is empty
    public bool IsEmpty()
    {
        return cards.Count == 0;
    }
}
