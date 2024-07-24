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
        transform.SetParent(canvas.transform, false); // Canvas'a ta��
        if (cardHover != null)
        {
            cardHover.SetDragging(true); // S�r�kleme ba�lad���nda hover efektlerini kapat
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
            // Kart� kullan�labilir alana b�rakt�k
            transform.SetParent(originalParent); // Orijinal panele geri ta��
            FindObjectOfType<DeckManager>().PlayCard(gameObject); // Kart� oynat
        }
        else
        {
            transform.position = startPosition; // Ba�lang�� pozisyonuna geri d�n
            transform.SetParent(originalParent); // Orijinal panele geri ta��
        }
        if (cardHover != null)
        {
            cardHover.SetDragging(false); // S�r�kleme bitti�inde hover efektlerini a�
        }
    }
}
