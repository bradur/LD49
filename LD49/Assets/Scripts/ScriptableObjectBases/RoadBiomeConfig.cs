using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


[CreateAssetMenu(fileName = "RoadBiomeConfig", menuName = "Configs/New RoadBiomeConfig")]
public class RoadBiomeConfig : ScriptableObject
{

    public int BiomeDurationInNodes = 10;
    private int nodeCount = 0;

    [SerializeField]
    private AudioClip music;

    public List<PropSpawn> Props = new List<PropSpawn>();
    public bool IsFinished() {
        return nodeCount > BiomeDurationInNodes;
    }

    public void Init(bool debug) {
        nodeCount = 0;
        foreach(PropSpawn propSpawn in Props) {
            propSpawn.Init(debug);
        }
        if (music != null) {
            MusicPlayer.main.PlayMusic(music);
        }
    }

    public void NewNodeWasAdded(Spline road, Transform container) {
        nodeCount++;
        foreach(PropSpawn propSpawn in Props) {
            propSpawn.NewNode(road, container);
        }
    }
}