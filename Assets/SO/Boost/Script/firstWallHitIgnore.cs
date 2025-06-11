using UnityEngine;


[CreateAssetMenu(menuName = "Boosts/First Wall Hit ignored")]
public class firstWallHitIgnore : ScriptableObject, IEffect
{

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.ignoreFirstCollision = true;
        Debug.Log($"🎁 first wall hit actif : Ignore la première collision");

    }
}