
using TMPro;
using UnityEngine;

public class AdjustTime : MonoBehaviour
{
    public TMP_InputField timeInput;
    public float time = 10f; 

    void Start()
    {
        if (timeInput != null)
        {
            timeInput.text = time.ToString();
        }
        timeInput.onEndEdit.AddListener(delegate { ChangeTime(); });
    }
    public void ChangeTime()
    {
        if (timeInput != null)
        {
            time = float.Parse(timeInput.text);
        }
    }

}