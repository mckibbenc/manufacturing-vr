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

	// URL
	private const string baseUrl = "https://mms-site-service-stg.run.aws-usw02-pr.ice.predix.io";
	private const string apiVersion = "/api/v2";

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
		UnityWebRequest siteService = UnityWebRequest.Get(baseUrl + apiVersion + "/clients/GELighting/sites/101/zones?type=-fixture,-consume,-assembly");
        siteService.SetRequestHeader("Content-Type", "application/json");
		siteService.SetRequestHeader("Authorization", Authorization.getToken());

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
					zone.transformPointsToLocal(CoordinateConverter.ORIENTATION_OFFSET, CoordinateConverter.METERS_PER_DEGREE_LATITUDE, CoordinateConverter.METERS_PER_DEGREE_LONGITUDE, CoordinateConverter.LATITUDE_ZERO, CoordinateConverter.LONGITUDE_ZERO);

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
		UnityWebRequest siteServiceDev = UnityWebRequest.Get("https://mms-site-service-stg.run.aws-usw02-pr.ice.predix.io" + apiVersion + "/clients/GELighting/sites/101/zones?type=fixture");
        siteServiceDev.SetRequestHeader("Content-Type", "application/json");
		siteServiceDev.SetRequestHeader("Authorization", Authorization.getToken());

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
				zone.transformPointsToLocal(CoordinateConverter.ORIENTATION_OFFSET, CoordinateConverter.METERS_PER_DEGREE_LATITUDE, CoordinateConverter.METERS_PER_DEGREE_LONGITUDE, CoordinateConverter.LATITUDE_ZERO, CoordinateConverter.LONGITUDE_ZERO);

                float fixtureWidth = fixturePrefab.transform.localScale.x; // meters
                float fixtureHeight = fixturePrefab.transform.localScale.z; // meters

                Vector3 location = new Vector3((float)(zone.points[0].x + fixtureWidth / 2), (float)zone.height, (float)(zone.points[0].y + fixtureHeight / 2));
                GameObject instance = (GameObject)Instantiate(fixturePrefab, location, Quaternion.identity);
                instance.isStatic = true;
                instance.transform.parent = root.transform;
            }
            StaticBatchingUtility.Combine(root);
        }
    }
}
