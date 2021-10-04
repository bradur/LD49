using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScraper : MonoBehaviour
{
    private MeshRenderer rend;

    public Material material;

    [SerializeField]
    private List<Material> neonMaterials;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        material = neonMaterials[Random.Range(0, rend.materials.Length)];
        rend.material = material;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
