using TMPro;
using UnityEngine;

public class GoldText : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private TextMeshProUGUI goldTMP;

    public void SetGoldText(float damage)
    {
        goldTMP.text = damage.ToString();
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}