using UnityEngine;

[CreateAssetMenu(menuName = "Boosts/ComboCollectorSpeedBoost")]
public class ComboCollectorSpeedBoost : ScriptableObject, IEffect
{
    [Range(0f, 100f)]
    public float bonusPercentPerCombo = 5f;

    public void Apply(PlayerMovement player)
    {
        var tracker = player.GetComponent<CollectorComboSpeedTracker>();
        if (tracker == null)
        {
            tracker = player.gameObject.AddComponent<CollectorComboSpeedTracker>();
        }

        tracker.Setup(bonusPercentPerCombo, player);

        Debug.Log($"🎁 Boost combo actif : +{bonusPercentPerCombo}% vitesse max toutes les 3 collectes sans collision");
    }
}