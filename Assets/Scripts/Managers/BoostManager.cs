using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public static BoostManager Instance;

     [SerializeField] private List<BoostData> acquiredBoosts = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RegisterBoost(BoostData boost)
    {
        if (!acquiredBoosts.Contains(boost))
        {
            acquiredBoosts.Add(boost);

            var boostUI = FindObjectOfType<BoostUI>();
            if (boostUI != null)
                boostUI.UpdateBoostUI();
        }
    }


    public void ApplyBoostsToPlayer(PlayerMovement player)
    {
        if(acquiredBoosts.Count == 0) return;
        foreach (var boost in acquiredBoosts)
        {
            boost.Apply(player);
        }
    }

    public void ResetBoosts()
    {
        acquiredBoosts.Clear();
    }
}