using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float maxSpeed;          // Maximum forward speed
    private float startSpeed;          // Initial speed
    private float accelerationDuration;
    private bool canSwitchLane = true;
    public float laneLockDuration = 0.4f;

    public float boostSpeed;
    public float laneDistance = 5f;
    private int currentLane = 0;
    private Vector3 targetPosition;
    public bool isFinished = false;


    public float currentSpeed;
    private float elapsedTime = 0f;



    void Start()
    {

        startSpeed = GameSession.Instance.baseSpeed;
        maxSpeed = GameSession.Instance.maxSpeed;
        accelerationDuration = GameSession.Instance.accelerationDuration;
        currentSpeed = startSpeed;

        UpdateTargetPosition();
    }

    void Update()
    {
        if (isFinished) return;

        //Debug.Log(Math.Min((currentSpeed + boostSpeed), maxSpeed) );
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / accelerationDuration); // value between 0 and 1
        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);
        
        // Mouvement automatique vers l'avant
        transform.Translate(Vector3.forward * Math.Min((currentSpeed + boostSpeed), maxSpeed)  * Time.deltaTime, Space.World);

        // Mouvement latéral vers la position cible
        Vector3 newPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        // On fait un Lerp pour adoucir le mouvement
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10f);

        // Si on est très proche du target X, on le force pour éviter les décimales flottantes
        if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.01f)
        {
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        }
        // Left input
        if (canSwitchLane && Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > -1)
        {
            currentLane--;
            UpdateTargetPosition();
        }

        // Right input
        if (canSwitchLane && Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 1)
        {
            currentLane++;
            UpdateTargetPosition();
        }
        
    }

// Boost the current speed without exceeding the maximum allowed
    public void BoostSpeed(float amount)
    {
        boostSpeed += amount;
    }

    void UpdateTargetPosition()
    {
        float xPosition = currentLane * laneDistance;
        targetPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall Collision");
            // 1. Réduire la vitesse de moitié
            ReduceSpeedByHalf();

            // 2. Recentrer sur la ligne actuelle
           RecenterOnLane();
           StartCoroutine(LockLaneSwitchCoroutine(laneLockDuration));

        }
    }
    public void ReduceSpeedByHalf()
    {
        currentSpeed *= 0.5f;
        boostSpeed *= 0.5f;
    }

    private IEnumerator LockLaneSwitchCoroutine(float duration)
    {
        canSwitchLane = false;
        yield return new WaitForSeconds(duration);
        canSwitchLane = true;

    }
    
    public void RecenterOnLane()
    {
        // Calcule la ligne la plus proche selon l'axe X
        float x = transform.position.x;

        // On suppose : ligne -1 = gauche, 0 = centre, 1 = droite
        // Donc si laneDistance = 5, alors :
        // x < -2.5 => gauche, x entre -2.5 et 2.5 => centre, x > 2.5 => droite

        if (x < -laneDistance / 2f)
            currentLane = -1;
        else if (x > laneDistance / 2f)
            currentLane = 1;
        else
            currentLane = 0;
        UpdateTargetPosition();
    }

}