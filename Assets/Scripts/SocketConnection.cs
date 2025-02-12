using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Concurrent;

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
    public LogManager logManager;

    // Lớp để ánh xạ dữ liệu JSON
    private class UserData
    {
        public int id { get; set; }
        public string nameTxT { get; set; }
        public string hashtag { get; set; }
        public bool played { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string deletedAt { get; set; }
    }

    void Start()
    {
        Connect();
    }

    void Update()
    {
        if (responseQueue.TryDequeue(out string response))
        {
            // Giải mã JSON thành đối tượng UserData
            UserData userData = JsonConvert.DeserializeObject<UserData>(response);

            if (userData != null)
            {
                // Cập nhật tên
                nameTxT.text = userData.nameTxT;

                // Tách các hashtag và cập nhật
                string[] hashtags = userData.hashtag.Split(new[] { ", " }, System.StringSplitOptions.None);
                if (hashtags.Length > 0) hashtag1.text = hashtags[0];
                if (hashtags.Length > 1) hashtag2.text = hashtags[1];
                if (hashtags.Length > 2) hashtag3.text = hashtags[2];
            }
        }
    }

    private async void Connect()
    {
        client = new SocketIO("ws://192.168.1.200:3000");

        client.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected");
            logManager.AddLog("Connected");
            await client.EmitAsync("getAllApartment");
        };

        client.On(nextTurn, async response =>
        {
            // Khi nhận được sự kiện nextTurn, gửi yêu cầu nextUser
            await client.EmitAsync(nextUser);
        });

        client.On(nextUser, response =>
        {
            // Thêm phản hồi vào hàng đợi
            responseQueue.Enqueue(response.ToString());
        });

        await client.ConnectAsync();
    }

    private async void OnApplicationQuit()
    {
        if (client != null && client.Connected)
        {
            await client.DisconnectAsync();
        }
    }
}
