using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MaterialClass {
    public string clientId;
    public string siteId;
    public string materialId;
    public string tripId;
    public string uri;
    public double x;
    public double y;
    public double z;
    public double orientation;
    public bool active;
    public string attributes;
    public long modified;
    public long dropoffTime;
    public string lastCarrierId;
    public string groupName;
    public string subgroupName;
    public double monetaryValue;
    public long createdDate;
    public long targetConsumptionDate;
    public string targetZone;
}
