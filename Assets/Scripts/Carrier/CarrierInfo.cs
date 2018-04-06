using VRTK;
using UnityEngine;

public class CarrierInfo : VRTK_InteractableObject
{
    private CarrierDetails details;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        TabletController tablet = GameObject.Find(string.Format("Tablet({0})", usingObject.transform.parent.name)).GetComponent<TabletController>();
        tablet.setObjectType("Forklift");
        tablet.setLabel1("ID");
        tablet.setValue1(details.CarrierId);
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
    }

    protected override void Awake()
    {
        details = gameObject.GetComponent<CarrierDetails>();

        if (disableWhenIdle && enabled && IsIdle())
        {
            startDisabled = true;
            enabled = false;
        }
    }
}

