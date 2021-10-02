using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysMovement : MonoBehaviour
{
    private float currentSidewaysSpeed = 1f; // base movementspeed of player when horizontal axis is 1f
    private float currentBalance = 0f;
    
    private CharacterController controller;

    [SerializeField]
    private PlayerConfig config;
    [SerializeField]
    private BalanceCursor cursor;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSidewaysSpeed = config.SidewaysMovementSpeed;
        // TODO: generate random value to offset perlin noise coordinate, so every run is different!
    }

    // Update is called once per frame
    void Update()
    {
        float z = config.ForwardMovementSpeed; //TODO: forward swaying?

        // update currentBalance
        float balanceScale = currentBalance / config.MaxBalance;
        float balanceNoise = Mathf.PerlinNoise(Time.time * 1f + 2451.3f, 0.0f) * 2f - 1f;
        currentBalance = currentBalance + balanceNoise * balanceScale;
        
        float perlinNoise = Mathf.PerlinNoise(Time.time * 1f, 0.0f);
        float swaySpeed = Mathf.Abs(currentBalance) / config.MaxBalance;
        float randomSway = perlinNoise * swaySpeed; // current amount of random sidestep
        float x = Input.GetAxis("Horizontal") + randomSway;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * Time.deltaTime * currentSidewaysSpeed);
        cursor.SetBalance(currentBalance);
    }
}
