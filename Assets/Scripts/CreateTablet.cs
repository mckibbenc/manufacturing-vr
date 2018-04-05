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
	
	// Update is called once per frame
	void Tablet (object sender, ClickedEventArgs e) {
        Debug.Log("Clicked");
        if (tabletInstance == null)
        {
            Debug.Log("Instantiate");
            tabletInstance = Instantiate(tablet, device.transform);
            tabletInstance.transform.localPosition = new Vector3(0, 0.08f, 0.141f);
        }
        else
        {
            Debug.Log("Destroy");
            Destroy(tabletInstance);
        }
	}
}
