using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Boosts/GoldOnMaxSpeed")]

public class GoldOnMaxSpeed : ScriptableObject, IEffect
{
    public float interval = 2f; // toutes les 2 secondes par défaut
    public int goldGenerated;
    public int price;
    public void Apply(PlayerMovement player)
    {
        player.StartCoroutine(GoldRoutine(player));
    }

    private IEnumerator GoldRoutine(PlayerMovement player)
    {
        while (!player.isFinished)
        {
            if (player.CurrentSpeedIsMax())
            {
                GameSession.Instance.playerMoney += goldGenerated;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}

