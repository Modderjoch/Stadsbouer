using UnityEngine;
using UnityEditor;
using System.IO;

public class CardDataAssetPostprocessor : AssetPostprocessor
{
    // This method will be called whenever an asset is imported into the project
    void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // Loop through all imported assets
        foreach (string importedAsset in importedAssets)
        {
            // Check if the imported asset is a JSON file
            if (Path.GetExtension(importedAsset).Equals(".json"))
            {
                // Read the JSON file
                string json = File.ReadAllText(importedAsset);

                // Convert the JSON to a CardData object
                CardData cardData = JsonUtility.FromJson<CardData>(json);

                // Create a new ScriptableObject asset for the CardData object
                string assetPath = Path.ChangeExtension(importedAsset, ".asset");
                AssetDatabase.CreateAsset(cardData, assetPath);
            }
        }
    }
}
