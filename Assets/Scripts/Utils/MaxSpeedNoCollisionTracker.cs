using System;
using UnityEngine;

public class MaxSpeedNoCollisionTracker : MonoBehaviour
{
    private float timeSinceLastCollision = 0f;
    private float requiredTime;
    private float bonusPercent;

    private bool bonusApplied = false;
    private PlayerMovement player;

    private float originalMaxSpeed;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    public void Setup(float requiredTime, float bonusPercent)
    {
        this.requiredTime = requiredTime;
        this.bonusPercent = bonusPercent;
        player = GetComponent<PlayerMovement>();
        originalMaxSpeed = GameSession.Instance.maxSpeed;
    }

    void Update()
    {
        timeSinceLastCollision += Time.deltaTime;

        if (!bonusApplied && timeSinceLastCollision >= requiredTime)
        {
            float bonusAmount = originalMaxSpeed * (bonusPercent / 100f);
            player.SetMaxSpeed(originalMaxSpeed + bonusAmount);
            bonusApplied = true;
        }
    }

    public void OnWallCollision()
    {
        timeSinceLastCollision = 0f;

        if (bonusApplied)
        {
            player.SetMaxSpeed(originalMaxSpeed);
            bonusApplied = false;
            Debug.Log("🚧 Collision détectée, boost de vitesse annulé");
        }
    }
}