using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using SocketIOClient.Newtonsoft.Json;
using System;

public class SocketConnection : MonoBehaviour
{
    private SocketIO client;
    public TMPro.TextMeshProUGUI firstNameTxT;
    public TMPro.TextMeshProUGUI hashtag1;
    public TMPro.TextMeshProUGUI hashtag2;
    public TMPro.TextMeshProUGUI hashtag3;
    public TMPro.TextMeshProUGUI hashtag4;
    public TMPro.TextMeshProUGUI secondNameTxT;
    private const string nextUser = "nextUser";
    private const string nextTurn = "nextTurn";
    private const string newUser = "newUser";
    private ConcurrentQueue<string> responseQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();    
    public LogManager logManager;
    public StartEnding startEnding;
    public string ip = "ws://192.168.0.105:9456";
    private bool isReconnecting = false;    
    public AdjustTime adjustTime;
    private string testData = "[{\"id\":5,\"name\":\" nguyễn văn an \",\"hashtag\":\"Chúc Mừng 25 Năm AIA Việt Nam + Hành Trình Đầy Tự Hào;Congratulations To AIA Vietnam + 25 Years Of Inspiration\",\"played\":false,\"createdAt\":\"2025-02-10T07:32:08.000Z\",\"updatedAt\":\"2025-02-10T07:32:08.000Z\",\"deletedAt\":null}]";
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
    void Start()
    {
        processUserData(testData);        
        Invoke("Connect", 2);        
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
            if(log == "Disconnected. Reconnecting...")
            {
                StartCoroutine(ReconnectRoutine());
            }
            if(log == "Max reconnect attempts reached. Stopping reconnection.")
            {
                StartCoroutine(ReconnectRoutine());
            }
        }
        if (client == null && !isReconnecting)
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
            client.On(newUser, response =>
            {
                if (startEnding.isWaiting != true && response != null)
                {
                    client.EmitAsync(nextUser);
                }
                logQueue.Enqueue("Received NewUser: " + response);
            });
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
            client.OnDisconnected += (sender, e) =>
            {
                logQueue.Enqueue("Disconnected. Reconnecting...");
            };

            await client.ConnectAsync();
        }
        catch (System.Exception ex)
        {
            logQueue.Enqueue("Connection failed: " + ex.Message);
            StartCoroutine(ReconnectRoutine());
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

            firstNameTxT.text = "• " + userData.name.Trim().ToUpper() + " ";
            secondNameTxT.text = "• " + userData.name.Trim().ToUpper() + " ";
            string[] hashtags = userData.hashtag.Split(new[] { ";" }, System.StringSplitOptions.None);
            string[] firsthastag = hashtags[0].Split(new[] { "+" }, System.StringSplitOptions.None);
            string[] secondhastag = hashtags[1].Split(new[] { "+" }, System.StringSplitOptions.None);
            hashtag1.text = firsthastag[0];
            hashtag2.text = firsthastag[1];
            hashtag3.text = secondhastag[0];
            hashtag4.text = secondhastag[1];
            startEnding.EndMenu();
        }
    }
    public void EmitNextUser()
    {
        client.EmitAsync(nextUser);
    }
}