using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class CsvToScriptableObject : MonoBehaviour
{
    private string _csvFilePath = "Assets/CSV/card_data.csv";
    [SerializeField] private CardData _cardDataTemplate;

    private void Start()
    {
        string[] csvLines = File.ReadAllLines(_csvFilePath);
        Debug.Log(csvLines);
        List<CardData> cardDatas = new List<CardData>();

        for (int i = 1; i < csvLines.Length; i++) // Skip header row
        {
            string[] csvValues = csvLines[i].Split(';');

            if (csvValues.Length < 9)
            {
                Debug.LogError($"Error on line {i}: Expected 9 fields but found {csvValues.Length}");
                continue;
            }

            string cardName = csvValues[0];

            // Check if asset with the same name already exists
            string assetPath = $"Assets/Cards/{cardName}.asset";
            CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);

            if (cardData == null)
            {
                // If asset doesn't exist, create new ScriptableObject
                cardData = Instantiate(_cardDataTemplate);
                string itemName = cardName.Replace(" ", "");
                cardData.name = itemName;
                AssetDatabase.CreateAsset(cardData, assetPath);
            }

            // Update ScriptableObject properties
            cardData.cardName = cardName;
            cardData.cardID = int.Parse(csvValues[1]);
            cardData.purchaseValue = int.Parse(csvValues[2]);
            cardData.diceValue = int.Parse(csvValues[3]);
            cardData.prestigeValue = int.Parse(csvValues[4]);

            switch (csvValues[5])
            {
                case "Gain":
                    cardData.cardType = CardData.CardType.Gain;
                    break;
                case "Take":
                    cardData.cardType = CardData.CardType.Take;
                    break;
                case "Profit":
                    cardData.cardType = CardData.CardType.Profit;
                    break;
            }

            if (csvValues[6] == "Yes")
            {
                cardData.cardAbility.hasAbility = true;
                cardData.cardAbility.abilityAmount = int.Parse(csvValues[7]);
                cardData.cardAbility.abilityDescription = csvValues[8];
            }

            // Load and assign sprites
            string spritesPath = "Assets/Graphics/Sprites/Cards/";
            string diceSpritesPath = "Assets/Graphics/Sprites/UI/";
            cardData.cardImage = AssetDatabase.LoadAssetAtPath<Sprite>(spritesPath + csvValues[9]);
            cardData.cardBackground = AssetDatabase.LoadAssetAtPath<Sprite>(spritesPath + csvValues[10]);
            cardData.diceImage = AssetDatabase.LoadAssetAtPath<Sprite>(diceSpritesPath + csvValues[11]);

            // Load and assign prefab
            string prefabPath = "Assets/Graphics/Models/";
            cardData.buildingModel = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + csvValues[12]);

            // Save changes to ScriptableObject asset
            EditorUtility.SetDirty(cardData);
        }

        // Save changes to all ScriptableObject assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Do something with the list of CardData objects
    }
}
