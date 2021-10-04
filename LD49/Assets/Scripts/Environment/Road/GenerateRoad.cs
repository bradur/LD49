using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRoad : MonoBehaviour
{
    public static GenerateRoad main;
    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Spline spline;
    [SerializeField]
    private RoadMeshTiling roadMesh;
    public RoadMeshTiling RoadMesh { get { return roadMesh; } }
    [SerializeField]
    private RoadMeshTiling groundMesh;
    public RoadMeshTiling GroundMesh { get { return groundMesh; } }

    public Spline Road { get { return spline; } }

    [SerializeField]
    private RoadSmoother roadSmoother;

    [SerializeField]
    private GameObject roadTurnerPrefab;
    private Transform roadTurner;

    private float newNodeTimer = 0f;

    [SerializeField]
    private bool makeNewRoad = true;

    [SerializeField]
    private float stepDistance = 1f;

    [SerializeField]
    private Vector2 rotationRange = new Vector2(-45f, 45f);

    [SerializeField]
    private bool removeOldNodes = false;

    [SerializeField]
    private int maxNodes = 10;

    [SerializeField]
    private Vector2Int turnFrequency = new Vector2Int(2, 10);

    private int nodesUntilTurn = 0;

    private int nodesAddedWithoutTurn = 0;

    private int currentDirection = 1;

    private int turnsInThisDirection = 0;

    [SerializeField]
    private int maxTurnsInOneDirection = 3;

    [SerializeField]
    [Range(2, 2000)]
    private int numberOfRoadNodesAtStart = 5;

    public float StepDistance { get { return stepDistance; } }

    public void Begin()
    {
        roadMesh.Begin();
        groundMesh.Begin();
        SetupRoadTurner();
        for (int index = 0; index < numberOfRoadNodesAtStart; index += 1)
        {
            CreateMoreRoad(true);
        }
        //roadSmoother.SmoothAll();
        RemoveRedundantStartNodes();
        MoveAlongRoad.main.UpdatePosition();
    }

    public void Stop()
    {
        makeNewRoad = false;
    }

    private void RemoveRedundantStartNodes()
    {
        spline.RemoveNode(spline.nodes[0]);
        spline.RemoveNode(spline.nodes[1]);
    }

    private void SetupRoadTurner()
    {
        if (roadTurner == null)
        {
            GameObject roadTurnerObject = Instantiate(roadTurnerPrefab);
            roadTurnerObject.transform.position = Vector3.zero;
            roadTurnerObject.transform.rotation = Quaternion.identity;
            roadTurner = roadTurnerObject.transform;
        }
        SplineNode prevNode = GetCurrentNode();
        roadTurner.position = prevNode.Position;
        SetTurnerDirection();
        float angle =
            Random.Range(rotationRange.x, rotationRange.y) * currentDirection;
        roadTurner.Rotate(0f, angle, 0f);
        nodesUntilTurn = Random.Range(turnFrequency.x, turnFrequency.y);
    }

    private SplineNode GetCurrentNode()
    {
        return spline.nodes[spline.nodes.Count - 1];
    }

    private void MoveRoadTurner()
    {
        roadTurner.position += roadTurner.forward * stepDistance;
    }

    private void AddNewNode(bool smoothing = true)
    {
        Vector3 newNodePos =
            new Vector3(roadTurner.position.x, 0f, roadTurner.position.z);
        SplineNode node = new SplineNode(newNodePos, roadTurner.forward);
        spline.AddNode(node);
        if (smoothing)
        {
            SplineNode newlyAddedNode = GetCurrentNode();
            roadSmoother.SmoothNode(newlyAddedNode);
        }
        if (removeOldNodes && spline.nodes.Count > maxNodes)
        {
            BiomeManager.main.DestroyProps(spline.nodes[0]);
            PowerupManager.main.DestroyProps(spline.nodes[0]);
            spline.RemoveNode(spline.nodes[0]);
        }
        nodesAddedWithoutTurn++;
    }

    private void SetTurnerDirection()
    {
        int newDirection = Random.Range(0, 2) == 1 ? 1 : -1;
        if (currentDirection != newDirection)
        {
            currentDirection = newDirection;
            turnsInThisDirection = 0;
        }
        else
        {
            if (turnsInThisDirection >= maxTurnsInOneDirection)
            {
                newDirection = -newDirection;
                currentDirection = newDirection;
                turnsInThisDirection = 0;
            }
            else
            {
                turnsInThisDirection++;
            }
        }
    }

    private void MakeATurn()
    {
        nodesAddedWithoutTurn = 0;
        nodesUntilTurn = Random.Range(turnFrequency.x, turnFrequency.y);
        SetTurnerDirection();
        float angle =
            Random.Range(rotationRange.x, rotationRange.y) * currentDirection;
        roadTurner.Rotate(0f, angle, 0f);
    }

    private void CreateMoreRoad(bool smoothing = true)
    {
        if (nodesAddedWithoutTurn >= nodesUntilTurn)
        {
            MakeATurn();
        }
        MoveRoadTurner();
        AddNewNode(smoothing);
    }

    private float CalculateSpeed()
    {
        return stepDistance / MoveAlongRoad.main.Speed;
    }

    private void Update()
    {
        if (!makeNewRoad)
        {
            return;
        }
        newNodeTimer += Time.deltaTime;
        if (newNodeTimer >= CalculateSpeed())
        {
            CreateMoreRoad();
            newNodeTimer = 0f;
        }
    }
}
