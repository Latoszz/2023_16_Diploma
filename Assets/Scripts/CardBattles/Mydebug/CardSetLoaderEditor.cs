using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Collections.Generic;
using CardBattles.CardScripts.CardDatas;

[ExecuteInEditMode]
public class CardSetLoaderEditor : MonoBehaviour
{
    [SerializeField, ResizableTextArea] private string folderPath = ""; 
    [SerializeField, ResizableTextArea,ReadOnly]  private  string defaultPath = "Assets/Scripts/CardBattles/Scriptable objects/CardSets"; 
    [SerializeField, ReadOnly] public List<CardSetData> cardSetDatas = new List<CardSetData>();

    [Button]
    private void LoadFromGivenPath()
    {
        LoadCardSets(folderPath);
    }

    [Button]
    private void LoadFromDefaultPath()
    {
        string defaultPath = "Assets/Scripts/CardBattles/Scriptable objects/CardSets"; 
        LoadCardSets(defaultPath);
    }

    private void LoadCardSets(string path)
    {
        cardSetDatas.Clear();

        // Ensure path is valid
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Folder path is empty. Please provide a valid path.");
            return;
        }

        // Get all asset GUIDs in the folderPath and its subfolders
        string[] guids = AssetDatabase.FindAssets("t:CardSetData", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CardSetData cardSet = AssetDatabase.LoadAssetAtPath<CardSetData>(assetPath);

            if (cardSet != null) {
                cardSetDatas.Add(Instantiate(cardSet));
            }
        }

        Debug.Log($"Loaded {cardSetDatas.Count} CardSetData assets from path: {path}");
        EditorUtility.SetDirty(this);
    }
}