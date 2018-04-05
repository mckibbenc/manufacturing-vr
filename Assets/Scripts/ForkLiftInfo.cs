using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ForkLiftInfo : VRTK_InteractableObject
{
    private CarrierDetails details;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        Debug.Log("StartUsing..............");
        Debug.Log(details.carrierId);
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
    }

    protected override void Awake()
    {
        details = gameObject.GetComponent<CarrierDetails>();
        Debug.Log("Awake.....................");

        interactableRigidbody = GetComponent<Rigidbody>();
        if (interactableRigidbody != null)
        {
            interactableRigidbody.maxAngularVelocity = float.MaxValue;
        }

        if (disableWhenIdle && enabled && IsIdle())
        {
            startDisabled = true;
            enabled = false;
        }
    }
}

