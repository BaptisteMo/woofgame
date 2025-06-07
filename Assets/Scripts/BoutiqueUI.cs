using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutiqueUI : MonoBehaviour
{
    public void LaunchNextLevel()
    {
        int nextLevel = GameSession.Instance.currentLevel + 1;
        GameSession.Instance.currentLevel = nextLevel;

        string sceneName = "Level_" + nextLevel;
        SceneManager.LoadScene(sceneName);
    }
}