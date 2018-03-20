using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ZoneClass
{
    public string clientId;
    public string siteId;
    public string zoneId;
    public string name;
    public string color;
    public int level;
    public double height;
    public double extrudedHeight;
    public double transparency;
    public double area;
    public double volume;
    public string type;
    public PointsClass[] points;

    public void transformPointsToLocal()
    {
        for (int i = 0; i < this.points.Length; i++)
        {
            double localY = CoordinateConverter.ConvertLatitudeToLocalY(this.points[i].y, this.points[i].x);
            double localX = CoordinateConverter.ConvertLongitudeToLocalX(this.points[i].x, localY);
            this.points[i].x = localX;
            this.points[i].y = localY;
        }
    }

    public GameObject[] GenerateWalls(float height, float width)
    {
        GameObject[] walls = new GameObject[points.Length - 1];
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 p1 = new Vector3((float)points[i].x, 0, (float)points[i].y);
            Vector3 p2 = new Vector3((float)points[i + 1].x, 0, (float)points[i + 1].y);
            float angle = Vector3.Angle(p2 - p1, Vector3.left);
            float distance = Vector3.Distance(p1, p2);
            Vector3 midpoint = new Vector3((p1.x + p2.x) / 2, height / 2, (p1.z + p2.z) / 2);
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.localScale = new Vector3(distance, height, width);
            wall.transform.Rotate(new Vector3(0, angle, 0));
            wall.transform.position = midpoint;
            walls[i] = wall;
        }
        return walls;
    }
  
    public GameObject GenerateFloor(float height, float depth)
    {
        Vector2 biggest = Vector2.zero;
        Vector2 smallest = Vector2.zero;

        foreach (PointsClass point in this.points) {
            if (point.x > biggest.x)
            {
                biggest.x = (float)point.x;
            }
            if (point.x < smallest.x)
            {
                smallest.x = (float)point.x;
            }
            if (point.y > biggest.y)
            {
                biggest.y = (float)point.y;
            }
            if (point.y < smallest.y)
            {
                smallest.y = (float)point.y;
            }
        }

        float length = biggest.x - smallest.x;
        float width = biggest.y - smallest.y;
        Vector3 midpoint = new Vector3((biggest.x + smallest.x) / 2, height, (biggest.y + smallest.y) / 2);
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(length, depth, width);
        floor.transform.position = midpoint;

        return floor;
    }
}

[Serializable]
public class PointsClass
{
    public double x;
    public double y;
}
