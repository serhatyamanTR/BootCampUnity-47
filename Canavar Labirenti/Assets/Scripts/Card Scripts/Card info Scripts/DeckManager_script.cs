using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DeckManager_script : MonoBehaviour
{
    public List<GameObject> cardPrefabs; // Kart prefablar� listesi

/*  ------------------    Paneller ve Listeleri ----------------------*/
    public Transform deckPanel; // Deste paneli
    public List<GameObject> deckList; // Deste kartlar�

    public Transform handPanel; // El paneli
    public List<GameObject> handList; // Elde bulunan kartlar

    public Transform inGamePanel;
    public List<GameObject> inGameList;

    public Transform gravePanel; // Mezar paneli
    public List<GameObject> graveList;

/*-------------------------------------------------------------------------*/

    public bool isTurnPlayer1 =true;

    public TextMeshProUGUI deckCountText; // Deste say�s�n� g�steren text
    public TextMeshProUGUI playedCardsText; // Oynanan kartlar� g�steren text
    public GameObject cardBackPrefab; // Kart arka y�z� prefab�

    
    
    private List<GameObject> playedCards = new List<GameObject>(); // Oynanan kartlar
    private int drawCount = 0; // Kart �ekme hakk�

    void Start()
    {
        isTurnPlayer1 = false;
        if (deckPanel == null || handPanel == null || inGamePanel == null || gravePanel == null)
            {
                Debug.LogError("Panel referanslarından biri veya birkaçı atanmadı!");
                return;
            }
        InitializeDeck();
        if  (isTurnPlayer1)
            {
                DealStartingHand();
            }
       //UpdateDeckCount();
    }

    void InitializeDeck()
    {
        


        for (int i = 0; i < 30; i++)
        {
            GameObject randomCardPrefab = GetRandomCardPrefab();
            
            GameObject newCard = Instantiate(randomCardPrefab, deckPanel);
            newCard.name = randomCardPrefab.name; // �simlendirme
            newCard.GetComponent<Card_State_script>().SetState(Card_State.inDeck); //card statetini desteye çektim
            // newCard.AddComponent<CardClickHandler>(); // Kartlara t�klama �zelli�i ekle
            // newCard.GetComponent<CardClickHandler>().SetDeckManager(this); // DeckManager referans�n� ayarla
            deckList.Add(newCard);
            //Debug.Log(newCard.name + " destede");

            // Kart arka y�z�n� ekle
            /*
            GameObject cardBack = Instantiate(cardBackPrefab, newCard.transform);
            cardBack.transform.localPosition = Vector3.zero;
            */
        }
        if (deckPanel == null || cardPrefabs == null || cardPrefabs.Count == 0)
            {
                Debug.LogError("DeckPanel veya CardPrefabs referansı atanmadı!");
                return;
            }
    }

    GameObject GetRandomCardPrefab()
    {
        Debug.Log("Random kart seçiliyor");
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

    void DealHandReload()
        {
            if (isTurnPlayer1 && handList.Count == 0)
                {
                    DealCardsCoroutine();
                }    
        }

    IEnumerator DealCardsCoroutine()
    {
        for (int i = 0; i < 5; i++)
            {
                DrawCard();
                yield return new WaitForSeconds(0.5f); // Animasyon s�resi
            }
    }

    public void DrawCard()
    {
        if (deckList.Count > 0 && handList.Count < 5)
        {
            GameObject drawnCard = GetUniqueCard();
            if (drawnCard != null)
            {
                drawnCard.GetComponent<Card_State_script>().SetState(Card_State.inHand);
                //StartCoroutine(MoveCardToHand(drawnCard));
                drawCount++;
                //UpdateDeckCount();
                Debug.Log(drawnCard.name + " elde");
            }
        }
    }

    GameObject GetUniqueCard()
    {
        List<GameObject> availableCards = new List<GameObject>(deckList); // T�m desteyi kopyala

        while (availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            GameObject selectedCard = availableCards[randomIndex];

            if (!handList.Any(c => c.name == selectedCard.name)) // Elde yoksa
            {
                //deckList.Remove(selectedCard); // Desteden ��kar
                return selectedCard;
            }
            else
            {
                availableCards.RemoveAt(randomIndex); // Elde varsa listeden ��kar ve tekrar dene
            }
        }

        return null; // E�er uygun kart bulunamazsa null d�nd�r
    }

    /*
    IEnumerator MoveCardToHand(GameObject card)
    {
        Vector3 startPos = card.transform.position;
        Vector3 endPos = handPanel.position;
        float elapsedTime = 0f;
        float duration = 0.5f; // Animasyon s�resi

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.SetParent(handPanel);
        card.transform.localPosition = Vector3.zero;

        // Kart arka y�z�n� kapat
        Transform cardBack = card.transform.Find(cardBackPrefab.name + "(Clone)");
        if (cardBack != null)
        {
            cardBack.gameObject.SetActive(false);
        }
    }
    */

    public void PlayCard(GameObject card)
    {
        if (handList.Contains(card) || card.transform.parent == inGamePanel)
        {
            card.GetComponent<Card_State_script>().SetState(Card_State.inGame);
            Debug.Log("PlayCard �a�r�ld�");
            Debug.Log("Kart mezar paneline ta��n�yor");
            handList.Remove(card);
            card.transform.SetParent(gravePanel); // Kart� mezar paneline ta��
            card.transform.localPosition = Vector3.zero; // Mezarl�kta konumunu s�f�rla
            playedCards.Add(card);
            UpdatePlayedCards();
            Debug.Log(card.name + " mezarda");

            if (handList.Count == 0)
            {
                StartCoroutine(DealCardsCoroutine());
            }
        }
        else
        {
            Debug.Log("Kart el veya oynanabilir alanda de�il");
        }
    }
/*
    public void SetCardsInteractable(bool interactable)
    {
        foreach (GameObject card in handList)
        {
            card.GetComponent<CardClickHandler>().enabled = interactable;
        }
    }

*/

/*
    string GetCardType(GameObject card)
    {
        if (card.name.Contains("Duvar")) return "Duvar";
        if (card.name.Contains("B�y�")) return "B�y�";
        if (card.name.Contains("Canavar")) return "Canavar";
        if (card.name.Contains("Zemin")) return "Zemin";
        if (card.name.Contains("Kart")) return "Kart";
        if (card.name.Contains("Obje")) return "Obje";

        return "Bilinmeyen";
    }

*/
   void UpdateDeckCount()
    {
       if(deckCountText ==null)
        {
            Debug.LogError("deckCountText referansı atanmadı!");
            return;
        }
        deckCountText.text = "Destede kalan kart sayısı: " + (30 - drawCount);
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
        Debug.Log(graveyardText); // Veya bir UI Text bile�eni ile g�sterin
    }
}
