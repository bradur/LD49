using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    public static BiomeManager main;
    private void Awake()
    {
        main = this;
    }

    private bool allowSpawn = false;
    [SerializeField]
    private bool debug = false;

    [SerializeField]
    private CampaignConfig campaign;

    private int currentBiomeIndex = 0;
    private RoadBiomeConfig currentBiome;

    [SerializeField]
    private GameObject propContainerPrefab;
    private Transform propContainer;

    private bool initialized = false;

    private Spline road;

    public void Begin()
    {
        if (propContainer == null) {
            GameObject propContainerObject = Instantiate(propContainerPrefab);
            propContainerObject.transform.position = Vector3.zero;
            propContainerObject.transform.rotation = Quaternion.identity;
            propContainer = propContainerObject.transform;
        }
        road = GenerateRoad.main.Road;
        if (campaign == null) {
            throw new System.Exception("BiomeManager needs a campaign!");
        }
        campaign.Init();
        allowSpawn = true;
        if (!initialized)
        {
            initialized = true;
            road.NodeListChanged += OnRoadLengthChange;
            currentBiome = campaign.GetFirstBiome();
            currentBiome.Init(debug);
        }
    }

    public void Stop()
    {
        allowSpawn = false;
    }

    private void OnRoadLengthChange(object sender, ListChangedEventArgs<SplineNode> args)
    {
        if (!allowSpawn || args.newItems == null || args.newItems.Count == 0 || currentBiome == null)
        {
            return;
        }
        if (currentBiome.IsFinished())
        {
            ChangeBiome();
        }
        currentBiome.NewNodeWasAdded(road, propContainer);
    }

    private void ChangeBiome()
    {
        RoadBiomeConfig oldBiome = currentBiome;
        currentBiome = campaign.GetNextBiome();
        currentBiome.Init(debug);
        Debug.Log($"Biome: [{oldBiome}] -> {currentBiome}");
    }

}