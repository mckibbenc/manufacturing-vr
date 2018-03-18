using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CarrierLocationClass
{
    public string clientId;
    public string siteId;
    public string carrierId;
    public double x;
    public double y;
    public double z;
    public double orientation;
    public long timestamp;
    public string[] materials;
    public long activeTime;
    public long materialActiveTime;
    public double speed;
}
