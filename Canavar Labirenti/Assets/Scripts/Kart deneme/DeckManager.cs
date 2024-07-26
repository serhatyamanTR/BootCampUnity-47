using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // Kart prefablarý listesi
    public Transform handPanel; // El paneli
    public Transform deckPanel; // Deste paneli
    public Transform graveyardPanel; // Mezar paneli
    public Transform playableArea; // Oynanabilir alan paneli
    public TextMeshProUGUI deckCountText; // Deste sayýsýný gösteren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartlarý gösteren text
    public GameObject cardBackPrefab; // Kart arka yüzü prefabý

    private List<GameObject> deck = new List<GameObject>(); // Deste kartlarý
    private List<GameObject> hand = new List<GameObject>(); // Elde bulunan kartlar
    private List<GameObject> playedCards = new List<GameObject>(); // Oynanan kartlar
    private int drawCount = 0; // Kart çekme hakký

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
            GameObject randomCardPrefab = GetRandomCardPrefab();
            GameObject newCard = Instantiate(randomCardPrefab, deckPanel);
            newCard.name = randomCardPrefab.name; // Ýsimlendirme
            newCard.AddComponent<CardClickHandler>(); // Kartlara týklama özelliði ekle
            newCard.GetComponent<CardClickHandler>().SetDeckManager(this); // DeckManager referansýný ayarla
            deck.Add(newCard);
            Debug.Log(newCard.name + " destede");

            // Kart arka yüzünü ekle
            GameObject cardBack = Instantiate(cardBackPrefab, newCard.transform);
            cardBack.transform.localPosition = Vector3.zero;
        }
    }

    GameObject GetRandomCardPrefab()
    {
        int randomIndex = Random.Range(0, 100);
        if (randomIndex < 5)
        {
            // %5 ihtimalle "EN EN EN Nadir" kartlar (0-1)
            return cardPrefabs[Random.Range(0, 2)];
        }
        else if (randomIndex < 15)
        {
            // %10 ihtimalle "En Nadir" kartlar (2-3)
            return cardPrefabs[Random.Range(2, 4)];
        }
        else if (randomIndex < 35)
        {
            // %20 ihtimalle "Ender" kartlar (4-11)
            return cardPrefabs[Random.Range(4, 12)];
        }
        else if (randomIndex < 65)
        {
            // %30 ihtimalle "Ortalama" kartlar (12-18)
            return cardPrefabs[Random.Range(12, 19)];
        }
        else
        {
            // %40 ihtimalle "Normal" kartlar (19-29)
            return cardPrefabs[Random.Range(19, cardPrefabs.Count)];
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
            GameObject drawnCard = GetUniqueCard();
            if (drawnCard != null)
            {
                hand.Add(drawnCard);
                StartCoroutine(MoveCardToHand(drawnCard));
                drawCount++;
                UpdateDeckCount();
                Debug.Log(drawnCard.name + " elde");
            }
        }
    }

    GameObject GetUniqueCard()
    {
        List<GameObject> availableCards = new List<GameObject>(deck); // Tüm desteyi kopyala

        while (availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            GameObject selectedCard = availableCards[randomIndex];

            if (!hand.Any(c => c.name == selectedCard.name)) // Elde yoksa
            {
                deck.Remove(selectedCard); // Desteden çýkar
                return selectedCard;
            }
            else
            {
                availableCards.RemoveAt(randomIndex); // Elde varsa listeden çýkar ve tekrar dene
            }
        }

        return null; // Eðer uygun kart bulunamazsa null döndür
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

        // Kart arka yüzünü kapat
        Transform cardBack = card.transform.Find(cardBackPrefab.name + "(Clone)");
        if (cardBack != null)
        {
            cardBack.gameObject.SetActive(false);
        }
    }

    public void PlayCard(GameObject card)
    {
        Debug.Log("PlayCard çaðrýldý");
        if (hand.Contains(card) || card.transform.parent == playableArea)
        {
            Debug.Log("Kart mezar paneline taþýnýyor");
            hand.Remove(card);
            card.transform.SetParent(graveyardPanel); // Kartý mezar paneline taþý
            card.transform.localPosition = Vector3.zero; // Mezarlýkta konumunu sýfýrla
            playedCards.Add(card);
            UpdatePlayedCards();
            Debug.Log(card.name + " mezarda");

            if (hand.Count == 0)
            {
                StartCoroutine(DealCardsCoroutine());
            }
        }
        else
        {
            Debug.Log("Kart el veya oynanabilir alanda deðil");
        }
    }

    public void SetCardsInteractable(bool interactable)
    {
        foreach (GameObject card in hand)
        {
            card.GetComponent<CardClickHandler>().enabled = interactable;
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
        deckCountText.text = "Destede kalan kart sayýsý: " + (30 - drawCount);
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
