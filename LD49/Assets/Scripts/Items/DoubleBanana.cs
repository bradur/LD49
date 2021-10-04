using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBanana : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> bananas;

    [SerializeField]
    private List<Vector3> possiblePositions;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bananas.Count; i++)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            Vector3 pos = possiblePositions[randomIndex];
            possiblePositions.RemoveAt(randomIndex);
            bananas[i].transform.localPosition = pos;
        }
        for (int i = 0; i < bananas.Count; i++)
        {
            Debug.Log("Bansku pos " + i + ": " + bananas[i].transform.localPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
