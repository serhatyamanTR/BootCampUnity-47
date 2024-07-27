using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public  enum Card_State
    {
        inDeck,
        inHand,
        inGame,
        inGrave
    }
public class Card_State_script : MonoBehaviour
{
    public Transform handPanel;
    public Transform graveYardPanel;
    public String cardSpellTag;
    public bool isCardPlayed    =   false;
    public bool isCardActive    =   false;
    public  int cardActiveRoundCount;
    public int cardActiveRoundRemain;
    public GameObject CardPrefab;
    public MonoBehaviour CardScript;
    public float AnimationSpeed;
    // Start is called before the first frame update
    void Start()
        {
            CardScript = CardPrefab.GetComponent<MonoBehaviour>(); // Kartın scriptini al
        }

    // Update is called once per frame
    void Update()
        {

        }

    public void SetState(Card_State state)
        {
            switch(state)
                {
                 case Card_State.inDeck:
                    {
                        isCardPlayed    =   false;
                        isCardActive    =   false;
                        
                      
                        
                    }
                    break;

                case Card_State.inHand:
                    {
                        MoveCardToPanel(CardPrefab, handPanel);
                        cardActiveRoundRemain = cardActiveRoundCount;
                    }
                    break;

                case Card_State.inGame:
                    {
                        isCardPlayed    =   true;
                        isCardActive    =   true;

                        if (CardScript != null)
                            {
                                var CardSpecialEffect = CardScript.GetType().GetMethod("CardSpecialEffect");
                                if (CardSpecialEffect != null)
                                    {
                                        CardSpecialEffect.Invoke(CardScript, null);
                                        
                                    }
                            }
                    }
                    break;

                case Card_State.inGrave:
                    {
                        MoveCardToPanel(CardPrefab, graveYardPanel);
                        if (CardScript != null)
                            {
                                var CardOutEffect = CardScript.GetType().GetMethod("CardOutEffect");
                                if (CardOutEffect != null)
                                    {
                                        CardOutEffect.Invoke(CardScript, null);
                                    }
                            }
                    }
                    break;
                }            
        }
    IEnumerator MoveCardToPanel(GameObject card, Transform hedefPanel)
        {
            Vector3 startPos = card.transform.position;
            Vector3 endPos = hedefPanel.position;
            float elapsedTime = 0f;
            float duration = 1f/AnimationSpeed; // Animasyon süresi

            while (elapsedTime < duration)
                {
                    card.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

            card.transform.SetParent(hedefPanel);
            card.transform.localPosition = Vector3.zero;
        }
}
