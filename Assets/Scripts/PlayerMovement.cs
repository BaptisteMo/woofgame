using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<int> OnLaneChanged;

    
    
    private float startSpeed;    // la vitesse au début de l’accélération
    private float maxSpeed;      // vitesse cible (modifiable dynamiquement)

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
        accelerationTimer = 0f;
        currentSpeed = startSpeed;
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

        // Avance du timer
        float t = Mathf.Clamp01(accelerationTimer / accelerationDuration);

        // Interpolation entre start et max
        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);

        // Mouvement avant
        transform.Translate(Vector3.forward * currentSpeed * Time.fixedDeltaTime, Space.World);
    }



    void Update()
    {
        if (isFinished) return;

        // ⏩ Accélération progressive vers targetSpeed
        accelerationTimer += Time.deltaTime;
        GameSession.Instance.lastPlayerSpeed = currentSpeed;

        // 🚀 Mouvement avant

        // ↔️ Mouvement latéral
        
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
        startSpeed *= GameSession.Instance.wallHitMalus;
        accelerationTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ReduceSpeedByHalf();
            RecenterOnLane();
            StartCoroutine(LockLaneSwitchCoroutine(laneLockDuration));

            // 🔄 Notifie le tracker s’il existe
            var tracker = GetComponent<MaxSpeedNoCollisionTracker>();
            
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
        if (x < -laneDistance / 2f) currentLane = -1;
        else if (x > laneDistance / 2f) currentLane = 1;
        else currentLane = 0;

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
        #if UNITY_EDITOR
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Speed: {currentSpeed:F1} / Max: {maxSpeed:F1}");
        #endif
    }

    

}
