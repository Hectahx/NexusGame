using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Text;

public class CardHandler : MonoBehaviour
{
    // These functions are all attached to buttons in game

    public void extraCard()//This is called when the user adds the extra turn card to their deck
    {
        Debug.Log("Extra Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "cards";
        payload["type"] = "extraCard";
        payload["color"] = WsClient.color;
        payload["gameId"] = WsClient.gameId;
        //WsClient.ws.Send(payload.ToString());
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }



    public void enabledCard()//This is called when the user adds the reverse card to their deck
    {
        Debug.Log("Enabled Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "cards";
        payload["type"] = "enabledCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }

    public void setExtraCard()//This is called when the user wants to use the extra turn card
    {
        Debug.Log("Extra Card in use");
        JObject payload = new JObject();
        payload["method"] = "setCard";
        payload["type"] = "extraCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }

    public void setEnabledCard()//This is called when the user wants to use the enabled card
    {
        Debug.Log("Enabled Card in use");
        JObject payload = new JObject();
        payload["method"] = "setCard";
        payload["type"] = "enabledCard";
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }

    /*
    *Doom cards
    */

    public void skipTurn()
    {
        //Debug.Log("Reverse Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "doomCards";
        payload["type"] = "skipTurn";
        payload["color"] = WsClient.color;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }
    public void snatchCard()
    {
        //Debug.Log("Reverse Card Clicked");
        JObject payload = new JObject();
        payload["method"] = "doomCards";
        payload["type"] = "snatchCard";
        payload["color"] = WsClient.color;
        payload["gameId"] = WsClient.gameId;
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }
}
