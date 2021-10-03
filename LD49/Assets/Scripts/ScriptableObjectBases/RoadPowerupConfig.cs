using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


[CreateAssetMenu(fileName = "RoadPowerupConfig", menuName = "Configs/New RoadPowerupConfig")]
public class RoadPowerupConfig : ScriptableObject
{

    public int SectionDurationInNodes = 10;
    private int nodeCount = 0;

    public List<PropSpawn> Props = new List<PropSpawn>();
    public bool IsFinished() {
        return nodeCount > SectionDurationInNodes;
    }

    public void Init(bool debug) {
        nodeCount = 0;
        foreach(PropSpawn propSpawn in Props) {
            propSpawn.Init(debug);
        }
    }

    public void NewNodeWasAdded(Spline road, Transform container) {
        nodeCount++;
        foreach(PropSpawn propSpawn in Props) {
            propSpawn.NewNode(road, container);
        }
    }
}