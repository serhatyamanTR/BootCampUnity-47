using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHover : MonoBehaviour
{
    public GameObject infoPanel; // Kart bilgilerini gösteren panel
    public TextMeshProUGUI infoText; // Paneldeki kart bilgileri metni

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        infoPanel.SetActive(false);
    }

    void OnMouseEnter()
    {
        transform.localScale = originalScale * 1.1f; // Kartı büyüt
        infoPanel.SetActive(true); // Bilgi panelini göster
        infoText.text = GetCardInfo(); // Kart bilgilerini güncelle
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale; // Kartı eski boyutuna getir
        infoPanel.SetActive(false); // Bilgi panelini gizle
    }

    string GetCardInfo()
    {
        // Kartın bilgilerini döndüren bir metot
        // Örnek olarak kartın adı ve özelliği:
        return "Kart Adı: " + gameObject.name + "\nÖzellik: " + "Özellik Bilgisi";
    }
}
