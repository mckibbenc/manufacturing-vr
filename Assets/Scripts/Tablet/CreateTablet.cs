using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTablet : MonoBehaviour {

    public GameObject tablet;

    private SteamVR_TrackedController device;
    private GameObject tabletInstance;

	void Awake () {
        device = GetComponent<SteamVR_TrackedController>();
        device.MenuButtonClicked += Tablet;
	}
	
	void Tablet (object sender, ClickedEventArgs e) {
        if (tabletInstance == null)
        {
            tabletInstance = Instantiate(this.tablet, device.transform);
            tabletInstance.name = string.Format("Tablet({0})", this.device.name);
            tabletInstance.transform.localPosition = new Vector3(0, 0.12f, 0.12f);
        }
        else
        {
            Destroy(tabletInstance);
        }
	}
}
