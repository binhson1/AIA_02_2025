using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSocketIP : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_InputField ipInput;
    public SocketConnection socketConnection;
    void Start()
    {
        if (ipInput != null)
        {
            ipInput.text = socketConnection.ip;
        }
        ipInput.onEndEdit.AddListener(delegate { AdjustIP(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AdjustIP()
    {
        socketConnection.ip = ipInput.text;
    }
}
