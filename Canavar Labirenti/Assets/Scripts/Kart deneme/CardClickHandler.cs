using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isActive = false;
    private static bool cardActive = false; // Di�er kartlara t�klamay� engellemek i�in
    private DeckManager deckManager;

    public void SetDeckManager(DeckManager manager)
    {
        deckManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !cardActive && transform.parent == deckManager.handPanel)
        {
            isActive = true;
            cardActive = true;
            Debug.Log(gameObject.name + " aktif edildi");

            // PlayableArea'ya ta��
            transform.SetParent(deckManager.playableArea);
            transform.localPosition = Vector3.zero;
            Debug.Log(gameObject.name + " oynand�");

            // Di�er kartlara t�klamay� engelle
            //deckManager.SetCardsInteractable(false);
            Debug.Log("sol");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Sa� t�klama alg�land�");

            if (isActive)
            {
                Debug.Log("Kart aktif durumda");

                if (transform.parent == deckManager.playableArea)
                {
                    Debug.Log("Kart PlayableArea'da");

                    // Mezar paneline ta��
                    deckManager.PlayCard(gameObject);
                    cardActive = false;
                    isActive = false;

                    // Di�er kartlara t�klamay� tekrar aktif et
                    deckManager.SetCardsInteractable(true);
                    Debug.Log("sa�");
                }
                else
                {
                    Debug.Log("Kart PlayableArea'da de�il");
                }
            }
            else
            {
                Debug.Log("Kart aktif de�il");
            }
        }
    }
}
