using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{

    [SerializeField]
    private bool followX;
    [SerializeField]
    private bool followY;
    [SerializeField]
    private bool followZ;

    [SerializeField]
    private Transform target;

    private Vector3 originalPosition;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = originalPosition;
        if (followX)
        {
            newPosition.x = originalPosition.x + target.position.x;
        }
        if (followY)
        {
            newPosition.y = originalPosition.y + target.position.y;
        }
        if (followZ)
        {
            newPosition.z = originalPosition.z + target.position.z;
        }
        transform.position = newPosition;
    }
}
