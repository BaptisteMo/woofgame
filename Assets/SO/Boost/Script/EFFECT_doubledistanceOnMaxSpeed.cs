using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/DoubleDistanceOnMaxSpeed")]
public class DoubleDistanceEffect : ScriptableObject, IEffect
{
    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.doubleDistanceOnMaxSpeed = true;
    }
}