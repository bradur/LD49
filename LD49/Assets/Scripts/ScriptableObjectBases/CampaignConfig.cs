using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


[CreateAssetMenu(fileName = "CampaignConfig", menuName = "Configs/New CampaignConfig")]
public class CampaignConfig : ScriptableObject
{

    public List<RoadBiomeConfig> Configs;
    private int currentBiomeIndex = 0;

    public void Init() {
        currentBiomeIndex = 0;
    }


    public RoadBiomeConfig GetFirstBiome() {
        if (Configs.Count < 1) {
            throw new System.Exception("You need to add BiomeConfigs to your Campaign Configs list!");
        }
        return Configs[0];
    }

    public RoadBiomeConfig GetNextBiome() {
        currentBiomeIndex++;
        if (currentBiomeIndex >= Configs.Count)
        {
            currentBiomeIndex = 0;
        }
        return Configs[currentBiomeIndex];
    }
}
