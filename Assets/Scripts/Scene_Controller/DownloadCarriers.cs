using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadCarriers : MonoBehaviour
{
    public GameObject forklift;

    private CarrierLocationClass[] carriers;

    // Hendersonville conversion factors
    private const double orientationOffset = -51.4; // orientation of the coordinate plane in relation to the Earth
    private const double metersPerDegreeLatitude = 110945.6224; // Total meters between each degree latitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double metersPerDegreeLongitude = 90982.115536; // Total meters between each degree longitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double latitudeZero = 35.274601; // The latitude coordinate that corresponds to the origin of the coordinate plane
    private const double longitudeZero = -82.412102; // The longitude coordinate that corresponds to the origin on the coordinate plane

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetCarriers());
    }

    IEnumerator GetCarriers()
    {
        UnityWebRequest carrierService = UnityWebRequest.Get("https://mms-carrier-service-stg.run.aws-usw02-pr.ice.predix.io/api/v2/clients/GELighting/sites/101/carriers/locations/latest?active=true");
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
                double localZ = CoordinateConversions.ConvertLatitudeToLocalY(carrier.y, carrier.x, orientationOffset, metersPerDegreeLatitude, metersPerDegreeLongitude, latitudeZero, longitudeZero);
                double localX = CoordinateConversions.ConvertLongitudeToLocalX(carrier.x, localZ, orientationOffset, metersPerDegreeLongitude, longitudeZero);
                GameObject carrierInstance = Instantiate(forklift, new Vector3((float)localX, (float)0.06177858, (float)localZ), Quaternion.Euler(0, (float)(carrier.orientation + 90 + 40), 0));
                carrierInstance.transform.Find("forklift_truck").GetComponent<CarrierDetails>().CarrierId = carrier.carrierId;
            }
        }
    }
}
