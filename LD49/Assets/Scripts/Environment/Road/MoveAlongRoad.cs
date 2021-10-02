using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class MoveAlongRoad : MonoBehaviour
{
    [SerializeField]
    private Spline road;

    [SerializeField]
    private bool AllowMove = true;

    [SerializeField]
    private float speed = 1f;

    void Update()
    {
        if (!AllowMove && road != null)
        {
            return;
        }
        Move();
    }

    private float rate = 0f;

    private void Move()
    {
        rate += Time.deltaTime * speed;

        CurveSample sample = road.GetSampleAtDistance(rate);

        transform.localPosition = sample.location;
        transform.rotation = sample.Rotation;

    }
}
