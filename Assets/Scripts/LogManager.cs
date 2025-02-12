using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{

    [SerializeField] private GameObject textPrefab; // Prefab của TextMeshPro
    [SerializeField] private Transform content; // Content của ScrollView
    [SerializeField] private ScrollRect scrollRect; // ScrollRect của ScrollView

    // Hàm để thêm nội dung mới vào ScrollView
    public void AddLog(string message)
    {
        // Tạo một đối tượng Text mới từ prefab
        GameObject newTextObject = Instantiate(textPrefab, content);

        // Lấy component TextMeshProUGUI và thiết lập nội dung
        TextMeshProUGUI textComponent = newTextObject.GetComponent<TextMeshProUGUI>();
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Thời gian hiện tại
        textComponent.text = $"[{timestamp}] {message}";

        // Cập nhật bố cục layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

        // Tự động cuộn xuống dưới cùng
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
