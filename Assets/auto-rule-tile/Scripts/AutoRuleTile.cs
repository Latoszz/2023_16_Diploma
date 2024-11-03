using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

// ----------------------------------------------------------------------------
// Author: Alexandre Brull - Pandaroo
// https://pandaroo.be
// ----------------------------------------------------------------------------

namespace Pandaroo.autoruletile
{
    [ExecuteInEditMode]
    [CreateAssetMenu(fileName = "New Auto Rule Tile", menuName = "Tiles/Auto Rule Tile")]
    public class AutoRuleTile : ScriptableObject
    {
#if (UNITY_EDITOR)

    [SerializeField]
    Texture2D TileMap;
    [SerializeField]
    RuleTile RuleTileTemplate;
    RuleTile RuleTileTemplate_Default;
    [SerializeField]
    RuleTile OverrideExistingRuleTile;

    private void OnValidate()
    {
        // If there is a default template, load it when the asset is created.
        RuleTileTemplate_Default = Resources.Load("AutoRuleTile_default") as RuleTile;
        if (RuleTileTemplate_Default != null)
        {
            RuleTileTemplate = RuleTileTemplate_Default;
        }
    }

    private void ChangeAssetName() {
        string assetPath = AssetDatabase.GetAssetPath(this);
        AssetDatabase.RenameAsset(assetPath, "RuleTile_"+TileMap.name);
        AssetDatabase.Refresh();
    }
    public void OverrideRuleTile()
    {
        // Make a copy of the Rule Tile Template from a new asset.
        RuleTile _new = CreateInstance<RuleTile>();
        EditorUtility.CopySerialized(RuleTileTemplate, _new);

        // Get all the sprites in the Texture2D file (TileMap)
        string spriteSheet = AssetDatabase.GetAssetPath(TileMap);
        string spiteSheetName = TileMap.name;
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
            .OfType<Sprite>().ToArray();

        /*if (sprites.Length != RuleTileTemplate.m_TilingRules.Count)
        {
            Debug.LogWarning("The Tilemap doesn't have the same number of sprites than the Rule Tile template has rules.");
        }*/

        // Set all the sprites of the TileMap.
        for (int i = 0; i < RuleTileTemplate.m_TilingRules.Count; i++)
        {
            _new.m_TilingRules[i].m_Sprites[0] = sprites[i];
            _new.m_DefaultSprite = sprites[34];
        }

        // Replace this Asset with the new one.
        if (OverrideExistingRuleTile != null)
        {
            string name = OverrideExistingRuleTile.name;
            EditorUtility.CopySerialized(_new, OverrideExistingRuleTile);
            OverrideExistingRuleTile.name = name;
            DestroyImmediate(_new);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        }
        else {
            ChangeAssetName();
            AssetDatabase.CreateAsset(_new, AssetDatabase.GetAssetPath(this));
            
        }
    }


#endif
    }

}

