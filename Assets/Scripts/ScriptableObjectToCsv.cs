using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectToCsv : EditorWindow
{
    [MenuItem("Tools/Export CardData to CSV")]
    static void ExportCsv()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Select folder with CardData assets", "Assets", "");

        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        string csvFilePath = Path.Combine(folderPath, "card_data.csv");

        List<CardData> cardDataList = new List<CardData>();

        // Get all CardData assets in the folder
        string[] guids = AssetDatabase.FindAssets("t:CardData", new string[] { "Assets/Cards" });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);

            if (cardData != null)
            {
                cardDataList.Add(cardData);
            }
        }

        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            // Write header row
            writer.WriteLine("Card Name;Card ID;Purchase Value;Dice Value;Prestige Value;Card Type;Has Ability;Ability Amount;Ability Description;Card Image;Card Background;Dice Image;Building Model");

            // Write data rows
            foreach (CardData cardData in cardDataList)
            {
                string cardName = cardData.cardName;
                int cardID = cardData.cardID;
                int purchaseValue = cardData.purchaseValue;
                int diceValue = cardData.diceValue;
                int prestigeValue = cardData.prestigeValue;
                string cardType = cardData.cardType.ToString();
                bool hasAbility = cardData.cardAbility.hasAbility;
                int abilityAmount = cardData.cardAbility.abilityAmount;
                string abilityDescription = cardData.cardAbility.abilityDescription;
                string cardImagePath = AssetDatabase.GetAssetPath(cardData.cardImage);
                string cardBackgroundPath = AssetDatabase.GetAssetPath(cardData.cardBackground);
                string diceImagePath = AssetDatabase.GetAssetPath(cardData.diceImage);
                string buildingModelPath = AssetDatabase.GetAssetPath(cardData.buildingModel);

                // Write data row
                writer.WriteLine($"{cardName};{cardID};{purchaseValue};{diceValue};{prestigeValue};{cardType};{hasAbility};{abilityAmount};{abilityDescription};{cardImagePath};{cardBackgroundPath};{diceImagePath};{buildingModelPath}");
            }
        }

        AssetDatabase.Refresh();

        Debug.Log($"Exported {cardDataList.Count} CardData assets to CSV: {csvFilePath}");
    }
}
