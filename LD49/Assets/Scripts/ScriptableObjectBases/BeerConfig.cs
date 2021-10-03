using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeerConfig", menuName = "Configs/New BeerConfig")]
public class BeerConfig : ScriptableObject
{
    public float BeerSipAmount; // How much beer-o-meter lowers from drinking
    public float BeerPickupAmount; // How much beer-o-meter rises from beer pickup
    public float BeerSipScore; // How many points drinking gives before modifier
    public float MinBeerScoreModifier;
    public float MaxBeerScoreModifier;
    public float BeerTotal; // Beer-o-meter maximum
}
