using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class CarrierWebSocket : MonoBehaviour {
    string uniqueId = "123Colina";

    void Start()
    {
		using (var ws = new WebSocket ("wss://mms-grpc-transfer-service.run.aws-usw02-pr.ice.predix.io/service")) {
			ws.OnOpen += (sender, e) => {
				Debug.Log("open");
				Debug.Log(sender);
				Debug.Log(e);
			};
			ws.OnMessage += (sender, e) => {
				Debug.Log("message");
				Debug.Log(sender);
				Debug.Log(e);
			};
			ws.OnError += (sender, e) => {
				Debug.Log("error");
				Debug.Log(sender);
				Debug.Log(e);
			};
			ws.OnClose += (sender, e) => {
				Debug.Log("close");
				Debug.Log(sender);
				Debug.Log(e);
			};

			ws.ConnectAsync ();
		}
    }
}
