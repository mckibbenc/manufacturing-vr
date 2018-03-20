using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadCarriers : MonoBehaviour
{
    public Transform forklift;

    private CarrierLocationClass[] carriers;

	// URL
	private const string baseUrl = "https://mms-carrier-service-stg.run.aws-usw02-pr.ice.predix.io";
	private const string apiVersion = "/api/v2";

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetCarriers());
    }

    IEnumerator GetCarriers()
    {
		UnityWebRequest carrierService = UnityWebRequest.Get(baseUrl + apiVersion + "/clients/GELighting/sites/101/carriers/locations/latest?active=true");
        carrierService.SetRequestHeader("Content-Type", "application/json");
		carrierService.SetRequestHeader("Authorization", Authorization.getToken());
        yield return carrierService.SendWebRequest();

        if (carrierService.isNetworkError || carrierService.isHttpError)
        {
            Debug.Log(carrierService.error);
        }
        else
        {
            carriers = JsonHelper.FromJson<CarrierLocationClass>(JsonHelper.FixJson(carrierService.downloadHandler.text));
            foreach (CarrierLocationClass carrier in carriers)
            {
                double localZ = CoordinateConverter.ConvertLatitudeToLocalY(carrier.y, carrier.x);
                double localX = CoordinateConverter.ConvertLongitudeToLocalX(carrier.x, localZ);
                Instantiate(forklift, new Vector3((float)localX, (float)0.06177858, (float)localZ), Quaternion.Euler(0, (float)(carrier.orientation + 90 + 40), 0));
            }
        }
    }
}
