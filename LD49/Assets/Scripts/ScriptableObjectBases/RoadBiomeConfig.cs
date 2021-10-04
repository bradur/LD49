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
    public AudioClip Music { get { return music; } }

    [SerializeField]
    private Material roadMaterial;
    public Material RoadMaterial { get { return roadMaterial; } }

    [SerializeField]
    private Material groundMaterial;
    public Material GroundMaterial { get { return groundMaterial; } }

    public List<PropSpawn> Props = new List<PropSpawn>();

    private bool firstSpawn = true;
    public bool IsFinished()
    {
        return nodeCount > BiomeDurationInNodes;
    }

    public void Init(bool debug)
    {
        firstSpawn = true;
        nodeCount = 0;
        foreach (PropSpawn propSpawn in Props)
        {
            propSpawn.Init(debug);
        }
        /*if (music != null)
        {
            MusicPlayer.main.PlayMusic(music);
        }*/
    }

    public void DestroyProps(SplineNode node)
    {
        foreach (PropSpawn propSpawn in Props)
        {
            propSpawn.DestroyProps(node);
        }
    }



    public void NewNodeWasAdded(ListChangedEventArgs<SplineNode> args, Spline road, Transform container)
    {
        if (args.newItems == null && args.newItems.Count < 1)
        {
            return;
        }
        if (firstSpawn)
        {
            firstSpawn = false;
            float sampleDistance = road.Length - GenerateRoad.main.StepDistance;
            CurveSample sample = road.GetSampleAtDistance(Mathf.Clamp(sampleDistance, 0, road.Length));
            GameObject trigger = Instantiate(GameManager.Main.TriggerPrefab);
            trigger.GetComponentInParent<MusicTrigger>().Config = this;

            Vector3 binormal = (Quaternion.LookRotation(sample.tangent, sample.up) * Vector3.right).normalized;
            Vector3 pos = sample.location + binormal;

            trigger.transform.localPosition = pos;
        }
        nodeCount++;
        foreach (PropSpawn propSpawn in Props)
        {
            propSpawn.NewNode(road, container, args.newItems[0]);
        }
    }
}