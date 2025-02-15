using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class SimpleScrollingText : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;  // Prefab của Text cần chạy
    public int textCount = 5;           // Số lượng clone của text
    public float speed = 100f;          // Tốc độ chạy của text    

    private List<RectTransform> textInstances = new List<RectTransform>();
    public float resetPositionX = 2500;

    public float startpoint = 500;

    void Start()
    {
        if (textPrefab == null)
        {
            Debug.LogError("Text Prefab chưa được gán!");
            return;
        }

        RectTransform textRect = textPrefab.GetComponent<RectTransform>();
        Invoke("CreateTextInstance", 0.1f);
    }

    private void CreateTextInstance()
    {
        
        // Tạo các bản sao text
        //destroy all text instances
        foreach (var textInstance in textInstances)
        {
            Destroy(textInstance.gameObject);
        }
        textInstances.Clear();
        for (int i = 0  ; i < textCount; i++)
        {
            TextMeshProUGUI newText = Instantiate(textPrefab, transform);
            RectTransform newRect = newText.GetComponent<RectTransform>();
            newRect.anchoredPosition = new Vector2(i * textPrefab.GetComponent<RectTransform>().rect.width + startpoint, 0);
            textInstances.Add(newRect);
        }        
    }
    void Update()
    {
        if (textInstances.Count > 0 && textPrefab.text != textInstances[1].GetComponent<TextMeshProUGUI>().text)
        {
            //invoke create text instance after 1 second
            Invoke("CreateTextInstance", 0.1f);
        }
        else
        {
            for (int i = 0; i < textInstances.Count; i++)
            {
                RectTransform textRect = textInstances[i];
                textRect.anchoredPosition += Vector2.left * speed * Time.deltaTime;
                // Khi text di chuyển đến ngoài màn hình thì đặt lại vị trí
                if (textRect.anchoredPosition.x < -resetPositionX)
                {
                    float maxX = GetMaxXPosition();
                    textRect.anchoredPosition = new Vector2(maxX + textPrefab.GetComponent<RectTransform>().rect.width, 0);
                }
            }
        }
    }
    float GetMaxXPosition()
    {
        float maxX = float.MinValue;
        foreach (var textRect in textInstances)
        {
            if (textRect.anchoredPosition.x > maxX)
                maxX = textRect.anchoredPosition.x;
        }
        return maxX;
    }
}