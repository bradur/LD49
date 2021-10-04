using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/New PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    // Forward movement, doesn't affect gameplay directly
    public float ForwardMovementSpeed;
    public float MinForwardSwaySpeed;
    public float MaxForwardSwaySpeed;

    // Input related
    public float SidewaysMovementSpeed; // Player movement speed
    public float MinInputBalanceWeight; // How much input changes the balance at minimum
    public float MaxInputBalanceWeight; // How much input changes the balance at maximum
    public float InitialInputBalanceVelocity; // Initial velocity for balance change when key is pressed
    public float InputBalanceVelocityDampeningFactor;

    // Not directly input related
    public float MinSidewaysSwaySpeed; // Total minimum sway speed
    public float MaxSidewaysSwaySpeed; // Total maximum sway speed
    public float MinDifficultyCoef; // Min difficulty of keeping balance (over time)
    public float MaxDifficultyCoef; // Max difficulty of keeping balance (over time)
    public float MinBalanceScale; // Min scale factor based on balance ratio
    public float MaxBalanceScale;
    public float MaxBalance; // How big abs(balance) can be, at max sidewaysSwaySpeed == MaxSidewaysSwaySpeed
    public float MinBalance;
    public float BalanceWeight; // How much balance is changed at most between frames
    public float PerlinNoiseScale; // How fast noise values change


    // Sideways movement is controlled by these factors
    // - Player control moves the character left or right
    // - Player controls sets balance to left or right
    // - Balance makes the character move left or right (slightly)
    // - Balance fluctuates
    //  - amount depends on the balance
    //  - at the edge means it gets harder
    // - Over time balance fluctuation gets harder (difficulty)
    // - At the balance extremes (and/or over time) player controls have a bigger effect on balance
    // - Longer player holds input direction, more weight the input has on balance (and movement?)
    // When balance is close to the center, player controls make character move faster than sway off balance
    // When balance is close to the edge, player controls changes balance faster/as fast as they move the character
    // Maybe: Character can fall over (game over) anywhere on the road if the balance is too far off
}
