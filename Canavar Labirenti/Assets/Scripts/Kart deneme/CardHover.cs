using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void OnPointerEnter()
    {
        transform.localScale = originalScale * 1.1f; // Kartý büyüt
        infoPanel.SetActive(true); // Bilgi panelini göster
        infoText.text = GetCardInfo(); // Kart bilgilerini güncelle
    }

    public void OnPointerExit()
    {
        transform.localScale = originalScale; // Kartý eski boyutuna getir
        infoPanel.SetActive(false); // Bilgi panelini gizle
    }

    string GetCardInfo()
    {
        // Kartýn bilgilerini döndüren bir metot
        // Örnek olarak kartýn adý ve özelliði:
        return "Kart Adý: " + gameObject.name + "\nÖzellik: " + "Özellik Bilgisi";
    }
}
