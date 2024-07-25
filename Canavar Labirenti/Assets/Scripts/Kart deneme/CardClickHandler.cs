using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private bool isActive = false;
    private static bool cardActive = false; // Diðer kartlara týklamayý engellemek için
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

            // PlayableArea'ya taþý
            transform.SetParent(deckManager.playableArea);
            transform.localPosition = Vector3.zero;
            Debug.Log(gameObject.name + " oynandý");

            // Diðer kartlara týklamayý engelle
            //deckManager.SetCardsInteractable(false);
            Debug.Log("sol");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Sað týklama algýlandý");

            if (isActive)
            {
                Debug.Log("Kart aktif durumda");

                if (transform.parent == deckManager.playableArea)
                {
                    Debug.Log("Kart PlayableArea'da");

                    // Mezar paneline taþý
                    deckManager.PlayCard(gameObject);
                    cardActive = false;
                    isActive = false;

                    // Diðer kartlara týklamayý tekrar aktif et
                    deckManager.SetCardsInteractable(true);
                    Debug.Log("sað");
                }
                else
                {
                    Debug.Log("Kart PlayableArea'da deðil");
                }
            }
            else
            {
                Debug.Log("Kart aktif deðil");
            }
        }
    }
}
