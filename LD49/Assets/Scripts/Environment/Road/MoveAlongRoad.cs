using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class MoveAlongRoad : MonoBehaviour
{
    public static MoveAlongRoad main;
    private void Awake()
    {
        main = this;
    }

    private Spline road;

    [SerializeField]
    private bool allowMove = false;

    [SerializeField]
    private float speed = 1f;

    public float Speed { get { return speed; } }

    private float rate = 0f;

    private bool firstCorrection = true;

    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private float debugRate = 0f;
    private float oldRoadLength = 0f;

    public void Begin()
    {
        allowMove = true;
    }

    public void Stop()
    {
        allowMove = false;
    }

    private void Start()
    {
        rate = GenerateRoad.main.StepDistance;
        road = GenerateRoad.main.Road;
        road.NodeListChanged += OnRoadLengthChange;
    }

    void Update()
    {
        if (debug) {
            debugRate = Mathf.Clamp(debugRate, 0, road.Length);
            CurveSample sample = road.GetSampleAtDistance(debugRate);

            transform.localPosition = sample.location;
            transform.rotation = sample.Rotation;
            return;
        }
        if (!allowMove && road != null)
        {
            return;
        }
        Move();
    }


    private void OnRoadLengthChange(object sender, ListChangedEventArgs<SplineNode> args)
    {
        if (allowMove && args.removedItems != null && args.removedItems.Count > 0)
        {
            if (firstCorrection) {
                firstCorrection = false;
                rate -= GenerateRoad.main.StepDistance * args.removedItems.Count;
            }
            rate -= GenerateRoad.main.StepDistance * args.removedItems.Count;
        }
        oldRoadLength = road.Length;
    }

    private void Move()
    {
        rate += Time.deltaTime * speed;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        rate = Mathf.Clamp(rate, 0f, road.Length);
        CurveSample sample = road.GetSampleAtDistance(rate);

        transform.localPosition = sample.location;
        transform.rotation = sample.Rotation;
    }
}
