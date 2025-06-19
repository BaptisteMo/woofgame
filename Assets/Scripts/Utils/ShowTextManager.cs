using UnityEngine;

public class ShowTextManager : MonoBehaviour
{
    public static ShowTextManager Instance;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    [Header("Config")]
    [SerializeField] private GoldText damageTextPrefab;
    

    
    public void GoldUp(float goldAmount, Transform parent)
    {
        GoldText text = Instantiate(damageTextPrefab, parent);
        //text.transform.position += Vector3.up * 0.2f;
        text.SetGoldText(goldAmount);
    }
   
}