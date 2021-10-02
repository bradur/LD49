using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRoad : MonoBehaviour
{
    public static GenerateRoad main;
    private void Awake() {
        main = this;
    }

    [SerializeField]
    private Spline spline;

    [SerializeField]
    private RoadSmoother roadSmoother;

    [SerializeField]
    private Transform roadTurner;

    [Range(0.1f, 10f)]
    [SerializeField]
    private float newNodeInterval = 2f;

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
    [Range(2, 20)]
    private int numberOfRoadNodesAtStart = 5;

    public void Begin()
    {
        for (int index = 0; index < numberOfRoadNodesAtStart; index += 1)
        {
            CreateMoreRoad(false);
        }
        roadSmoother.SmoothAll();
        RemoveRedundantStartNodes();
        MoveAlongRoad.main.UpdatePosition();
    }

    private void RemoveRedundantStartNodes()
    {
        spline.RemoveNode(spline.nodes[0]);
        spline.RemoveNode(spline.nodes[1]);
    }

    private void SetupRoadTurner() {
        SplineNode prevNode = GetCurrentNode();
        roadTurner.position = prevNode.Position;
        Vector3 firstPos = spline.nodes[0].Position;
        Vector3 secondPos = spline.nodes[1].Position;
        roadTurner.rotation = Quaternion.LookRotation(firstPos, secondPos);
        nodesUntilTurn = Random.Range(turnFrequency.x, turnFrequency.y);
    }

    private float AddNode(Vector3 position, Vector3 direction)
    {
        SplineNode prevNode = spline.nodes[spline.nodes.Count - 1];
        Vector3 newPos = prevNode.Position + position;
        newPos.y = 0f;

        SplineNode node = new SplineNode(newPos, direction);
        spline.AddNode (node);
        return Mathf.Abs(Vector3.Distance(prevNode.Position, newPos));
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
        spline.AddNode (node);
        if (smoothing) {
            SplineNode newlyAddedNode = GetCurrentNode();
            roadSmoother.SmoothNode (newlyAddedNode);
        }
        if (removeOldNodes && spline.nodes.Count > maxNodes)
        {
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

    private void Update()
    {
        if (!makeNewRoad)
        {
            return;
        }
        newNodeTimer += Time.deltaTime;
        if (newNodeTimer >= newNodeInterval)
        {
            CreateMoreRoad();
            newNodeTimer = 0f;
        }
    }
}
