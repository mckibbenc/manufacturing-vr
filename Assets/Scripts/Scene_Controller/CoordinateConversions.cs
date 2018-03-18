using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinateConversions {

    public static double ConvertLatitudeToLocalY(double latitude, double longitude, double orientationOffset, double metersPerDegreeLatitude, double metersPerDegreeLongitude, double latitudeZero, double longitudeZero)
    {
        double rotation = convertToRadians(orientationOffset);
        double localY = ((latitude - latitudeZero) * metersPerDegreeLatitude * Math.Cos(rotation) - metersPerDegreeLongitude * (longitude - longitudeZero) * Math.Sin(rotation)) / (Math.Pow(Math.Cos(rotation), 2) + Math.Pow(Math.Sin(rotation), 2));
        return localY;
    }

    public static double ConvertLongitudeToLocalX(double longitude, double localY, double orientationOffset, double metersPerDegreeLongitude, double longitudeZero)
    {
        double rotation = convertToRadians(orientationOffset);
        double localX = (metersPerDegreeLongitude * (longitude - longitudeZero) + localY * Math.Sin(rotation)) / Math.Cos(rotation);
        return localX;
    }

    private static double convertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }
}
