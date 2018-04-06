using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadMaterials : MonoBehaviour {
    public GameObject palletAndBoxes;

    private MaterialClass[] materials;

    // Hendersonville conversion factors
    private const double orientationOffset = -51.4; // orientation of the coordinate plane in relation to the Earth
    private const double metersPerDegreeLatitude = 110945.6224; // Total meters between each degree latitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double metersPerDegreeLongitude = 90982.115536; // Total meters between each degree longitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double latitudeZero = 35.274601; // The latitude coordinate that corresponds to the origin of the coordinate plane
    private const double longitudeZero = -82.412102; // The longitude coordinate that corresponds to the origin on the coordinate plane

    // Use this for initialization
    void Start () {
        StartCoroutine(GetMaterials());
	}

    IEnumerator GetMaterials()
    {
        UnityWebRequest materialService = UnityWebRequest.Get("https://mms-material-service-stg.run.aws-usw02-pr.ice.predix.io/api/v2/clients/GELighting/sites/101/materials/views/lastlocation?status=active");
        materialService.SetRequestHeader("Content-Type", "application/json");
		materialService.SetRequestHeader("Authorization", Authorization.getToken());
        yield return materialService.SendWebRequest();

        if (materialService.isNetworkError || materialService.isHttpError)
        {
            Debug.Log(materialService.error);
        }
        else
        {
            materials = JsonHelper.FromJson<MaterialClass>(JsonHelper.FixJson(materialService.downloadHandler.text));
            foreach (MaterialClass material in materials)
            {
                double localZ = CoordinateConversions.ConvertLatitudeToLocalY(material.y, material.x, orientationOffset, metersPerDegreeLatitude, metersPerDegreeLongitude, latitudeZero, longitudeZero);
                double localX = CoordinateConversions.ConvertLongitudeToLocalX(material.x, localZ, orientationOffset, metersPerDegreeLongitude, longitudeZero);
                GameObject materialInstance = Instantiate(palletAndBoxes, new Vector3((float)localX, (float)0.05420906, (float)localZ), Quaternion.Euler(-90, 0, 0));
                materialInstance.GetComponent<MaterialDetails>().MaterialId = material.materialId;
            }
        }
    }
}
