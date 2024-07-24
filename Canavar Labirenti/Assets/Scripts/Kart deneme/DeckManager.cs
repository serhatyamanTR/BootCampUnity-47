using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // Kart prefablarý listesi
    public Transform handPanel; // El paneli
    public Transform deckPanel; // Deste paneli
    public Transform graveyardPanel; // Mezar paneli
    public TextMeshProUGUI deckCountText; // Deste sayýsýný gösteren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartlarý gösteren text

    private List<GameObject> deck = new List<GameObject>(); // Deste kartlarý
    private List<GameObject> hand = new List<GameObject>(); // Elde bulunan kartlar
    private List<GameObject> playedCards = new List<GameObject>(); // Oynanan kartlar

    void Start()
    {
        InitializeDeck();
        DealStartingHand();
        UpdateDeckCount();
    }

    void InitializeDeck()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject randomCardPrefab = cardPrefabs[Random.Range(0, cardPrefabs.Count)];
            GameObject newCard = Instantiate(randomCardPrefab, deckPanel);
            newCard.name = "Card " + (i + 1);
            deck.Add(newCard);
        }
    }

    void DealStartingHand()
    {
        StartCoroutine(DealCardsCoroutine());
    }

    IEnumerator DealCardsCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.5f); // Animasyon süresi
        }
    }

    public void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < 5)
        {
            GameObject drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
            StartCoroutine(MoveCardToHand(drawnCard));
            UpdateDeckCount();
        }
    }

    IEnumerator MoveCardToHand(GameObject card)
    {
        Vector3 startPos = card.transform.position;
        Vector3 endPos = handPanel.position;
        float elapsedTime = 0f;
        float duration = 0.5f; // Animasyon süresi

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.SetParent(handPanel);
        card.transform.localPosition = Vector3.zero;
    }

    public void PlayCard(GameObject card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            card.transform.SetParent(graveyardPanel); // Kartý mezar paneline taþý
            playedCards.Add(card);
            UpdatePlayedCards();

            string cardType = GetCardType(card);
            Debug.Log(cardType + " aktif edildi");

            if (hand.Count == 0)
            {
                StartCoroutine(DealCardsCoroutine());
            }
        }
    }

    string GetCardType(GameObject card)
    {
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
        string graveyardText = "Oynanan Kartlar:\n";
        foreach (GameObject card in playedCards)
        {
            graveyardText += card.name + "\n";
        }
        Debug.Log(graveyardText); // Veya bir UI Text bileþeni ile gösterin
    }
}
