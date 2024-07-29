using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public  enum Card_State
    {
        inDeck,
        inHand,
        inGame,
        inGrave
    }
public class Card_State_script : MonoBehaviour, IPointerClickHandler, IBeginDragHandler , IDragHandler,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DeckManager_script deckManager_Script;

    public String cardSpellTag;
    public bool isCardPlayed    =   false;
    public bool isCardActive    =   false;
    public bool isCardinHand = false;
    public bool isCardinGrave = false;

    private Vector3 originalScale; 

    private bool isDragging = false;
    public Transform parentAfterDrag;
    public Image tempImage; // sürükle bırak için prefabin kendi resmi

    public  int cardActiveRoundCount;
    public int cardActiveRoundRemain;
    //public GameObject CardPrefab; //kartın kendi prefabi
    public GameObject infoPanel; //kartın info panel prefabi
    public MonoScript CardSpecialScript; //kartın özel efektini içeren script
    public float cardsAnimationSpeed;
    public float inCardPointerTimer = 2f;
    private float inCardPointerTimeRemain;

    // Start is called before the first frame update
    void Start()
        {
            //CardPrefab = transform.gameObject;
            originalScale = transform.localScale;

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
                        isCardinHand    =   false;
                        isCardActive    =   false;
                        isCardPlayed    =   false;
                        Instantiate(gameObject, deckManager_Script.deckPanel);
                        deckManager_Script.deckList.Add(gameObject);
                        Debug.Log(gameObject + " Desteye eklendi");
                        
                        
                      
                        
                    }
                    break;

                case Card_State.inHand:
                    {
                        isCardinHand    =   true;
                        deckManager_Script.handList.Add(gameObject);
                        deckManager_Script.deckList.Remove(gameObject);
                        StartCoroutine(MoveCardToPanel(gameObject, deckManager_Script.handPanel));
                        cardActiveRoundRemain = cardActiveRoundCount;

                    }
                    break;

                case Card_State.inGame:
                    {
                        StartCoroutine(MoveCardToPanel(gameObject, deckManager_Script.inGamePanel));
                        deckManager_Script.handList.Remove(gameObject);
                        isCardinHand    =   false;
                        isCardActive    =   true;
                        isCardPlayed    =   false;
                        
                       


                    }
                    break;

                case Card_State.inGrave:
                    {

                        StartCoroutine(MoveCardToPanel(gameObject, deckManager_Script.gravePanel));
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
            float duration = 1f/cardsAnimationSpeed; // Animasyon süresi

            while (elapsedTime < duration)
                {
                    card.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

            card.transform.SetParent(hedefPanel);
            card.transform.localPosition = Vector3.zero;
        }

    public void OnPointerEnter(PointerEventData eventData)
        { 
            inCardPointerTimeRemain = inCardPointerTimer;
    
            if (!isDragging && (isCardinHand || isCardActive))
                {
                    transform.localScale = originalScale * 1.1f;

                    while(inCardPointerTimeRemain >= 0f)
                        {
                            inCardPointerTimeRemain -=Time.deltaTime;
                        }
                    if (inCardPointerTimeRemain <= 0)
                        {
                            infoPanel.SetActive(true);
                        }

                }
        }

    public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale= originalScale;
            inCardPointerTimeRemain = inCardPointerTimer;
            infoPanel.SetActive(false);
        }
    public void OnPointerClick(PointerEventData eventData)
        {
            infoPanel.SetActive(false);

            if ( isCardinHand && eventData.button == PointerEventData.InputButton.Right)
                {
                    SetState(Card_State.inGame);
                }
        }
    public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                {
                    isDragging = true;
                    parentAfterDrag = transform.parent;
                    transform.SetParent(transform.root);
                    transform.SetAsLastSibling();
                    tempImage.raycastTarget = false;
                }
        }    
    
    public void OnDrag(PointerEventData eventData)
        { 
            if (eventData.button == PointerEventData.InputButton.Left)
            {

                gameObject.transform.Translate(Input.mousePosition - gameObject.transform.position);
            }
        }

    public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            tempImage.raycastTarget = true;
        }


}
