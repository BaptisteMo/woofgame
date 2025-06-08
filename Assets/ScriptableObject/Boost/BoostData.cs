using UnityEngine;

[CreateAssetMenu(fileName = "NewBoost", menuName = "Boutique/Boost")]
public class BoostData : ScriptableObject
{
    public string boostName;
   //TODO public Sprite icon;
    public int price;
    public string description;
    public enum BoostType { WallResistance, CoinLuck, Other }
    public BoostType type;
}