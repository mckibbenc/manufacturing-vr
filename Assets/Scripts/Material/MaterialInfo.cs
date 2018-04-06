using VRTK;
using UnityEngine;

public class MaterialInfo : VRTK_InteractableObject
{

    private MaterialDetails details;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        TabletController tablet = GameObject.Find(string.Format("Tablet({0})", usingObject.transform.parent.name)).GetComponent<TabletController>();
        tablet.setObjectType("Pallet");
        tablet.setLabel1("ID");
        tablet.setValue1(details.MaterialId);
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
    }

    protected override void Awake()
    {
        details = gameObject.GetComponent<MaterialDetails>();

        if (disableWhenIdle && enabled && IsIdle())
        {
            startDisabled = true;
            enabled = false;
        }
    }
}
