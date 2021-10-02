using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKOrchestrator : MonoBehaviour
{
    private LegIK[] legIKs;

    // Start is called before the first frame update
    void Start()
    {
        legIKs = GetComponents<LegIK>();
        foreach (var legIK in legIKs)
        {
            legIK.Orchestrator = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovingLeg(LegIK leg) {
        var allowed = leg == null;
        foreach (var legIK in legIKs)
        {
            if (legIK != leg) {
                legIK.AllowedToMove(allowed);
            }
        }
    }
}
