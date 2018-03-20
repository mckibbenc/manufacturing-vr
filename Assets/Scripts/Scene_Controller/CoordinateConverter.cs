using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinateConverter {

	// Hendersonville conversion factors
	public const double ORIENTATION_OFFSET = -51.4; // orientation of the coordinate plane in relation to the Earth
	public const double METERS_PER_DEGREE_LATITUDE = 110945.6224; // Total meters between each degree latitude. For best accuracy, this constant is dependent on altitude and position on earth
	public const double METERS_PER_DEGREE_LONGITUDE = 90982.115536; // Total meters between each degree longitude. For best accuracy, this constant is dependent on altitude and position on earth
	public const double LATITUDE_ZERO = 35.274601; // The latitude coordinate that corresponds to the origin of the coordinate plane
	public const double LONGITUDE_ZERO = -82.412102; // The longitude coordinate that corresponds to the origin on the coordinate plane

    public static double ConvertLatitudeToLocalY(double latitude, double longitude)
    {
        double rotation = convertToRadians(ORIENTATION_OFFSET);
        double localY = ((latitude - LATITUDE_ZERO) * METERS_PER_DEGREE_LATITUDE * Math.Cos(rotation) - METERS_PER_DEGREE_LONGITUDE * (longitude - LONGITUDE_ZERO) * Math.Sin(rotation)) / (Math.Pow(Math.Cos(rotation), 2) + Math.Pow(Math.Sin(rotation), 2));
        return localY;
    }

    public static double ConvertLongitudeToLocalX(double longitude, double localY)
    {
        double rotation = convertToRadians(ORIENTATION_OFFSET);
        double localX = (METERS_PER_DEGREE_LONGITUDE * (longitude - LONGITUDE_ZERO) + localY * Math.Sin(rotation)) / Math.Cos(rotation);
        return localX;
    }

    private static double convertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }
}
