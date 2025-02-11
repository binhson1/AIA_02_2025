using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System.Threading.Tasks;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json;
public class SocketConnection : MonoBehaviour
{
    private static SocketIO client;
    private const string nextUser = "nextUser";
    private const string nextTurn = "nextTurn";

    public LogManager logManager;
    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void Connect()
    {
        client = new SocketIO("ws://192.168.1.200:3000");
        // client = new SocketIO("ws://192.168.1.100:9456");
        client.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected");
            // logManager.AddLog("Connected");
            await client.EmitAsync("getAllApartment");
            await client.EmitAsync("nextUser");
        };
        client.On("nextTurn", response =>
        {
            logManager.AddLog(response.ToString());
            Debug.Log(response);
        });
        client.On("nextUser", response =>
        {
            logManager.AddLog(response.ToString());
            string message = JsonConvert.DeserializeObject<string>(response.ToString());
            Debug.Log(response);
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
