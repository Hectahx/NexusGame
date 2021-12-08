using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class CardHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public void reverseCard()
    {
        //Debug.Log("Reverse Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "cards";
        payload["type"] = "extraCard";
        payload["color"] = WsClient.color;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(payload.ToString());
    }

    public void enabledCard()
    {
        //Debug.Log("Enabled Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "cards";
        payload["type"] = "enabledCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(payload.ToString());
    }

    public void setExtraCard()
    {
        //Debug.Log("Enabled Card in use");
        JObject payload = new JObject();
        payload["method"] = "setCard";
        payload["type"] = "extraCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(payload.ToString());
    }

        public void setEnabledCard()
    {
        //Debug.Log("Enabled Card in use");
        JObject payload = new JObject();
        payload["method"] = "setCard";
        payload["type"] = "enabledCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(payload.ToString());
    }
}
