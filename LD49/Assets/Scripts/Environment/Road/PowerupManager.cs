using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager main;
    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private PowerupSpawnConfig spawnConfig;

    private int currentIndex = 0;
    private RoadPowerupConfig currentConfig;

    private bool initialized = false;
    private bool allowSpawn = false;

    [SerializeField]
    private GameObject propContainerPrefab;
    private Transform propContainer;

    private Spline road;

    public void Begin()
    {
        if (propContainer == null) {
            GameObject propContainerObject = Instantiate(propContainerPrefab);
            propContainerObject.transform.position = propContainerPrefab.transform.position;
            propContainerObject.transform.rotation = Quaternion.identity;
            propContainer = propContainerObject.transform;
        }
        road = GenerateRoad.main.Road;

        allowSpawn = true;
        spawnConfig.Init();
        if (!initialized)
        {
            initialized = true;
            road.NodeListChanged += OnRoadLengthChange;
            currentConfig = spawnConfig.GetFirst();
            currentConfig.Init(false);
        }
    }

    public void DestroyProps(SplineNode node) {
        foreach(RoadPowerupConfig powerupConfig in spawnConfig.Configs) {
            powerupConfig.DestroyProps(node);
        }
    }

    public void Stop()
    {
        allowSpawn = false;
    }

    private void OnRoadLengthChange(object sender, ListChangedEventArgs<SplineNode> args)
    {
        if (!allowSpawn || args.newItems == null || args.newItems.Count == 0 || currentConfig == null)
        {
            return;
        }
        if (currentConfig.IsFinished())
        {
            NextConfig();
        }
        currentConfig.NewNodeWasAdded(road, propContainer, args);
    }

    private void NextConfig()
    {
        var oldConfig = currentConfig;
        currentConfig = spawnConfig.GetNext();
        currentConfig.Init(false);
        Debug.Log($"Powerups: [{oldConfig}] -> {currentConfig}");
    }
}
