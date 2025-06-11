using UnityEngine;


[CreateAssetMenu(menuName = "Boosts/Wall Hit reduced")]
public class WallHitReduced : ScriptableObject, IEffect
{

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.wallHitReduced = true;
        Debug.Log($"🎁 Boost Wall hit actif : Les ralentissement sont réduits de moitié lors des collisions");

    }
}