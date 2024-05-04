using System.IO;
using CardGame.Enums;
using CardGame.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class CreateCardDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Terek Gaming/Create Card Data Objects")]
    private static void OpenWindow()
    {
        GetWindow<CreateCardDataEditor>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Add("Create Card Data", new CardDataCreator());
        return tree;
    }

    public class CardDataCreator
    {
        [FolderPath(RequireExistingPath = true), LabelText("Sprite Folder Path")]
        public string spriteFolderPath;

        [FolderPath(RequireExistingPath = true), LabelText("Save Path for ScriptableObjects")]
        public string savePath;

        [Button("Generate Card Data Objects")]
        private void GenerateCardDataObjects()
        {
            if (string.IsNullOrEmpty(spriteFolderPath) || string.IsNullOrEmpty(savePath))
            {
                Debug.LogError("Both paths must be set!");
                return;
            }

            var spriteFiles = Directory.GetFiles(spriteFolderPath, "*.png", SearchOption.TopDirectoryOnly);
            int index = 0;
            foreach (var file in spriteFiles)
            {
                if (index > (int)CardType.Z) break;
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(file);
                
                if (sprite != null)
                {
                    CreateCardDataObject(sprite, index);
                }

                index++;
            }

            index = 0;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Card Data Objects created successfully!");
        }

        private void CreateCardDataObject(Sprite sprite, int index)
        {
            var cardData = CreateInstance<CardDataContainerSO>();
            cardData.SetEditorBuild(sprite, (CardType)index);

            string assetPath = Path.Combine(savePath, $"{sprite.name}_CardDataContainer.asset");
            AssetDatabase.CreateAsset(cardData, assetPath);
        }
    }
}
