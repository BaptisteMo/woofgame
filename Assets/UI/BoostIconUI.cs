using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoostIconUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private BoostData boostData;

    public void Setup(BoostData data)
    {
        boostData = data;
        iconImage.sprite = data.icon;

        // ⚠️ Ajoute un listener s’il y a un bouton
        GetComponent<Button>().onClick.AddListener(ShowDetails);
    }

    public void ShowDetails()
    {
        BoostDetailsPanel.Instance.Show(boostData);
    }
}