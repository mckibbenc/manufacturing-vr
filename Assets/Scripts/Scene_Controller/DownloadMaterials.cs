using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadMaterials : MonoBehaviour {
    public Transform palletAndBoxes;

    private MaterialClass[] materials;
    private const string token = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6ImxlZ2FjeS10b2tlbi1rZXkiLCJ0eXAiOiJKV1QifQ.eyJqdGkiOiJmYTNjYTM4MGE1Yzg0OGFiOWZmNGYwYjNmMDFkNTFiZCIsInN1YiI6IjdiNGU3MTM0LWRhZWItNGI5NC1hZjA2LWNkZjZhNWEyMmFkYiIsInNjb3BlIjpbInBhc3N3b3JkLndyaXRlIiwicHJlZGl4LWV2ZW50LWh1Yi56b25lcy40MGQ5Y2NjZS1iOTQ4LTRjMzMtODQ3OC0wNWU3OTJlYjVjNzgudXNlciIsInByZWRpeC1ldmVudC1odWIuem9uZXMuNDBkOWNjY2UtYjk0OC00YzMzLTg0NzgtMDVlNzkyZWI1Yzc4LmdycGMucHVibGlzaCJdLCJjbGllbnRfaWQiOiJtbXMiLCJjaWQiOiJtbXMiLCJhenAiOiJtbXMiLCJncmFudF90eXBlIjoicGFzc3dvcmQiLCJ1c2VyX2lkIjoiN2I0ZTcxMzQtZGFlYi00Yjk0LWFmMDYtY2RmNmE1YTIyYWRiIiwib3JpZ2luIjoidWFhIiwidXNlcl9uYW1lIjoiSW50ZWdyYXRpb25UZXN0VXNlciIsImVtYWlsIjoiZGFycmVsbC50aG9iZUBnZS5jb20iLCJhdXRoX3RpbWUiOjE1MjA2MzcyNDIsInJldl9zaWciOiI1OGI2NmJiYSIsImlhdCI6MTUyMDYzNzI0MiwiZXhwIjoxNTI3ODM3MjMxLCJpc3MiOiJodHRwczovLzFiY2JiYTg3LTIyOGEtNDAyNi1hMTk3LWM4M2YzYzZkZDFkMi5wcmVkaXgtdWFhLnJ1bi5hd3MtdXN3MDItcHIuaWNlLnByZWRpeC5pby9vYXV0aC90b2tlbiIsInppZCI6IjFiY2JiYTg3LTIyOGEtNDAyNi1hMTk3LWM4M2YzYzZkZDFkMiIsImF1ZCI6WyJtbXMiLCJwYXNzd29yZCIsInByZWRpeC1ldmVudC1odWIuem9uZXMuNDBkOWNjY2UtYjk0OC00YzMzLTg0NzgtMDVlNzkyZWI1Yzc4IiwicHJlZGl4LWV2ZW50LWh1Yi56b25lcy40MGQ5Y2NjZS1iOTQ4LTRjMzMtODQ3OC0wNWU3OTJlYjVjNzguZ3JwYyJdfQ.lClTEZB-o3FT-ikLeFyOSxiVIL76L5L7OwALWO-jAAhnmg8eKVVMar4VmuOuoNW5lRFuCSsQz7ouauQZwSt5WmD1m69gQD0n_DGSAJHfvpJ5CZuj32Hkut5Eeom8ekQIQRNHcQF_r9J5sN5APibT95MDysq1xVyA6XLagndopoFXEq0zahyzhmlNu7vRpIFAF5_8cgTVQovFhU85VZJgtERkGOg27qGaDmi8CzNJWUlpdw8ECFZPCWHEH3FEpp_JwTzgaZg8YUmYnFB1aaRFAd7w8rMI1w7z7bI4DkC3EukXUQsFxdKAhT7ZjAqYKBanku7tNyGTUlzAeNNfPpasGg";

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
        materialService.SetRequestHeader("Authorization", token);
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
                Instantiate(palletAndBoxes, new Vector3((float)localX, (float)0.05420906, (float)localZ), Quaternion.Euler(-90, 0, 0));
            }
        }
    }
}
