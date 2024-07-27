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
    public MonoScript CardSpecialScript;
    public float AnimationSpeed;
    // Start is called before the first frame update
    void Start()
        {
            //CardSpecialScript = CardPrefab.GetComponent<MonoScript>(); // Kartın scriptini al
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
                        isCardActive    =   true;
                        isCardPlayed    =   false;
                       


                    }
                    break;

                case Card_State.inGrave:
                    {
                        MoveCardToPanel(CardPrefab, graveYardPanel);
                        if (CardSpecialScript != null)
                            {
                                var CardOutEffect = CardSpecialScript.GetType().GetMethod("CardOutEffect");
                                if (CardOutEffect != null)
                                    {
                                        CardOutEffect.Invoke(CardSpecialScript, null);
                                    }
                            }
                    }
                    break;
                }            
        }
    void OnMouseDown()
        {
            Debug.Log("OnmouseDown çalıştı");
            if (isCardActive &&!isCardPlayed)
                {
                    if (CardSpecialScript != null)
                        {
                            var CardSpecialEffect = CardSpecialScript.GetType().GetMethod("CardSpecialEffect");
                            if (CardSpecialEffect != null)
                                    {
                                        CardSpecialEffect.Invoke(CardSpecialScript, null);

                                    }
                                else
                                    {
                                        Debug.LogWarning("Katın özel efekti yok");
                                    }
                        }
                        else
                            {
                                Debug.LogWarning("Katın özel scripti yok");
                            }
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
