using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System.Threading.Tasks;
public class SocketConnection : MonoBehaviour
{
    private static SocketIO client;

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
        client.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected");
            await client.EmitAsync("message", "Hello from Unity");
        };
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
