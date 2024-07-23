using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // Kart prefablar� listesi
    public Transform handPanel; // El paneli
    public Transform deckPanel; // Deste paneli
    public TextMeshProUGUI deckCountText; // Deste say�s�n� g�steren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartlar� g�steren text

    private List<GameObject> deck = new List<GameObject>(); // Deste kartlar�
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
            // Kart prefablar�n� rastgele se�erek desteyi olu�tur
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
            card.transform.SetParent(null); // Panelden ��kar
            playedCards.Add(card);
            UpdatePlayedCards();

            // Kart�n t�r�ne g�re mesaj g�ster
            string cardType = GetCardType(card);
            Debug.Log(cardType + " aktif edildi");
        }
    }

    string GetCardType(GameObject card)
    {
        // Kart�n t�r�n� belirleyen bir metot
        // �rne�in, kart�n ad� veya bir bile�eni �zerinden t�r belirleyebilirsiniz:
        // Bu �rnekte kart�n ad�n� kullanarak t�r belirliyoruz:
        if (card.name.Contains("Duvar")) return "Duvar";
        if (card.name.Contains("B�y�")) return "B�y�";
        if (card.name.Contains("Canavar")) return "Canavar";
        if (card.name.Contains("Zemin")) return "Zemin";
        if (card.name.Contains("Kart")) return "Kart";
        if (card.name.Contains("Obje")) return "Obje";

        return "Bilinmeyen";
    }


    void UpdateDeckCount()
    {
        deckCountText.text = "Destede kalan kart say�s�: " + deck.Count;
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
        // Mevcut durumda oynanan kartlar� bir pencerede g�ster
        string graveyardText = "Oynanan Kartlar:\n";
        foreach (GameObject card in playedCards)
        {
            graveyardText += card.name + "\n";
        }
        Debug.Log(graveyardText); // Veya bir UI Text bile�eni ile g�sterin
    }

}
