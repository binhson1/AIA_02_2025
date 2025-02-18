
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
        firstNameTxT.text = "• Chúc Mừng 25 Năm AIA Việt Nam Hành Trình Đầy Tự Hào";
        hashtag1.text = "Congratulations To AIA Vietnam";
        hashtag2.text = "25 Years Of Inspiration";
        hashtag3.text = "Congratulations To AIA Vietnam";
        hashtag4.text = "25 Years Of Inspiration";
        secondNameTxT.text = "• Chúc Mừng 25 Năm AIA Việt Nam Hành Trình Đầy Tự Hào ";
        socketConnection.EmitNextUser();
        isWaiting = false;
    }
}