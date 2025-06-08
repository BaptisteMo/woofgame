#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class LevelGeneratorEditor : EditorWindow
{
    private int levelNumber = 1;
    private string levelPath = "Assets/Scenes/Levels/";

    private SceneTemplateAsset baseLevelTemplate;
    private SceneTemplateAsset bossLevelTemplate;

    [MenuItem("Tools/Generate Level Scenes")]
    public static void ShowWindow()
    {
        GetWindow<LevelGeneratorEditor>("Générateur de Niveau");
    }

    private void OnGUI()
    {
        GUILayout.Label("Créer un nouveau niveau complet", EditorStyles.boldLabel);
        levelNumber = EditorGUILayout.IntField("Numéro du niveau :", levelNumber);

        EditorGUILayout.Space(10);

        baseLevelTemplate = (SceneTemplateAsset)EditorGUILayout.ObjectField("Template classique :", baseLevelTemplate, typeof(SceneTemplateAsset), false);
        bossLevelTemplate = (SceneTemplateAsset)EditorGUILayout.ObjectField("Template boss :", bossLevelTemplate, typeof(SceneTemplateAsset), false);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Générer les 3 scènes"))
        {
            if (baseLevelTemplate == null || bossLevelTemplate == null)
            {
                EditorUtility.DisplayDialog("Erreur", "Tu dois renseigner les deux templates !", "OK");
                return;
            }

            GenerateScenes(levelNumber);
        }
    }

    private void GenerateScenes(int number)
    {
        for (int i = 1; i <= 3; i++)
        {
            string sceneName = $"Level_{number}_{i}";
            string fullPath = levelPath + sceneName + ".unity";

            SceneTemplateAsset chosenTemplate = (i < 3) ? baseLevelTemplate : bossLevelTemplate;

            SceneTemplateService.Instantiate(chosenTemplate, false, fullPath);
            Debug.Log($"Scène créée : {sceneName}");
        }

        AssetDatabase.Refresh();
    }
}
#endif