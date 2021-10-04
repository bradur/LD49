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

    [SerializeField]
    private float rotationSpeed = 1f;

    private bool rotationInProgress = false;
    private float rotationTimer = 0f;
    private Quaternion originalRotation;
    private Quaternion targetrotation;

    [Range(1, 10)]
    [SerializeField]
    private int startingNode = 1;

    private float rateAllTime = 0f;

    public void SetInitialPosition() {
        rate = GenerateRoad.main.StepDistance * startingNode;
        rate = Mathf.Clamp(rate, 0f, road.Length);
        CurveSample sample = road.GetSampleAtDistance(rate);

        transform.localPosition = sample.location;
        transform.rotation = sample.Rotation;
    }
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
        rate = GenerateRoad.main.StepDistance * startingNode;
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
        GraduallyRotate();
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
        float rateAdd = Time.deltaTime * speed;
        rate += rateAdd;
        rateAllTime += rateAdd;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        rate = Mathf.Clamp(rate, 0f, road.Length);
        CurveSample sample = road.GetSampleAtDistance(rate);

        transform.localPosition = sample.location;
        rotationInProgress = true;
        rotationTimer = 0f;
        targetrotation = sample.Rotation;
        originalRotation = transform.rotation;
    }

    public void GraduallyRotate() {
        if (rotationInProgress) {
            rotationTimer += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(originalRotation, targetrotation, rotationTimer);
            if (rotationTimer >= 1f) {
                rotationInProgress = false;
            }
        }
    }

    public string GetTravelDistance() {
        //float steps = (rate - GenerateRoad.main.StepDistance * startingNode) / GenerateRoad.main.StepDistance;
        return (rateAllTime / GenerateRoad.main.StepDistance / 100.0f).ToString("F2");
    }
}
