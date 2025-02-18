using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class ReverseScrollingText : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;  // Prefab của Text cần chạy
    public int textCount = 5;           // Số lượng clone của text
    public AdjustSpeed speedController; // Script điều chỉnh tốc độ

    private List<RectTransform> textInstances = new List<RectTransform>();
    public float resetPositionX = -2500;

    public float startpoint = -500;

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
        // Xóa tất cả các bản sao hiện tại
        foreach (var textInstance in textInstances)
        {
            Destroy(textInstance.gameObject);
        }
        textInstances.Clear();

        // Tạo các bản sao text mới
        for (int i = 0; i < textCount; i++)
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
            // Tạo lại các bản sao nếu nội dung thay đổi
            Invoke("CreateTextInstance", 0.1f);
        }
        else
        {
            for (int i = 0; i < textInstances.Count; i++)
            {
                RectTransform textRect = textInstances[i];
                // Di chuyển text về bên phải
                textRect.anchoredPosition += Vector2.right * speedController.speed * Time.deltaTime;
                // Khi text di chuyển ra ngoài màn hình thì đặt lại vị trí
                if (textRect.anchoredPosition.x > -resetPositionX)
                {
                    float minX = GetMinXPosition();
                    textRect.anchoredPosition = new Vector2(minX - textPrefab.GetComponent<RectTransform>().rect.width, 0);
                }
            }
        }
    }

    float GetMinXPosition()
    {
        float minX = float.MaxValue;
        foreach (var textRect in textInstances)
        {
            if (textRect.anchoredPosition.x < minX)
                minX = textRect.anchoredPosition.x;
        }
        return minX;
    }
}
