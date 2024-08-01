using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Ballista_Special_Script : MonoBehaviour
{
    public GameObject BallistaBaseObject;
    private Transform BallistaBase;
    public DeckManager_script deckManager_Script;
    public GameObject CardSpecialObject;
    // Start is called before the first frame update
    void Start()
    {
        deckManager_Script = GameObject.Find("DeckManagerPanel").GetComponent<DeckManager_script>();
        BallistaBase.position = BallistaBaseObject.transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CardSpecialEffect()
        {
            Debug.Log("Balista card özel efekt çalıştı");         
            if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("0 butonuna basıldı");
                    Instantiate(CardSpecialObject, BallistaBase);
                    //Balista animasyon kodunu belki bootcamp sonrası buraya yazabilirim
                    deckManager_Script.isTurnPlayer1=false;
                    Debug.Log("balista oluşturma kodu çalıştı");
                }

        }
    void CardOutEffect()
        {
            CardSpecialObject.GetComponent<Renderer>().material.color = Color.red;
            CardSpecialObject.transform.Translate(CardSpecialObject.transform.up*Time.deltaTime);
            Destroy(CardSpecialObject , 2f);
        }
}
