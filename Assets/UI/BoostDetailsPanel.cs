using UnityEngine;
using TMPro;

public class BoostDetailsPanel : MonoBehaviour
{
    public static BoostDetailsPanel Instance;

    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        Instance = this;
        panelRoot.SetActive(false);
    }

    public void Show(BoostData boost)
    {
        titleText.text = boost.boostName;
        descriptionText.text = boost.description;
        panelRoot.SetActive(true);
    }

    public void Hide()
    {
        panelRoot.SetActive(false);
    }
}