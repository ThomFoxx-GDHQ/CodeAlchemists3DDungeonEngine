using UnityEditor;
using UnityEngine;

public class ItemDeletionDetector : AssetModificationProcessor
{
    // This method is called when assets are about to be deleted
    private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
    {
        // Load the asset at the given path
        var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        // Check if the asset is an ItemData ScriptableObject
        if (asset is ItemSO itemData)
        {
            // Call the static method to remove the item from the list
            ItemManager.RemoveItem(itemData);
        }

        // Allow the deletion to proceed
        return AssetDeleteResult.DidNotDelete;
    }
}
