
using System.Collections;
using UnityEngine;

public class StartEnding : MonoBehaviour
{
    public TMPro.TextMeshProUGUI firstNameTxT;
    public TMPro.TextMeshProUGUI hashtag1;
    public TMPro.TextMeshProUGUI hashtag2;
    public TMPro.TextMeshProUGUI hashtag3;
    public TMPro.TextMeshProUGUI hashtag4;
    public TMPro.TextMeshProUGUI secondNameTxT;

    public string endingMessage = "WELLCOME TO THE SHOW";
    public AdjustTime adjustTime;
    public bool isWaiting = false;
    public SocketConnection socketConnection;
    public void EndMenu()
    {
        StopAllCoroutines();
        isWaiting = false;
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        isWaiting = true;
        yield return new WaitForSeconds(adjustTime.time);
        firstNameTxT.text = endingMessage;
        hashtag1.text = endingMessage;
        hashtag2.text = endingMessage;
        hashtag3.text = endingMessage;
        hashtag4.text = endingMessage;
        secondNameTxT.text = endingMessage;
        socketConnection.EmitNextUser();
        isWaiting = false;
    }
}