using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public static string clientId;
    public static string clientName;
    public static WebSocket ws;
    public GameObject signupPanel;
    public InputField loginEmail;
    public InputField loginPass;
    public InputField signupEmail;
    public InputField signupPass;
    public InputField signupUsername;
    public Button loginButton;
    bool publicServer = true;
    private string localIP = "127.0.0.1";
    private string publicIP = "143.244.213.81";
    private int port = 6969;
    private Regex hasNumber = new Regex(@"[0-9]+");
    private Regex hasUpperChar = new Regex(@"[A-Z]+");
    private Regex hasMinimum8Chars = new Regex(@".{8,}");
    private Regex isEmail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        if (publicServer) ws = new WebSocket($"wss://{publicIP}:{port}");
        else ws = new WebSocket($"wss://{localIP}:{port}");

        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connection open!");
        };

        ws.OnMessage += (sender, e) =>
        {
            JObject response;

            if (e.IsText)
            {
                response = JObject.Parse(e.Data);
            }
            else
            {
                response = JObject.Parse(Encoding.UTF8.GetString(e.RawData));
            }


            if (response["method"].ToString() == "loggedIn")
            {
                clientId = response["clientId"].ToString();
                clientName = response["username"].ToString();
                Debug.Log("yeet");
                UnityMainThread.wkr.AddJob(() =>
                {
                    SceneManager.LoadScene("MainDevelop");
                });
            }
            if (response["method"].ToString() == "guest")
            {
                clientId = response["clientId"].ToString();
                Debug.Log(clientId);
                UnityMainThread.wkr.AddJob(() =>
                {
                    SceneManager.LoadScene("MainDevelop");
                });
            }
            if (response["method"].ToString() == "signupComplete")
            {
                StartCoroutine(signupPanelTimer());
                Debug.Log("yeet");
            }


        };
        ws.OnClose += (sender, e) =>
        {
            Debug.Log(e.Reason);
        };


        ws.Connect();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void login()
    {
        JObject payload = new JObject();
        payload["method"] = "login";
        payload["email"] = loginEmail.text;
        payload["password"] = loginPass.text;
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    public void signup() //This is executed when a user clicks the sign in screen
    {
        var isPassValidated = hasNumber.IsMatch(signupPass.text) && hasUpperChar.IsMatch(signupPass.text) && hasMinimum8Chars.IsMatch(signupPass.text); //This checks the password matches the regex above
        var isEmailValidated = isEmail.IsMatch(signupEmail.text); //Same as above for email
        if (!isPassValidated)
        {
            Debug.Log("Pass not validated");
            return;
        }
        if (!isEmailValidated)
        {
            Debug.Log("Email not validated");
            return;
        }
        //If both are validated it means that they can signup with their account
        JObject payload = new JObject();
        payload["method"] = "signup";
        payload["email"] = signupEmail.text;
        payload["username"] = signupUsername.text;
        payload["password"] = signupPass.text;
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    IEnumerator signupPanelTimer()
    {
        showLogin();
        signupPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        signupPanel.SetActive(false);

    }

    public void showSignup() //These show functions are attached to buttons on the UI
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject loginUI = mainCanvas.transform.Find("LoginUIHolder").gameObject;
        GameObject signupUI = mainCanvas.transform.Find("SignupUIHolder").gameObject;

        loginUI.SetActive(false);
        signupUI.SetActive(true);
    }

    public void showLogin()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject loginUI = mainCanvas.transform.Find("LoginUIHolder").gameObject;
        GameObject signupUI = mainCanvas.transform.Find("SignupUIHolder").gameObject;

        loginUI.SetActive(true);
        signupUI.SetActive(false);
    }

    public void continueAsGuest()
    {
        JObject payload = new JObject();
        payload["method"] = "guest";
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }
}
