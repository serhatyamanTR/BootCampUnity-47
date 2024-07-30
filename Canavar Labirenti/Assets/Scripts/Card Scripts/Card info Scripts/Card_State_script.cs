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


/*  ------------------    Paneller ve Listeleri ----------------------*/
    private Transform deckPanel; // Deste paneli
    private List<GameObject> deckList; // Deste kartrivate
    private Transform handPanel; // El paneli
    private List<GameObject> handList; // Elde bulunan karrivate
    private Transform inGamePanel;
    private List<GameObject> inGameList;
    private Transform gravePanel; // Mezar paneli
    private List<GameObject> graveList;

/*-------------------------------------------------------------------------*/

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
            deckManager_Script = FindAnyObjectByType<DeckManager_script>();
            deckPanel = deckManager_Script.deckPanel;
            handPanel = deckManager_Script.handPanel;
            inGamePanel = deckManager_Script.inGamePanel;
            gravePanel = deckManager_Script.gravePanel;

            deckList = deckManager_Script.deckList;
            handList = deckManager_Script.handList;
            inGameList = deckManager_Script.inGameList;
            graveList = deckManager_Script.graveList;

            /*
            if (deckManager_Script == null)
                {
                    Debug.LogError("DeckManager_script referansı bulunamadı!");
                }
            */


        }

    // Update is called once per frame
    void Update()
        {

        }

    public void SetState(Card_State state)
        {
            /*
            if (deckManager_Script == null)
                {
                    Debug.LogError("DeckManager_script referansı atanmadı!");
                    return;
                }
            */
            switch(state)
                {
                 case Card_State.inDeck:
                    {
                        isCardinHand    =   false;
                        isCardActive    =   false;
                        isCardPlayed    =   false;
                        //Instantiate(gameObject, deckPanel.transform);
                        infoPanel = transform.GetChild(0).gameObject;
                        infoPanel.SetActive(false);
                        //deckList.Add(gameObject);
                        Debug.Log(gameObject + " Desteye eklendi");
                        
                        
                      
                        
                    }
                    break;

                case Card_State.inHand:
                    {
                        isCardinHand    =   true;
                        handList.Add(gameObject);
                        deckList.Remove(gameObject);
                        StartCoroutine(MoveCardToPanel(gameObject, handPanel.transform));
                        cardActiveRoundRemain = cardActiveRoundCount;
                        

                    }
                    break;

                case Card_State.inGame:
                    {
                        StartCoroutine(MoveCardToPanel(gameObject, inGamePanel));
                        deckManager_Script.handList.Remove(gameObject);
                        isCardinHand    =   false;
                        isCardActive    =   true;
                        isCardPlayed    =   false;
                        
                       


                    }
                    break;

                case Card_State.inGrave:
                    {

                        StartCoroutine(MoveCardToPanel(gameObject, gravePanel));
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
        }
    public void OnBeginDrag(PointerEventData eventData)
        {
            if (isCardinHand)
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
        }    
    
    public void OnDrag(PointerEventData eventData)
        { 
            if (isDragging  &&  eventData.button == PointerEventData.InputButton.Left)
            {

                gameObject.transform.Translate(Input.mousePosition - gameObject.transform.position);
            }
        }

    public void OnEndDrag(PointerEventData eventData)
        {
            if (isDragging)
                {
                    isDragging = false;
                    transform.SetParent(parentAfterDrag);
                    tempImage.raycastTarget = true;
                }
        }


}
