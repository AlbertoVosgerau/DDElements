using System;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace DandyDino.Elements
{
    public class RenameAssetUtility : AssetCreationEndAction
    {
        public Action onRename;

        public override void Action(EntityId instanceId, string path, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return;
            }
            
            onRename?.Invoke();
            Debug.Log($"Path: {path}, New Name: {newName}");
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
        }
    }
}