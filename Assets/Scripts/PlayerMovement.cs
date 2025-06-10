using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float maxSpeed;
    private float baseSpeed;
    private float accelerationDuration;
    private float accelerationTimer = 0f;

    public float currentSpeed { get; private set; }
    private float targetSpeed;

    private bool canSwitchLane = true;
    public float laneLockDuration = 0.4f;

    public float laneDistance = 5f;
    private int currentLane = 0;
    private Vector3 targetPosition;
    public bool isFinished = false;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseSpeed = GameSession.Instance.baseSpeed;
        maxSpeed = GameSession.Instance.maxSpeed;
        accelerationDuration = GameSession.Instance.accelerationDuration;

        currentSpeed = baseSpeed;
        targetSpeed = maxSpeed;
        
        BoostManager.Instance.ApplyBoostsToPlayer(this); // ✅ Application des boosts

        UpdateTargetPosition();
    }

    private void FixedUpdate()
    {
        if (isFinished) return;

        // Toujours avancer dans le temps d'accélération
        accelerationTimer += Time.deltaTime;

        // Calcule le facteur d'interpolation (linéaire)
        float t = Mathf.Clamp01(accelerationTimer / accelerationDuration);

        // Interpolation linéaire de baseSpeed vers maxSpeed
        currentSpeed = Mathf.Lerp(baseSpeed, GameSession.Instance.maxSpeed, t);

        // Mouvement vers l’avant
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.World);
    }




    void Update()
    {
        if (isFinished) return;

        // ⏩ Accélération progressive vers targetSpeed
        accelerationTimer += Time.deltaTime;

        // 🚀 Mouvement avant

        // ↔️ Mouvement latéral
        
        Vector3 newPosition = new Vector3(targetPosition.x, rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.deltaTime * 10f));

        // ⌨️ Input
        if (canSwitchLane && Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > -1)
        {
            currentLane--;
            UpdateTargetPosition();
        }

        if (canSwitchLane && Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 1)
        {
            currentLane++;
            UpdateTargetPosition();
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
        baseSpeed = currentSpeed + amount; // repart de cette nouvelle vitesse
        accelerationTimer = 0f; // recommence l’interpolation depuis maintenant
    }




    public void DecreaseSpeed(float amount)
    {
        baseSpeed = Mathf.Max(currentSpeed - amount, 2f); // évite les vitesses trop faibles
        accelerationTimer = 0f;
    }

    public void ModifyPercentSpeed(float amount)
    {
        baseSpeed = Mathf.Max(currentSpeed * amount, 2f);
        accelerationTimer = 0f;
    }


    public void ResetSpeed()
    {
        targetSpeed = baseSpeed;
        accelerationTimer = 0f;
    }

    public void ReduceSpeedByHalf()
    {
        baseSpeed *= GameSession.Instance.wallHitMalus;
        accelerationTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ReduceSpeedByHalf();
            RecenterOnLane();
            StartCoroutine(LockLaneSwitchCoroutine(laneLockDuration));
        }
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
    

}
