using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform originalParent;
    private Canvas canvas;
    private CardHover cardHover;

    void Start()
    {
        cardHover = GetComponent<CardHover>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform, false); // Canvas'a taþý
        if (cardHover != null)
        {
            cardHover.SetDragging(true); // Sürükleme baþladýðýnda hover efektlerini kapat
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // Fareyi takip et
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        if (cardHover != null)
        {
            cardHover.SetDragging(false); // Sürükleme bittiðinde hover efektlerini aç
        }
    }
}
