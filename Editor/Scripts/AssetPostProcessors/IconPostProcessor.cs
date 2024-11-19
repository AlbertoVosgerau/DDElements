using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DandyDino.Elements
{
    public class IconPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool shouldRefresh = false;
            IconsDatabase iconsDatabase = null;
            
            for (int i = 0; i < importedAssets.Length; i++)
            {
                string importedAsset = importedAssets[i];
                if (!importedAsset.EndsWith(".png"))
                {
                    if (importedAsset.EndsWith(".asset"))
                    {
                        string importedDirectory = DDElements.Assets.GetAssetDirectory(importedAsset);
                        iconsDatabase = DDElements.Assets.GetAssetsInDirectory<IconsDatabase>(importedDirectory).FirstOrDefault();
                        iconsDatabase.Refresh();
                    }
                    return;
                }
                string directory = DDElements.Assets.GetAssetDirectory(importedAsset);
                
                iconsDatabase = DDElements.Assets.GetAssetsInDirectory<IconsDatabase>(directory).FirstOrDefault();
                if (iconsDatabase == null)
                {
                    return;
                }
                
                iconsDatabase = DDElements.Assets.GetAssetsInDirectory<IconsDatabase>(directory).FirstOrDefault();
                
                TextureImporter textureImporter = AssetImporter.GetAtPath(importedAsset) as TextureImporter;
                if (textureImporter != null && textureImporter.textureType != TextureImporterType.Sprite)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.maxTextureSize = 32;
                    textureImporter.SaveAndReimport();
                }

                shouldRefresh = true;
            }

            if (!shouldRefresh)
            {
                for (int i = 0; i < deletedAssets.Length; i++)
                {
                    string importedAsset = deletedAssets[i];
                    string directory = DDElements.Assets.GetAssetDirectory(importedAsset);
                
                    iconsDatabase = DDElements.Assets.GetAssetsInDirectory<IconsDatabase>(directory).FirstOrDefault();
                    shouldRefresh = iconsDatabase != null;
                }
            }

            if (iconsDatabase == null || !shouldRefresh)
            {
                return;
            }

            iconsDatabase.Refresh();
        }
    }
}