using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFixtures : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fixture")
        {
            //other.gameObject.GetComponent<Renderer>().enabled = true;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fixture")
        {
            //other.gameObject.GetComponent<Renderer>().enabled = false;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            Debug.Log("fixture disabled");
        }
    }
}
