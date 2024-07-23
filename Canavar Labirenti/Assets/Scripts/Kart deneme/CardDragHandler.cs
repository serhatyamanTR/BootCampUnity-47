using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(transform.root); // Canvas'a taþý
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // Fareyi takip et
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kartý kullanýlabilir alana býraktýðýmýzý kontrol edin
        // Eðer geçerli bir yere býrakýlmadýysa, kartý baþlangýç pozisyonuna geri döndür
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("PlayableArea"))
        {
            // Kartý kullanýlabilir alana býraktýk
            transform.SetParent(originalParent); // Orijinal panele geri taþý
            FindObjectOfType<DeckManager>().PlayCard(gameObject); // Kartý oynat
        }
        else
        {
            transform.position = startPosition; // Baþlangýç pozisyonuna geri dön
            transform.SetParent(originalParent); // Orijinal panele geri taþý
        }
    }
}
