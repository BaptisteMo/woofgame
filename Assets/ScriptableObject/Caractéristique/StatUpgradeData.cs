using UnityEngine;

[CreateAssetMenu(fileName = "NewStatUpgrade", menuName = "Boutique/Stat Upgrade")]
public class StatUpgradeData : ScriptableObject
{
    public string statName;
   // public Sprite icon;
    public int price;

    public float speedBonus;
    public float accelerationBonus;
}