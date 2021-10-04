using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParentAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent AnimationEvent1;

    [SerializeField]
    private UnityEvent AnimationEvent2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Event1() {
        AnimationEvent1.Invoke();
    }

    public void Event2() {
        AnimationEvent2.Invoke();
    }
}
