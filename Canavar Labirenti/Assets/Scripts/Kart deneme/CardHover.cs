using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel; // Kart bilgilerini g�steren panel
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

        transform.localScale = originalScale * 1.1f; // Kart� b�y�t
        infoPanel.SetActive(true); // Bilgi panelini g�ster
        infoText.text = GetCardInfo(); // Kart bilgilerini g�ncelle
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.localScale = originalScale; // Kart� eski boyutuna getir
        infoPanel.SetActive(false); // Bilgi panelini gizle
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
        if (dragging)
        {
            transform.localScale = originalScale; // S�r�kleme ba�lad���nda kart� eski boyutuna getir
            infoPanel.SetActive(false); // S�r�kleme ba�lad���nda bilgi panelini gizle
        }
    }

    string GetCardInfo()
    {
        // Kart�n bilgilerini d�nd�ren bir metot
        // �rnek olarak kart�n ad� ve �zelli�i:
        return "Kart Ad�: " + gameObject.name + "\n�zellik: " + "�zellik Bilgisi";
    }
}
