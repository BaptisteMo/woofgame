using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class BoostIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    private BoostData boostData;

    public void Setup(BoostData data)
    {
        boostData = data;
        iconImage.sprite = data.icon;
    }

    // 👉 Appelé automatiquement quand la souris entre dans le champ du GameObject
    public void OnPointerEnter(PointerEventData eventData)
    {
        BoostDetailsPanel.Instance.Show(boostData);
    }

    // 👉 Appelé quand la souris quitte le GameObject
    public void OnPointerExit(PointerEventData eventData)
    {
        BoostDetailsPanel.Instance.Hide(); // ou Clear()
    }
}