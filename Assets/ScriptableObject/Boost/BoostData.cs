using UnityEngine;

[CreateAssetMenu(fileName = "NewBoost", menuName = "Boutique/Boost")]
public class BoostData : ScriptableObject
{
    public string boostName;
    public int price;
    public string description;

    public enum BoostType
    {
        WallResistance, CoinLuck, Other, GenerateOnMaxSpeed, MaxSpeed, Acceleration
        
    }
    public BoostType type;

    public ScriptableObject effect; // doit implémenter IBoostEffect

    public void Apply(PlayerMovement player)
    {
        if (effect is IEffect boostEffect)
        {
            boostEffect.Apply(player);
        }
        else
        {
            Debug.LogError($"L'effet du boost {boostName} n'implémente pas IBoostEffect !");
        }
    }
}