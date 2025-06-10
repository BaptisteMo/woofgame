using UnityEngine;

[CreateAssetMenu(fileName = "NewBoost", menuName = "Boutique/Boost")]
public class BoostData : MonoBehaviour
{
    public string boostName;
    public int price;
    public string description;

    public enum BoostType
    {
        WallResistance, CoinLuck, Other, GenerateOnMaxSpeed, MaxSpeed, Acceleration,LootCrate, Bonus
        
    }
    public BoostType type;

    public UnityEngine.ScriptableObject effect; // doit implémenter IBoostEffect

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