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
        cursor.Initialize(config.MaxBalance);
    }

    // Update is called once per frame
    void Update()
    {
        float z = config.ForwardMovementSpeed; //TODO: forward swaying?

        float xInput = Input.GetAxis("Horizontal");

        // update currentBalance
        float balanceRatio = Mathf.Abs(currentBalance) / config.MaxBalance;
        float balanceScale = balanceRatio * (config.MaxBalanceScale - config.MinBalanceScale) + config.MinBalanceScale; //Mathf.Sign(currentBalance) * Mathf.Max(Mathf.Abs(currentBalance) / config.MaxBalance, 0.1f);
        // calculate random value, offset to be different from perlin noise below and scaled to [-1,1]
        float balanceNoise = Mathf.PerlinNoise(Time.time * config.PerlinNoiseScale + 2451.3f, 0.0f) * 2f - 1f;
        // input changes balance
        float balanceInputEffect = -xInput * (balanceRatio * (config.MaxInputBalanceWeight - config.MinInputBalanceWeight) + config.MinInputBalanceWeight);
        currentBalance = currentBalance 
            + balanceNoise * balanceScale * config.BalanceWeight * Time.deltaTime
            + balanceInputEffect * Time.deltaTime;
        currentBalance = Mathf.Clamp(currentBalance, -config.MaxBalance, config.MaxBalance);

        float perlinNoise = Mathf.PerlinNoise(Time.time * config.PerlinNoiseScale, 0.0f);
        // current balance ratio determines the sideways sway amount
        float swaySpeed = Mathf.Abs(currentBalance) / config.MaxBalance * (config.MaxSidewaysSwaySpeed - config.MinSidewaysSwaySpeed) + config.MinSidewaysSwaySpeed;
        float randomSway = perlinNoise * swaySpeed; // current amount of random sidestep
        float xMovement = xInput + randomSway;

        Vector3 move = transform.right * xMovement + transform.forward * z;

        controller.Move(move * Time.deltaTime * currentSidewaysSpeed);
        cursor.SetBalance(currentBalance);
        Debug.Log("cBal: " + currentBalance + " |\txAxis: " + xInput + " |\tbInput: " + balanceInputEffect + " |\tbScale: " + balanceScale);
    }
}
