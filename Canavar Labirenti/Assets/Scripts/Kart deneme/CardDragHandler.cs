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
        transform.SetParent(transform.root); // Canvas'a ta��
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // Fareyi takip et
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kart� kullan�labilir alana b�rakt���m�z� kontrol edin
        // E�er ge�erli bir yere b�rak�lmad�ysa, kart� ba�lang�� pozisyonuna geri d�nd�r
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("PlayableArea"))
        {
            // Kart� kullan�labilir alana b�rakt�k
            transform.SetParent(originalParent); // Orijinal panele geri ta��
            FindObjectOfType<DeckManager>().PlayCard(gameObject); // Kart� oynat
        }
        else
        {
            transform.position = startPosition; // Ba�lang�� pozisyonuna geri d�n
            transform.SetParent(originalParent); // Orijinal panele geri ta��
        }
    }
}
