
using System.Collections.Generic;
using UnityEngine;

public class CenterDup : MonoBehaviour
{
    public TMPro.TextMeshProUGUI centerText;
    public int duplicateCount = 0;
    private List<TMPro.TextMeshProUGUI> leftTextInstances = new List<TMPro.TextMeshProUGUI>();
    private List<TMPro.TextMeshProUGUI> rightTextInstances = new List<TMPro.TextMeshProUGUI>();
    private void CreateTextInstance()
    {
        // Xóa tất cả các bản sao hiện tại
        foreach (var textInstance in leftTextInstances)
        {
            Destroy(textInstance.gameObject);
        }
        leftTextInstances.Clear();
        foreach (var textInstance in rightTextInstances)
        {
            Destroy(textInstance.gameObject);
        }
        rightTextInstances.Clear();
        for(int i = 0; i < duplicateCount; i++)
        {
            TMPro.TextMeshProUGUI newLeftText = Instantiate(centerText, transform);
            TMPro.TextMeshProUGUI newRightText = Instantiate(centerText, transform);
            RectTransform leftRect = newLeftText.GetComponent<RectTransform>();
            RectTransform rightRect = newRightText.GetComponent<RectTransform>();
            float halfWidth = centerText.GetComponent<RectTransform>().rect.width / 2;
            float width = centerText.GetComponent<RectTransform>().rect.width;
            leftRect.anchoredPosition = new Vector2(-i * centerText.GetComponent<RectTransform>().rect.width - width, 0);
            rightRect.anchoredPosition = new Vector2(i * centerText.GetComponent<RectTransform>().rect.width + width, 0);
            rightTextInstances.Add(newRightText);
            leftTextInstances.Add(newLeftText);
        }
    }

    void Start()
    {
        if (centerText == null)
        {
            Debug.LogError("Text Prefab chưa được gán!");
            return;
        }        
        Invoke("CreateTextInstance", 0.1f);
    }

    void Update()
    {
        if (leftTextInstances.Count > 0 && centerText.text != leftTextInstances[0].text)
        {
            Invoke("CreateTextInstance", 0.1f);
        }        
    }
}