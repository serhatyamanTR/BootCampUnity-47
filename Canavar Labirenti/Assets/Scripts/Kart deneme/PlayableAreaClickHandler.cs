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
            Debug.Log("PlayableArea kart�na sa� t�klama alg�land�");

            // T�klanan kart� bul
            GameObject clickedCard = eventData.pointerCurrentRaycast.gameObject;

            // Mezar paneline ta��
            deckManager.PlayCard(clickedCard); // DeckManager'a kart� g�nder

            // Di�er kartlara t�klamay� tekrar aktif et
            deckManager.SetCardsInteractable(true);
        }
    }
}
