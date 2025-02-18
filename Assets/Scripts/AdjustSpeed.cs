using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdjustSpeed : MonoBehaviour
{
    public float speed = 100f;

    public TMP_InputField speedInput;
    // Start is called before the first frame update
    void Start()
    {
        if(speedInput != null)
        {
            speedInput.text = speed.ToString();
        }
        speedInput.onEndEdit.AddListener(delegate { ChangeSpeed(); });
    }

    public void ChangeSpeed()
    {
        if(speedInput != null)
        {
            speed = float.Parse(speedInput.text);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
