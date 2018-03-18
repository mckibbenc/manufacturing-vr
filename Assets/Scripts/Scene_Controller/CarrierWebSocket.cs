using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class CarrierWebSocket : MonoBehaviour {
    string uniqueId = "123Colina";

    void Start()
    {
        WebSocket ws = new WebSocket("wss://mms-grpc-transfer-service.run.aws-usw02-pr.ice.predix.io/service");
        ws.OnOpen += (o, e) =>
        {
            Debug.Log("open");
        };
        ws.Connect();
    }
}
