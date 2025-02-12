using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public TextMeshProUGUI textPrefab;  // Prefab của Text cần chạy
    public int textCount = 5;           // Số lượng clone của text
    public float speed = 100f;          // Tốc độ chạy của text
    public float spacing = 300f;        // Khoảng cách giữa các chữ

    private List<RectTransform> textInstances = new List<RectTransform>();
    public float resetPositionX;
    public GameObject prefab; // Prefab để gắn vào cuối phần tử

    private GameObject prefabInstance; // Instance của prefab

    public int height = 0;

    void Start()
    {
        if (textPrefab == null)
        {
            Debug.LogError("Text Prefab chưa được gán!");
            return;
        }

        RectTransform textRect = textPrefab.GetComponent<RectTransform>();
        // resetPositionX = textCount * spacing;
        prefabInstance = Instantiate(prefab, textPrefab.GetComponent<RectTransform>());
        prefabInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            textPrefab.GetComponent<RectTransform>().rect.width / 2 + 3,
            height
        );
        // Tạo các bản sao text
        for (int i = 0; i < textCount; i++)
        {
            TextMeshProUGUI newText = Instantiate(textPrefab, transform);
            RectTransform newRect = newText.GetComponent<RectTransform>();
            newRect.anchoredPosition = new Vector2(i * spacing, 0);
            textInstances.Add(newRect);
        }
    }

    void Update()
    {
        if (textPrefab != null)
        {
            foreach (var duplicateTransform in textInstances)
            {
                var duplicateTMP = duplicateTransform.GetComponent<TextMeshProUGUI>();
                duplicateTMP.text = textPrefab.text;
                duplicateTMP.fontSize = textPrefab.fontSize;
            }
        }
        for (int i = 0; i < textInstances.Count; i++)
        {
            RectTransform textRect = textInstances[i];
            textRect.anchoredPosition += Vector2.left * speed * Time.deltaTime;

            // Khi text di chuyển đến ngoài màn hình thì đặt lại vị trí
            if (textRect.anchoredPosition.x < -resetPositionX)
            {
                float maxX = GetMaxXPosition();
                textRect.anchoredPosition = new Vector2(maxX + spacing, 0);
            }

        }
    }

    // Lấy giá trị X lớn nhất để đặt lại vị trí text bị đẩy ra ngoài
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
