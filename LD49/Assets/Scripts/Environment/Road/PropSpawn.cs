
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[System.Serializable]
public class PropSpawn {
    private bool debug = false;
    public void Init(bool debug) {
        this.debug = debug;
        ResetSpawnRate();
        spawned = 0;
    }

    private void Debug(string message) {
        if (debug) {
            MonoBehaviour.print($"<b><color=purple>[</color><color=white>PropSpawn</color><color=purple>]</color><color=white>:</color> </b>{message}");
        }
    }

    public void NewNode(Spline spline, Transform container) {
        if (ShouldSpawn()) {
            Spawn(spline, container);
        }
        nodesSinceSpawn++;
    }

    private void ResetSpawnRate() {
        nodesSinceSpawn = 0;
        nodesBetweenSpawns = Random.Range(EveryNNodesRange.x, EveryNNodesRange.y);
    }

    private void DebugSpawn(bool wasSpawned, string condition) {
        Debug($"Spawn [{Prefab.name}] <color={(wasSpawned ? "green" : "red")}>{(wasSpawned ? "YES" : "NO")}</color> ({condition})");
    }

    private bool ShouldSpawn() {
        if (Limit > 0 && spawned >= Limit) {
            DebugSpawn(false, $"Limit: {spawned} >= {Limit}");
            return false;
        }
        if (FrequencyMode == PropFrequencyMode.EveryNNodes) {
            bool spawn = (nodesSinceSpawn >= nodesBetweenSpawns);
            if (spawn) {
                DebugSpawn(spawn, $"{nodesSinceSpawn} >= {nodesBetweenSpawns}");
                return true;
            } else {
                DebugSpawn(spawn, $"{nodesSinceSpawn} > {nodesBetweenSpawns}");
            }
        }
        if (FrequencyMode == PropFrequencyMode.Chance) {
            float randomPercentage = Random.Range(0f, 1f) * 100;
            bool spawn = (ChancePercentage >= randomPercentage);
            if (spawn) {
                DebugSpawn(spawn, $"{ChancePercentage}% >= {randomPercentage}%");
                return true;
            } else {
                DebugSpawn(spawn, $"{ChancePercentage}% < {randomPercentage}%");
            }
        }
        return false;
    }

    private void Spawn(Spline road, Transform container) {
        ResetSpawnRate();
        spawned++;

        for (int index = 0; index < Amount; index += 1) {
            SpawnSingleProp(road, container);
        }
    }

    private CurveSample GetSample(Spline road, Vector3 offset) {
        float sampleDistance = road.Length - GenerateRoad.main.StepDistance / 2 + offset.z;
        return road.GetSampleAtDistance(Mathf.Clamp(sampleDistance, 0, road.Length));
    }

    private void SpawnSingleProp(Spline road, Transform container) {
        Vector3 offset = GetOffsetPosition();
        CurveSample sample = GetSample(road, offset);
        GameObject newProp = GameObject.Instantiate(Prefab, container);
        newProp.transform.localPosition = CalculatePosition(offset, sample);

        Quaternion rotation = GetRotation(sample, newProp.transform);
        newProp.transform.rotation = rotation;

        if (debug) {
            UnityEngine.Debug.DrawLine(sample.location, sample.location + Vector3.up * 10.0f, Color.red, 300000000.0f);
        }
    }

    private Vector3 CalculatePosition(Vector3 offset, CurveSample sample) {
        Vector3 binormal = (Quaternion.LookRotation(sample.tangent, sample.up) * Vector3.right).normalized;
        binormal *= offset.x;
        Vector3 pos = sample.location + binormal;
        pos.y = offset.y;
        return pos;
    }

    private int spawned = 0;
    private int nodesSinceSpawn = 0;
    private int nodesBetweenSpawns = 0;
    public GameObject Prefab;

    [Header("Frequency")]
    public PropFrequencyMode FrequencyMode = PropFrequencyMode.EveryNNodes;
    public Vector2Int EveryNNodesRange = new Vector2Int(0, 0);

    [Range(1, 100)]
    public int ChancePercentage = 100;

    [Header("Spawn size")]
    public int Limit = 0;
    public int Amount = 1;


    [Header("Rotation")]
    public PropRotationMode RotationMode;


    [Header("Position")]
    public Vector2 YPosRange = new Vector2(0, 0);
    public Vector2 OffsetXRange = new Vector2(-1, 1);
    public Vector2 OffsetZRange = new Vector2(-1, 1);

    public Quaternion GetRotation(CurveSample sample, Transform newObject) {
        if (RotationMode == PropRotationMode.FaceRoad) {
            Vector3 target = new Vector3(sample.location.x, 0f, sample.location.z);
            Vector3 origin = new Vector3(newObject.position.x, 0f, newObject.position.z);
            return Quaternion.LookRotation(target - origin, Vector3.up);
        }
        return Quaternion.Euler(0f, Random.Range(0, 360), 0f);
    }

    private Vector3 GetOffsetPosition () {
        return new Vector3(
            Random.Range(OffsetXRange.x, OffsetXRange.y),
            Random.Range(YPosRange.x, YPosRange.y),
            Random.Range(OffsetZRange.x, OffsetZRange.y)
        );
    }
}

public enum PropFrequencyMode {
    EveryNNodes,
    Chance
}

public enum PropRotationMode {
    FaceRoad,
    Random
}