using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Boutique/Consumable")]
public class ConsumableData : UnityEngine.ScriptableObject
{
    public string consumableName;
  //  public Sprite icon;
    public int price;
    public float speedBoost; // Exemple d’effet
    // Tu peux ajouter d'autres effets ici
}