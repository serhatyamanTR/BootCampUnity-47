using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefabı
    public Transform handPanel; // El paneli
    public Transform deckPanel; // Deste paneli
    public TextMeshProUGUI deckCountText; // Deste sayısını gösteren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartları gösteren text

    private List<GameObject> deck = new List<GameObject>(); // Deste kartları
    private List<GameObject> hand = new List<GameObject>(); // Elde bulunan kartlar
    private List<GameObject> playedCards = new List<GameObject>(); // Oynanan kartlar

    void Start()
    {
        InitializeDeck();
        UpdateDeckCount();
    }

    void InitializeDeck()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, deckPanel);
            newCard.name = "Card " + (i + 1);
            deck.Add(newCard);
        }
    }

    public void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < 5)
        {
            GameObject drawnCard = deck[0];
            deck.RemoveAt(0);
            drawnCard.transform.SetParent(handPanel);
            hand.Add(drawnCard);
            UpdateDeckCount();
        }
    }

    public void PlayCard(GameObject card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            card.transform.SetParent(null); // Panelden çıkar
            playedCards.Add(card);
            UpdatePlayedCards();
        }
    }

    void UpdateDeckCount()
    {
        deckCountText.text = "Destede kalan kart sayısı: " + deck.Count;
    }

    void UpdatePlayedCards()
    {
        playedCardsText.text = "Oynanan kartlar:\n";
        foreach (GameObject card in playedCards)
        {
            playedCardsText.text += card.name + "\n";
        }
    }
}
