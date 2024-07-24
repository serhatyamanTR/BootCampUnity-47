using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel; // Kart bilgilerini gösteren panel
    public TextMeshProUGUI infoText; // Paneldeki kart bilgileri metni

    private Vector3 originalScale;
    private bool isDragging = false;

    void Start()
    {
        originalScale = transform.localScale;
        infoPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.localScale = originalScale * 1.1f; // Kartý büyüt
        infoPanel.SetActive(true); // Bilgi panelini göster
        infoText.text = GetCardInfo(); // Kart bilgilerini güncelle
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.localScale = originalScale; // Kartý eski boyutuna getir
        infoPanel.SetActive(false); // Bilgi panelini gizle
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (dragging)
        {
            transform.localScale = originalScale; // Sürükleme baþladýðýnda kartý eski boyutuna getir
            infoPanel.SetActive(false); // Sürükleme baþladýðýnda bilgi panelini gizle
        }
    }

    string GetCardInfo()
    {
        // Kartýn bilgilerini döndüren bir metot
        // Örnek olarak kartýn adý ve özelliði:
        return "Kart Adý: " + gameObject.name + "\nÖzellik: " + "Özellik Bilgisi";
    }
}
