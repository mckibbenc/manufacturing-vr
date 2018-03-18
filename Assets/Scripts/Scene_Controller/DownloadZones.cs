using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadZones : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject ceilingPrefab;
    public GameObject fixturePrefab;
    [SerializeField]
    public bool downloadBorders;
    [SerializeField]
    public bool downloadFixtures;


    private ZoneClass[] zones;

    // Hendersonville conversion factors
    private const double orientationOffset = -51.4; // orientation of the coordinate plane in relation to the Earth
    private const double metersPerDegreeLatitude = 110945.6224; // Total meters between each degree latitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double metersPerDegreeLongitude = 90982.115536; // Total meters between each degree longitude. For best accuracy, this constant is dependent on altitude and position on earth
    private const double latitudeZero = 35.274601; // The latitude coordinate that corresponds to the origin of the coordinate plane
    private const double longitudeZero = -82.412102; // The longitude coordinate that corresponds to the origin on the coordinate plane

    private const string token = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6ImxlZ2FjeS10b2tlbi1rZXkiLCJ0eXAiOiJKV1QifQ.eyJqdGkiOiJmYTNjYTM4MGE1Yzg0OGFiOWZmNGYwYjNmMDFkNTFiZCIsInN1YiI6IjdiNGU3MTM0LWRhZWItNGI5NC1hZjA2LWNkZjZhNWEyMmFkYiIsInNjb3BlIjpbInBhc3N3b3JkLndyaXRlIiwicHJlZGl4LWV2ZW50LWh1Yi56b25lcy40MGQ5Y2NjZS1iOTQ4LTRjMzMtODQ3OC0wNWU3OTJlYjVjNzgudXNlciIsInByZWRpeC1ldmVudC1odWIuem9uZXMuNDBkOWNjY2UtYjk0OC00YzMzLTg0NzgtMDVlNzkyZWI1Yzc4LmdycGMucHVibGlzaCJdLCJjbGllbnRfaWQiOiJtbXMiLCJjaWQiOiJtbXMiLCJhenAiOiJtbXMiLCJncmFudF90eXBlIjoicGFzc3dvcmQiLCJ1c2VyX2lkIjoiN2I0ZTcxMzQtZGFlYi00Yjk0LWFmMDYtY2RmNmE1YTIyYWRiIiwib3JpZ2luIjoidWFhIiwidXNlcl9uYW1lIjoiSW50ZWdyYXRpb25UZXN0VXNlciIsImVtYWlsIjoiZGFycmVsbC50aG9iZUBnZS5jb20iLCJhdXRoX3RpbWUiOjE1MjA2MzcyNDIsInJldl9zaWciOiI1OGI2NmJiYSIsImlhdCI6MTUyMDYzNzI0MiwiZXhwIjoxNTI3ODM3MjMxLCJpc3MiOiJodHRwczovLzFiY2JiYTg3LTIyOGEtNDAyNi1hMTk3LWM4M2YzYzZkZDFkMi5wcmVkaXgtdWFhLnJ1bi5hd3MtdXN3MDItcHIuaWNlLnByZWRpeC5pby9vYXV0aC90b2tlbiIsInppZCI6IjFiY2JiYTg3LTIyOGEtNDAyNi1hMTk3LWM4M2YzYzZkZDFkMiIsImF1ZCI6WyJtbXMiLCJwYXNzd29yZCIsInByZWRpeC1ldmVudC1odWIuem9uZXMuNDBkOWNjY2UtYjk0OC00YzMzLTg0NzgtMDVlNzkyZWI1Yzc4IiwicHJlZGl4LWV2ZW50LWh1Yi56b25lcy40MGQ5Y2NjZS1iOTQ4LTRjMzMtODQ3OC0wNWU3OTJlYjVjNzguZ3JwYyJdfQ.lClTEZB-o3FT-ikLeFyOSxiVIL76L5L7OwALWO-jAAhnmg8eKVVMar4VmuOuoNW5lRFuCSsQz7ouauQZwSt5WmD1m69gQD0n_DGSAJHfvpJ5CZuj32Hkut5Eeom8ekQIQRNHcQF_r9J5sN5APibT95MDysq1xVyA6XLagndopoFXEq0zahyzhmlNu7vRpIFAF5_8cgTVQovFhU85VZJgtERkGOg27qGaDmi8CzNJWUlpdw8ECFZPCWHEH3FEpp_JwTzgaZg8YUmYnFB1aaRFAd7w8rMI1w7z7bI4DkC3EukXUQsFxdKAhT7ZjAqYKBanku7tNyGTUlzAeNNfPpasGg";


    // Use this for initialization
    void Start()
    {
        if (downloadBorders)
        {
            StartCoroutine(GetZones());
        }
        if (downloadFixtures)
        {
            StartCoroutine(GetFixtures());
        }
    }

    IEnumerator GetZones()
    {
        UnityWebRequest siteService = UnityWebRequest.Get("https://mms-site-service-stg.run.aws-usw02-pr.ice.predix.io/api/v2/clients/GELighting/sites/101/zones");
        siteService.SetRequestHeader("Content-Type", "application/json");
        siteService.SetRequestHeader("Authorization", token);

        yield return siteService.SendWebRequest();

        if (siteService.isNetworkError || siteService.isHttpError)
        {
            Debug.Log(siteService.error);
        }
        else
        {
            zones = JsonHelper.FromJson<ZoneClass>(JsonHelper.FixJson(siteService.downloadHandler.text));
            foreach (ZoneClass zone in zones)
            {
                if (zone.name == "main")
                {
                    zone.transformPointsToLocal(orientationOffset, metersPerDegreeLatitude, metersPerDegreeLongitude, latitudeZero, longitudeZero);

                    Rect boundaries = new Rect(0, 0, (float)zone.points[1].x, (float)zone.points[2].y);
                    Vector3 location = new Vector3(boundaries.width / 2, 0, boundaries.height / 2);
                    //GameObject newFloor = Instantiate(floorPrefab, location, Quaternion.identity) as GameObject;
                    //newFloor.transform.localScale = new Vector3(boundaries.width, 0.1f, boundaries.height);

                    // Generate walls
                    GameObject[] walls = zone.GenerateWalls(9, 0.1f);
                    foreach(GameObject w in walls)
                    {
                        GameObject wall = Instantiate(this.wallPrefab, w.transform.position, w.transform.localRotation);
                        wall.transform.localScale = w.transform.localScale;
                        Destroy(w); // We only need the temporary wall to modify the transform of the prefab wall
                    }

                    // Generate floor
                    GameObject f = zone.GenerateFloor(0, 0.1f);
                    GameObject floor = Instantiate(this.floorPrefab, f.transform.position, Quaternion.identity);
                    floor.transform.localScale = f.transform.localScale;
                    Destroy(f); // We only need the temporary floor to modify the transform of the prefab floor

                    // Generate ceiling
                    GameObject c = zone.GenerateFloor(9, 0.1f);
                    GameObject ceiling = Instantiate(this.ceilingPrefab, c.transform.position, Quaternion.identity);
                    ceiling.transform.localScale = c.transform.localScale;
                    Destroy(c); // We only need the temporary ceiling to modify the transform of the prefab ceiling
                }
            }
        }
    }

    IEnumerator GetFixtures()
    {
        UnityWebRequest siteServiceDev = UnityWebRequest.Get("https://mms-site-service-dev.run.aws-usw02-pr.ice.predix.io/api/v2/clients/GELighting/sites/101/zones");
        siteServiceDev.SetRequestHeader("Content-Type", "application/json");
        siteServiceDev.SetRequestHeader("Authorization", token);

        yield return siteServiceDev.SendWebRequest();

        if (siteServiceDev.isNetworkError || siteServiceDev.isHttpError)
        {
            Debug.Log(siteServiceDev.error);
        }
        else
        {
            ZoneClass[] zones = JsonHelper.FromJson<ZoneClass>(JsonHelper.FixJson(siteServiceDev.downloadHandler.text));
            GameObject root = new GameObject();
            foreach (ZoneClass zone in zones)
            {
                if (zone.type == "fixture")
                {
                    zone.transformPointsToLocal(orientationOffset, metersPerDegreeLatitude, metersPerDegreeLongitude, latitudeZero, longitudeZero);

                    
                    float fixtureWidth = fixturePrefab.transform.localScale.x; // meters
                    float fixtureHeight = fixturePrefab.transform.localScale.z; // meters

                    Vector3 location = new Vector3((float)(zone.points[0].x + fixtureWidth / 2), (float)zone.height, (float)(zone.points[0].y + fixtureHeight / 2));
                    GameObject instance = (GameObject)Instantiate(fixturePrefab, location, Quaternion.identity);
                    instance.isStatic = true;
                    instance.transform.parent = root.transform;
                }
            }
            StaticBatchingUtility.Combine(root);
        }
    }
}
