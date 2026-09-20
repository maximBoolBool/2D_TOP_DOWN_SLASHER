#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;

public class FixSpriteAssetUnicode
{
    [MenuItem("Tools/Fix Selected Sprite Asset Unicode")]
    public static void FixUnicode()
    {
        // Берем выделенный Sprite Asset
        TMP_SpriteAsset spriteAsset = Selection.activeObject as TMP_SpriteAsset;

        if (spriteAsset == null)
        {
            Debug.LogError("Выделите файл Sprite Asset в окне Project!");
            return;
        }

        Undo.RecordObject(spriteAsset, "Fix Unicode");

        foreach (var sprite in spriteAsset.spriteCharacterTable)
        {
            if (string.IsNullOrEmpty(sprite.name)) continue;

            // Если имя спрайта состояит из одного символа (буква/цифра)
            if (sprite.name.Length == 1)
            {
                char c = sprite.name[0];
                sprite.unicode = (uint)c; // Автоматически ставит правильный Unicode (41 для 'A', 42 для 'B' и т.д.)
            }
        }

        EditorUtility.SetDirty(spriteAsset);
        AssetDatabase.SaveAssets();
        Debug.Log("Unicode для всех букв успешно обновлен!");
    }
}
#endif