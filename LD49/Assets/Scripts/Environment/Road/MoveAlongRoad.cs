using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class MoveAlongRoad : MonoBehaviour
{
    public static MoveAlongRoad main;
    private void Awake() {
        main = this;
    }

    [SerializeField]
    private Spline road;

    [SerializeField]
    private bool allowMove = false;

    [SerializeField]
    private float speed = 1f;


    public void Begin() {
        allowMove = true;
    }

    public void Stop() {
        allowMove = false;
    }

    void Update()
    {
        if (!allowMove && road != null)
        {
            return;
        }
        Move();
    }

    private float rate = 1f;

    private void Move()
    {
        rate += Time.deltaTime * speed;
        UpdatePosition();
    }

    public void UpdatePosition() {
        CurveSample sample = road.GetSampleAtDistance(rate);

        transform.localPosition = sample.location;
        transform.rotation = sample.Rotation;
    }
}
