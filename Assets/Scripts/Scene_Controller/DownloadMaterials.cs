using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadMaterials : MonoBehaviour {
    public Transform palletAndBoxes;

    private MaterialClass[] materials;

	// URL
	private const string baseUrl = "https://mms-material-service-stg.run.aws-usw02-pr.ice.predix.io";
	private const string apiVersion = "/api/v2";


	private long previousTime;

    // Use this for initialization
    void Start () {
        StartCoroutine(GetMaterials());
		//InvokeRepeating ("InvokeGetLatestChanges", 6.0f, 6.0f);
	}

    IEnumerator GetMaterials()
    {
		UnityWebRequest materialService = UnityWebRequest.Get(baseUrl + apiVersion + "/clients/GELighting/sites/101/materials/views/lastlocation?status=active");
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
                double localZ = CoordinateConverter.ConvertLatitudeToLocalY(material.y, material.x);
                double localX = CoordinateConverter.ConvertLongitudeToLocalX(material.x, localZ);
                Instantiate(palletAndBoxes, new Vector3((float)localX, (float)0.05420906, (float)localZ), Quaternion.Euler(-90, 0, 0));
            }
        }
    }

	void InvokeGetLatestChanges() 
	{
		StartCoroutine (getLatestChanges ());
	}

	IEnumerator GetLatestChanges(long previousTime)
	{
		UnityWebRequest materialService = UnityWebRequest.Get (baseUrl + apiVersion + "/clients/GELighting/sites/101/materials/views/lastlocation?status=active&modifiedStart>" + previousTime);
		materialService.SetRequestHeader ("Content-Type", "application/json");
		materialService.SetRequestHeader ("Authorization", Authorization.getToken ());
		previousTime = (DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds;
		yield return materialService.SendWebRequest();

		if (materialService.isNetworkError || materialService.isHttpError) {
			Debug.Log (materialService.error);
		} else {
			// 
			materials = JsonHelper.FromJson<MaterialClass> (JsonHelper.FixJson (materialService.downloadHandler.text));
			foreach (MaterialClass material in materials) {
				double localZ = CoordinateConverter.ConvertLatitudeToLocalY (material.y, material.x);
				double localX = CoordinateConverter.ConvertLongitudeToLocalX (material.x, localZ);
				Instantiate (palletAndBoxes, new Vector3 ((float)localX, (float)0.05420906, (float)localZ), Quaternion.Euler (-90, 0, 0));
			}
		}
	}
}
