using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // Kart prefablarý listesi
    public Transform handPanel; // El paneli
    public Transform deckPanel; // Deste paneli
    public TextMeshProUGUI deckCountText; // Deste sayýsýný gösteren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartlarý gösteren text

    private List<GameObject> deck = new List<GameObject>(); // Deste kartlarý
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
            // Kart prefablarýný rastgele seçerek desteyi oluþtur
            GameObject randomCardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Count)];
            GameObject newCard = Instantiate(randomCardPrefab, deckPanel);
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
            card.transform.SetParent(null); // Panelden çýkar
            playedCards.Add(card);
            UpdatePlayedCards();

            // Kartýn türüne göre mesaj göster
            string cardType = GetCardType(card);
            Debug.Log(cardType + " aktif edildi");
        }
    }

    string GetCardType(GameObject card)
    {
        // Kartýn türünü belirleyen bir metot
        // Örneðin, kartýn adý veya bir bileþeni üzerinden tür belirleyebilirsiniz:
        // Bu örnekte kartýn adýný kullanarak tür belirliyoruz:
        if (card.name.Contains("Duvar")) return "Duvar";
        if (card.name.Contains("Büyü")) return "Büyü";
        if (card.name.Contains("Canavar")) return "Canavar";
        if (card.name.Contains("Zemin")) return "Zemin";
        if (card.name.Contains("Kart")) return "Kart";
        if (card.name.Contains("Obje")) return "Obje";

        return "Bilinmeyen";
    }


    void UpdateDeckCount()
    {
        deckCountText.text = "Destede kalan kart sayýsý: " + deck.Count;
    }

    void UpdatePlayedCards()
    {
        playedCardsText.text = "Oynanan kartlar:\n";
        foreach (GameObject card in playedCards)
        {
            playedCardsText.text += card.name + "\n";
        }
    }

    public void ShowGraveyard()
    {
        // Mevcut durumda oynanan kartlarý bir pencerede göster
        string graveyardText = "Oynanan Kartlar:\n";
        foreach (GameObject card in playedCards)
        {
            graveyardText += card.name + "\n";
        }
        Debug.Log(graveyardText); // Veya bir UI Text bileþeni ile gösterin
    }

}
