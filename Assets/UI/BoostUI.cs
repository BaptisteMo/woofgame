using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoostUI : MonoBehaviour
{
    public GameObject boostIconTemplate;
    public Transform boostContainer;

    private void Start()
    {
        UpdateBoostUI();
    }

    public void UpdateBoostUI()
    {
        foreach (Transform child in boostContainer)
        {
            if (child != boostIconTemplate.transform)
                Destroy(child.gameObject);
        }

        List<BoostData> boosts = GetBoosts();

        foreach (var boost in boosts)
        {
            GameObject icon = Instantiate(boostIconTemplate, boostContainer);
            icon.SetActive(true);

            icon.GetComponent<BoostIconUI>().Setup(boost);
        }
    }

    private List<BoostData> GetBoosts()
    {
        return typeof(BoostManager).GetField("acquiredBoosts",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(BoostManager.Instance) as List<BoostData>;
    }
}