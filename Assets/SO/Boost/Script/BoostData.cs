using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewBoost", menuName = "Boutique/Boost")]
public class BoostData : ScriptableObject
{
    public string boostName;
    public int price;
    [TextArea]
    public string description;

    public Sprite icon; // 🖼️ Image à afficher dans l'UI

    public enum BoostType
    {
        WallResistance, CoinLuck, Other, GenerateOnMaxSpeed, MaxSpeed, Acceleration, LootCrate, Bonus
    }

    public BoostType type;

    public ScriptableObject effect; // doit implémenter IEffect

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