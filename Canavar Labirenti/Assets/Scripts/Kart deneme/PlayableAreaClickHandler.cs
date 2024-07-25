using UnityEngine;
using UnityEngine.EventSystems;

public class PlayableAreaClickHandler : MonoBehaviour, IPointerClickHandler
{
    public DeckManager deckManager; // DeckManager'a referans
    public Transform graveyardPanel; // GraveyardPanel'e referans

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("PlayableArea kartýna sað týklama algýlandý");

            // Týklanan kartý bul
            GameObject clickedCard = eventData.pointerCurrentRaycast.gameObject;

            // Mezar paneline taþý
            deckManager.PlayCard(clickedCard); // DeckManager'a kartý gönder

            // Diðer kartlara týklamayý tekrar aktif et
            deckManager.SetCardsInteractable(true);
        }
    }
}
