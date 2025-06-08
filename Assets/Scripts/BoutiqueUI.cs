using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutiqueUI : MonoBehaviour
{
    public void OnClick_Continuer()
    {
        string nextScene = GameSession.Instance.nextSceneName;

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogWarning("Aucune scène suivante définie !");
        }
    }
}
