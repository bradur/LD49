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

    [SerializeField]
    private Image beerHilight;

    [SerializeField]
    private Image bubblesMask;

    [SerializeField]
    private RawImage bubbles;

    private float amount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Rect uv = bubbles.uvRect;
        float newY = 100f;
        bubbles.uvRect = new Rect(uv.x, newY, uv.width, uv.height);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fPos = foam.rectTransform.localPosition;
        foam.rectTransform.localPosition = new Vector3(fPos.x, Mathf.Lerp(foamBottomPos, foamTopPos, amount), fPos.z);
        beer.fillAmount = amount;
        beerHilight.fillAmount = amount;
        bubblesMask.fillAmount = amount;
        updateBubbles();
    }

    private void updateBubbles() {
        Rect uv = bubbles.uvRect;
        float newY = (uv.y - Time.deltaTime * 0.1f) % 2;
        bubbles.uvRect = new Rect(uv.x, newY, uv.width, uv.height);
    }

    public void SetAmount(float amount) {
        this.amount = Mathf.Clamp(amount, 0f, 1f);
    }
}
