using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public static BoostManager Instance;

    public List<BoostData> acquiredBoosts = new();

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
            acquiredBoosts.Add(boost);
    }

    public void ApplyBoostsToPlayer(PlayerMovement player)
    {
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