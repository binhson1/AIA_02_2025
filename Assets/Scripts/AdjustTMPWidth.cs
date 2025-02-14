using UnityEngine;
using TMPro;

public class AdjustTMPWidth : MonoBehaviour
{
    public TextMeshProUGUI tmpText;  // Kéo TMP vào đây
    public RectTransform rectTransform; // Kéo RectTransform vào đây hoặc lấy từ chính tmpText

    void Start()
    {
        AdjustWidth();
    }

    void Update()
    {
        AdjustWidth();
    }
    void AdjustWidth()
    {
        if (tmpText == null || rectTransform == null) return;

        // Bắt buộc TMP cập nhật kích thước trước khi lấy preferredWidth
        tmpText.ForceMeshUpdate();

        // Lấy chiều rộng ưa thích của văn bản
        float textWidth = tmpText.preferredWidth;

        // Thêm padding nhỏ để tránh cắt chữ
        float padding = 10f;

        // Cập nhật kích thước của RectTransform
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth + padding);
    }
}
