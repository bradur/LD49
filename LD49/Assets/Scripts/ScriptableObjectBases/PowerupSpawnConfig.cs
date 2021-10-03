using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


[CreateAssetMenu(fileName = "PowerupSpawnConfig", menuName = "Configs/New PowerupSpawnConfig")]
public class PowerupSpawnConfig : ScriptableObject
{

    public List<RoadPowerupConfig> Configs;
    private int currentIndex = 0;

    public void Init() {
        currentIndex = 0;
    }


    public RoadPowerupConfig GetFirst() {
        if (Configs.Count < 1) {
            throw new System.Exception("You need to add BiomeConfigs to your Campaign Configs list!");
        }
        return Configs[0];
    }

    public RoadPowerupConfig GetNext() {
        currentIndex++;
        if (currentIndex >= Configs.Count)
        {
            currentIndex = Configs.Count - 1;
        }
        return Configs[currentIndex];
    }
}
