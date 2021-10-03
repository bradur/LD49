using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerMeter : MonoBehaviour
{
    public static BeerMeter main;
    private void Awake() {
        main = this;
    }

    [SerializeField]
    private float foamTopPos;

    [SerializeField]
    private float foamBottomPos;

    [SerializeField]
    private Image foam;

    [SerializeField]
    private Image beer;

    private float amount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fPos = foam.rectTransform.localPosition;
        foam.rectTransform.localPosition = new Vector3(fPos.x, Mathf.Lerp(foamBottomPos, foamTopPos, amount), fPos.z);
        beer.fillAmount = amount;
    }

    public void SetAmount(float amount) {
        this.amount = Mathf.Clamp(amount, 0f, 1f);
    }
}
