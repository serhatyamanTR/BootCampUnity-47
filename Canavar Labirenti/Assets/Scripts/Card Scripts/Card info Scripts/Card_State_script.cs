using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using System.Security.Cryptography;
using System.Net.WebSockets;
using System.Reflection;

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

    public Selection_Point_interact selection_Point_Interact;


/*  ------------------    Paneller ve Listeleri ----------------------*/
    private Transform deckPanel; // Deste paneli
    private List<GameObject> deckList; // Deste kartrivate
    private static Transform handPanel; // El paneli
    private List<GameObject> handList; // Elde bulunan karrivate
    private Transform inGamePanel;

    public int SelectedinGamePanelSlot;
    public bool isSlotSelected;
    private List<GameObject> inGameList;
    private Transform gravePanel; // Mezar paneli
    private List<GameObject> graveList;

    public GameObject infoPanel; //kartın info panel prefabi

/*-------------------------------------------------------------------------*/

    public String cardStateSpellTag;
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
    public MonoBehaviour CardSpecialScript; //kartın özel efektini içeren script
    public float cardsAnimationSpeed=3f;
    public float inCardPointerTimer = 2f;
    private float inCardPointerTimeRemain;

    private Component[] components;
    

    void Awake()
        {
            
            
            deckManager_Script = GameObject.Find("DeckManagerPanel").GetComponent<DeckManager_script>();
            deckPanel   = GameObject.Find("DeckPanel").transform;
            handPanel   = GameObject.Find("HandPanel").transform;
            inGamePanel = GameObject.Find("inGamePanel").transform;
            gravePanel  = GameObject.Find("GravePanel").transform;
            selection_Point_Interact = GameObject.Find("Selection_Point").GetComponent<Selection_Point_interact>();
        }
    // Start is called before the first frame update
    void Start()
        {

             components = GetComponents<Component>();
                Debug.Log("CardState Start için  ismi = "+gameObject.name );
                if((MonoBehaviour)components[4] !=null)
                {
                    CardSpecialScript = (MonoBehaviour)components[4];
                }
                else
                    {
                        Debug.LogWarning("CardSpecialScript 4. component değil veya eklenmemiş");
                    }

            //CardPrefab = transform.gameObject;
            originalScale = transform.localScale;
            

            deckList = deckManager_Script.deckList;
            handList = deckManager_Script.handList;
            inGameList = deckManager_Script.inGameList;
            graveList = deckManager_Script.graveList;
//---------------------Component kontrolü   -----------------------------------//
            if (deckPanel == null)
                {
                    Debug.LogError("deckPanel referansı bulunamadı!");
                }
            if (deckList == null)
                {
                    Debug.LogError("deckList referansı bulunamadı!");
                }
            if (handPanel == null)
                {
                    Debug.LogError("handPanel referansı bulunamadı!");
                }
            if (handList == null)
                {
                    Debug.LogError("handList referansı bulunamadı!");
                }
            if (inGamePanel == null)
                {
                    Debug.LogError("inGamePanel referansı bulunamadı!");
                }
            if (inGamePanel == null)
                {
                    Debug.LogError("inGamePanel referansı bulunamadı!");
                }
            if (inGameList == null)
                {
                    Debug.LogError("inGameList referansı bulunamadı!");
                }
            if (gravePanel == null)
                {
                    Debug.LogError("gravePanel referansı bulunamadı!");
                }
            if (graveList == null)
            {
                Debug.LogError("graveList referansı bulunamadı!");
            }
//-----------------------------------------------------------------------------------//


        }

    // Update is called once per frame
    void Update()
        {

        }
    private void InvokeMethodIfExists(MonoBehaviour component, string methodName)
        {
            if (component != null)
                {
                    // Metod bilgilerini al
                    MethodInfo method = component.GetType().GetMethod(methodName);

                    if (method != null)
                        {
                            // Metodu çalıştır
                            method.Invoke(component, null);
                        }
                    else
                        {
                            Debug.LogWarning($"Method {methodName} not found in {component.GetType().Name}");
                        }
                }
            else
                {
                    Debug.LogWarning("Component is null");
                }
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
                        if (gameObject.transform.childCount == 1)
                            {
                                infoPanel = transform.GetChild(0).gameObject;
                                if (infoPanel != null)
                                    {
                                        infoPanel.SetActive(false);
                                    }
                            }
                            else
                                {
                                    Debug.LogError(gameObject.name + " 0 veya birden fazla info paneli var");
                                    infoPanel = null;
                                }
                        if (gameObject != null )
                            {    
                                if (
                                        gameObject.transform.parent == handPanel
                                        ||
                                        gameObject.transform.parent == inGamePanel
                                        ||
                                        gameObject.transform.parent == gravePanel

                                   )
                                    {
                                        StartCoroutine(MoveCardToPanel(gameObject, deckPanel));
                                    }
                                    else
                                    {
                                        gameObject.transform.SetParent(deckPanel);
                                        gameObject.transform.SetAsFirstSibling();  
                                    }

                            }
                        gameObject.transform.localPosition = Vector3.zero;
                        //deckList.Add(gameObject);
                        Debug.Log(gameObject + " Desteye eklendi");
                        
                        
                      
                        
                    }
                    break;

                case Card_State.inHand:
                    {
                        isCardinHand    =   true;
                        //handList.Add(gameObject);
                        //gameObject.transform.SetParent(handPanel);  
                        //Debug.Log("inhand deki HandPanel = " + handPanel);
                        if (handPanel != null)
                        {
                            //Debug.Log("handpanel çalşıyor");
                        }
                        else
                            {
                                Debug.LogError("handpanel kaput");
                            }
                        Debug.Log("inhandDeki card = " + gameObject.name );
                        //deckList.Remove(gameObject);
                        if (infoPanel != null)
                            {
                                StartCoroutine(MoveCardToPanel(gameObject, handPanel));
                            }
                            else
                            {
                                Debug.LogError("infoPanel null olduğu için MoveCardToPanel fonksiyonu çalıştırılamadı.");
                            }
                        //gameObject.transform.localPosition = Vector3.zero;
                        cardActiveRoundRemain = cardActiveRoundCount;
                        

                    }
                    break;

                case Card_State.inGame:
                    {
                        selection_Point_Interact.cardSpellTag = cardStateSpellTag;
                        gameObject.transform.localPosition = Vector3.zero;
                        deckManager_Script.handList.Remove(gameObject);
                        isCardinHand    =   false;
                        isCardActive    =   true;
                        isCardPlayed    =   false;
                        if (isCardActive &&!isCardPlayed)
                                {
                                    Debug.Log("Kart Oynandı = " + gameObject.name);
                                    InvokeMethodIfExists(CardSpecialScript, "CardSpecialEffect");
                                }
                       


                    }
                    break;

                case Card_State.inGrave:
                    {

                        StartCoroutine(MoveCardToPanel(gameObject, gravePanel));
                        gameObject.transform.localPosition = Vector3.zero;
                        if (CardSpecialScript != null)
                            {
                                var CardOutEffect = CardSpecialScript.GetType().GetMethod("CardOutEffect()");
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
            card.transform.SetAsFirstSibling();
            card.transform.localPosition = Vector3.zero;
            deckManager_Script.isMoveAnimCompleted= true;
        }



    public void OnPointerEnter(PointerEventData eventData)
        { 
            inCardPointerTimeRemain = inCardPointerTimer;
    
            if (!isDragging && (isCardinHand || isCardActive))
                {
                    transform.localScale = originalScale * 1.5f;

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
            if(infoPanel.activeSelf)
                {
                    infoPanel.SetActive(false);
                }
        }
    public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Tıklanan Kart = " + gameObject.name);
            infoPanel.SetActive(false);
            

            if ( isCardinHand && eventData.button == PointerEventData.InputButton.Right)
                {
                    if(deckManager_Script.isMoveAnimCompleted)
                        {
                            deckManager_Script.isMoveAnimCompleted = false;
                            Debug.Log("karta SAĞ tıklandı = " + gameObject.name);
                            SetState(Card_State.inGame);

                            for (int i = 0; i< inGamePanel.childCount;i++)
                                {
                                    if (inGamePanel.GetChild(i).childCount == 0)
                                        {
                                            StartCoroutine(MoveCardToPanel(gameObject, inGamePanel.GetChild(i)));
                                            break;
                                        }
                                }


                            
                        }
                        else
                            {
                                Debug.LogWarning("Moveanimation bitmeden sağ tıklandı");
                            }
                }
        }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCardinHand && eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Kart Elde ve Sürüklenmeye çalışıldı = " + gameObject.name);
            isDragging = true;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root); // Orman yapısına taşınma
            transform.SetAsLastSibling(); // En üstte görünmesini sağlar
            tempImage = GetComponent<Image>();
            tempImage.raycastTarget = false;
        }
        else
        {
            Debug.Log("Kart Sürüklenmeye çalışıldı ama kart elde değil = " + gameObject.name);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    (RectTransform)transform.parent,
                    eventData.position,
                    eventData.enterEventCamera,
                    out localPointerPosition
                );
            transform.localPosition = localPointerPosition;
            
            //Debug.Log("Anlık mouse input lokasyonu = " + Input.mousePosition);
            //Debug.Log("Anlık mouse eventdataposition lokasyonu = " + eventData.position);
            //Debug.Log("Anlık localposition lokasyonu = " + transform.localPosition);
            //Debug.Log(gameObject.name + " kartının anlık lokasyonu = " + transform.position);
            //Debug.Log("Card Anlık Parenti = " + transform.parent.gameObject.name);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            //Debug.Log("Sürükleme Bitti = " + gameObject.name);
            isDragging = false;
            transform.SetParent(parentAfterDrag);
            tempImage = GetComponent<Image>();
            tempImage.raycastTarget = true;
        }
    }

    


}
