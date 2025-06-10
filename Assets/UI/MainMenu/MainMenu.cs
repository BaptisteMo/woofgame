using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{
    public UIDocument uiDocument;
    public List<string> scenes = new List<string>();

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        var sceneDropdown = root.Q<DropdownField>("scene-dropdown");
        var playButton = root.Q<Button>("play-scene-button");
        var shopButton = root.Q<Button>("go-shop-button");
        

        sceneDropdown.choices = scenes;
        sceneDropdown.value = scenes[0];

        playButton.clicked += () =>
        {
            string selectedScene = sceneDropdown.value;
            SceneManager.LoadScene(selectedScene);
        };

        shopButton.clicked += () =>
        {
            SceneManager.LoadScene("Boutique");
        };
    }
}