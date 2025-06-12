using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/PerfectRunDoubleDistance")]
public class PerfectRunDoubleDistance  : ScriptableObject, IEffect
{

    public void Apply(PlayerMovement player)
    {
        GameSession.Instance.perfectRunMultiplier = true;
        

        Debug.Log($"🚦 Boost activé : Si run parfaite, distance X2");
    }
}