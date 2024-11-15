using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
#if UNITY_EDITOR
public class AddressAbleController : EditorWindow
{
    private string folderPath = "Assets/00 DevHai/01 Prefabs/Level";
    private string addressableGroupName = "Level"; 

    [MenuItem("Tools/Auto Add to Addressable Group")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AddressAbleController), true, "Auto");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto Add Addressables", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        addressableGroupName = EditorGUILayout.TextField("Addressable Group Name", addressableGroupName);

        if (GUILayout.Button("Add Assets to Addressable Group"))
        {
            AddAssetsToGroup(folderPath, addressableGroupName);
        }
    }

    private void AddAssetsToGroup(string folderPath, string groupName)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found. Make sure you have Addressables set up in your project.");
            return;
        }

        AddressableAssetGroup group = settings.FindGroup(groupName);
        if (group == null)
        {
            Debug.LogError($"Group {groupName} not found. Make sure the group exists in your Addressable settings.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var entry = settings.CreateOrMoveEntry(guid, group);

            if (entry != null)
            {
                entry.address = assetPath.Replace(folderPath + "/", ""); // Optional: Set the address as the relative path
                Debug.Log($"Added {assetPath} to {groupName}");
            }
            else
            {
                Debug.LogWarning($"Failed to add {assetPath} to {groupName}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif