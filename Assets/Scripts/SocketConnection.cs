using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using SocketIOClient.Newtonsoft.Json;
using System;

public class SocketConnection : MonoBehaviour
{
    private SocketIO client;
    public TMPro.TextMeshProUGUI nameTxT;
    public TMPro.TextMeshProUGUI hashtag1;
    public TMPro.TextMeshProUGUI hashtag2;
    public TMPro.TextMeshProUGUI hashtag3;
    private const string nextUser = "nextUser";
    private const string nextTurn = "nextTurn";
    private ConcurrentQueue<string> responseQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
    public LogManager logManager;
    public string ip = "ws://192.168.0.105:9456";
    private bool isReconnecting = false;

    private class UserData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hashtag { get; set; }
        public bool played { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string deletedAt { get; set; }
    }

    private IEnumerator startConnection()
    {
        yield return new WaitForSeconds(3);
        Connect();
    }
    void Start()
    {
        startConnection();
        // StartCoroutine(ReconnectRoutine());
    }

    void Update()
    {
        if (responseQueue.TryDequeue(out string response))
        {
            processUserData(response);
        }
        if (logQueue.TryDequeue(out string log))
        {
            logManager.AddLog(log);
        }

        if (client != null && !client.Connected && !isReconnecting)
        {
            logQueue.Enqueue("Disconnected. Attempting to reconnect...");
            StartCoroutine(ReconnectRoutine());
        }
    }

    private async void Connect()
    {
        if (string.IsNullOrEmpty(ip) || !Uri.IsWellFormedUriString(ip, UriKind.Absolute))
        {
            logQueue.Enqueue("Invalid IP address. Connection aborted.");
            return;
        }

        try
        {
            logQueue.Enqueue("Connecting to " + ip);

            client = new SocketIO(ip);
            client.JsonSerializer = new NewtonsoftJsonSerializer();

            client.OnConnected += async (sender, e) =>
            {
                logQueue.Enqueue("Connected");
                await client.EmitAsync("nextUser");
                logQueue.Enqueue("Sent nextUser");
            };

            client.On(nextTurn, response =>
            {
                logQueue.Enqueue("Received nextTurn");
                client.EmitAsync(nextUser);
            });
            client.On("nextUser", response =>
            {
                if (response != null)
                {
                    responseQueue.Enqueue(response.ToString());
                }
                logQueue.Enqueue("Received nextUser: " + response);
            });
            client.OnDisconnected += async (sender, e) =>
            {
                logQueue.Enqueue("Disconnected. Reconnecting...");
                StartCoroutine(ReconnectRoutine());
            };

            await client.ConnectAsync();
        }
        catch (System.Exception ex)
        {
            logQueue.Enqueue("Connection failed: " + ex.Message);
        }
    }


    private IEnumerator ReconnectRoutine()
    {
        if (isReconnecting) yield break;

        isReconnecting = true;
        int retryCount = 0;
        const int maxRetries = 5;

        while ((!client?.Connected ?? true) && retryCount < maxRetries)
        {
            logQueue.Enqueue($"Attempting to reconnect... (Attempt {retryCount + 1}/{maxRetries})");
            Connect();
            retryCount++;
            yield return new WaitForSeconds(5);
        }

        if (retryCount >= maxRetries)
        {
            logQueue.Enqueue("Max reconnect attempts reached. Stopping reconnection.");
        }

        isReconnecting = false;
    }

    private async void OnApplicationQuit()
    {
        if (client != null && client.Connected)
        {
            await client.DisconnectAsync();
        }
    }

    public void processUserData(string response)
    {
        List<UserData> userDataList = JsonConvert.DeserializeObject<List<UserData>>(response);

        if (userDataList != null && userDataList.Count > 0)
        {
            UserData userData = userDataList[0];

            nameTxT.text = " " + userData.name + " •";

            string[] hashtags = userData.hashtag.Split(new[] { ", " }, System.StringSplitOptions.None);
            if (hashtags.Length > 0) hashtag1.text = " " + hashtags[0] + " •";
            if (hashtags.Length > 1) hashtag2.text = " " + hashtags[1] + " •";
            hashtag3.text = " " + userData.name + " •";
        }
    }

    public void UpdateIP(string newIP)
    {
        if (ip != newIP)
        {
            ip = newIP;
            logQueue.Enqueue("IP updated to: " + ip);
            if (client != null && client.Connected)
            {
                client.DisconnectAsync();
            }
            StartCoroutine(ReconnectRoutine());
        }
    }
}