using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Quobject.SocketIoClientDotNet.Client;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;


public class CarrierWebSocket : MonoBehaviour
{
    public void Start()
    {
        
        Debug.Log("Hello");
        /*
        var socket = IO.Socket("https://unity-eventhub-broker.run.aws-usw02-pr.ice.predix.io");
        var jobj = new JObject
        {
            { "Authorization", Authorization.getTokenWithoutBearer() }
        };

        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("connecting");
            socket.Emit("authentication", jobj);
        });
        socket.On(Socket.EVENT_DISCONNECT, (data) =>
        {
            Debug.Log("disconnect");
            Debug.Log(data);
        });
        socket.On("data", (data) =>
        {
            Debug.Log(data);
        });
        socket.On("authenticated", () =>
        {
            Debug.Log("authenticated");
        });

        socket.Connect();
        */
    }
}

