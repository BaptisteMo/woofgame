using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/AdjacentLootCollector")]
public class Boost_AdjacentLootCollector : ScriptableObject, IEffect
{
    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.adjacentLootCollectorEnabled = true;
        Debug.Log("🎁 Boost activé : le joueur récupère aussi les caisses des lanes adjacentes !");
    }
}