using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<int> OnLaneChanged;

    
    
    private float startSpeed;    // la vitesse au début de l’accélération
    private float maxSpeed;      // vitesse cible (modifiable dynamiquement)
    private float baseMaxSpeed;

    private float accelerationDuration;
    private float accelerationTimer = 0f;
    
    public float currentSpeed { get; private set; }
    private float targetSpeed;

    private bool canSwitchLane = true;
    public float laneLockDuration = 0.4f;

    public float laneDistance = 5f;
    public int currentLane = 0;
    private Vector3 targetPosition;
    public bool isFinished = false;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        startSpeed = GameSession.Instance.baseSpeed;
        maxSpeed = GameSession.Instance.maxSpeed;
        baseMaxSpeed = maxSpeed;
        accelerationTimer = 0f;
        currentSpeed = startSpeed;
        currentLane = 0;
        Debug.Log("🔍 Vitesse max initialisée à : " + maxSpeed);

        accelerationDuration = GameSession.Instance.accelerationDuration;

        targetSpeed = maxSpeed;
        
        BoostManager.Instance.ApplyBoostsToPlayer(this); // ✅ Application des boosts

        UpdateTargetPosition();
    }

    private void FixedUpdate()
    {
        if (isFinished) return;

        accelerationTimer += Time.fixedDeltaTime;

        float t = Mathf.Clamp01(accelerationTimer / accelerationDuration);
        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);

        // ✅ Mouvement avant avec collision
        Vector3 forwardMovement = Vector3.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }




    void Update()
    {
        if (isFinished) return;

        // ⏩ Accélération progressive vers targetSpeed
        accelerationTimer += Time.deltaTime;
        GameSession.Instance.lastPlayerSpeed = currentSpeed;
        Vector3 newPosition = new Vector3(targetPosition.x, rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.deltaTime * 10f));
        // ⌨️ Input
        if (canSwitchLane && Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > -1)
        {
            SetLane(currentLane - 1);

        }

        if (canSwitchLane && Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 1)
        {
            SetLane(currentLane + 1);

        }
   
    }

    void UpdateTargetPosition()
    {
        float xPosition = currentLane * laneDistance;
        targetPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
    }


    // 🧠 External effects
    public void BoostSpeed(float amount)
    {
        startSpeed = currentSpeed + amount; // repart de cette nouvelle vitesse
        accelerationTimer = 0f; // recommence l’interpolation depuis maintenant
    }

    private void SetLane(int newLane)
    {
        if (currentLane != newLane)
        {
            currentLane = newLane;
            OnLaneChanged?.Invoke(currentLane);
            UpdateTargetPosition();
        }
    }


    public void DecreaseSpeed(float amount)
    {
        startSpeed = Mathf.Max(currentSpeed - amount, 2f); // évite les vitesses trop faibles
        accelerationTimer = 0f;
    }

    public void ModifyPercentSpeed(float amount)
    {
        startSpeed = Mathf.Max(currentSpeed * amount, 2f);
        accelerationTimer = 0f;
    }


    public void ResetSpeed()
    {
        targetSpeed = startSpeed;
        accelerationTimer = 0f;
    }

    public void ReduceSpeedByHalf()
    {
        if (GameSession.Instance.ignoreFirstCollision && GameSession.Instance.wallHitCount < 1) return;
        
        if (GameSession.Instance.wallHitReduced)
        {
            Debug.Log("Wall hit reduced");
            
            ModifyPercentSpeed(0.75f);

        }
        else
        {
            Debug.Log("Wall hit not reduced");
            ModifyPercentSpeed(0.5f);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall collision detected");
            ReduceSpeedByHalf();
            RecenterOnLane();
            StartCoroutine(LockLaneSwitchCoroutine(laneLockDuration));
            GameSession.Instance.wallHitCountTrack();
            // 🔄 Notifie le tracker s’il existe
            var tracker = GetComponent<MaxSpeedNoCollisionTracker>();
            var combo = GetComponent<CollectorComboSpeedTracker>();
            if (combo != null)
            {
                combo.OnObstacleHit();
            }
            if (tracker != null)
            {
                tracker.OnWallCollision();
            }
        }
    }
    public void SetMaxSpeed(float newMaxSpeed)
    {
        Debug.Log($"🎯 SetMaxSpeed appelé avec {newMaxSpeed}");

        startSpeed = currentSpeed; // ← on repart depuis la vitesse actuelle
        maxSpeed = newMaxSpeed;
        accelerationTimer = 0f;    // ← redémarre l’accélération
    }

    private IEnumerator LockLaneSwitchCoroutine(float duration)
    {
        canSwitchLane = false;
        yield return new WaitForSeconds(duration);
        canSwitchLane = true;
    }

    public void RecenterOnLane()
    {
        float x = transform.position.x;

        // Détermine les X exacts de chaque lane
        float leftX = -laneDistance;
        float centerX = 0f;
        float rightX = laneDistance;

        // Calcule la distance du joueur à chaque lane
        float distLeft = Mathf.Abs(x - leftX);
        float distCenter = Mathf.Abs(x - centerX);
        float distRight = Mathf.Abs(x - rightX);

        // Compare et assigne la lane la plus proche
        if (distLeft < distCenter && distLeft < distRight)
            currentLane = -1;
        else if (distRight < distCenter && distRight < distLeft)
            currentLane = 1;
        else
            currentLane = 0;

        UpdateTargetPosition();
    }

    public bool CurrentSpeedIsMax()
    {
        if (currentSpeed >= GameSession.Instance.maxSpeed)
            return true;
        else
        {
            return false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.left * laneDistance, transform.position + Vector3.left * laneDistance + Vector3.forward * 10f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 10f);
        Gizmos.DrawLine(transform.position + Vector3.right * laneDistance, transform.position + Vector3.right * laneDistance + Vector3.forward * 10f);  
        #if UNITY_EDITOR
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Speed: {currentSpeed:F1} / Max: {maxSpeed:F1}");
        #endif
    }
    
  
    private Dictionary<string, float> speedModifiers = new();

    public void AddSpeedModifier(string sourceId, float percent)
    {
        speedModifiers[sourceId] = percent;
        RecalculateMaxSpeed();
    }

    public void RemoveSpeedModifier(string sourceId)
    {
        if (speedModifiers.ContainsKey(sourceId))
        {
            speedModifiers.Remove(sourceId);
            RecalculateMaxSpeed();
        }
    }

    private void RecalculateMaxSpeed()
    {
        float totalBonusPercent = 0f;
        foreach (var modifier in speedModifiers.Values)
        {
            totalBonusPercent += modifier;
        }

        float newMaxSpeed = baseMaxSpeed * (1f + totalBonusPercent / 100f);
        SetMaxSpeed(newMaxSpeed);
    }


    

}
