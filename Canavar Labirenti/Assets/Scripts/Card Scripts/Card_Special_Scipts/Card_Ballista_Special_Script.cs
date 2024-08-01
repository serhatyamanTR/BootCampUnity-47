using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Ballista_Special_Script : MonoBehaviour
{
    public Transform BallistaBaseObject;
    public DeckManager_script deckManager_Script;
    public GameObject CardSpecialObject;
    public GameObject CreatedBallista;
    // Start is called before the first frame update
    
    void Awake()
        {
            
            deckManager_Script =    GameObject.Find("DeckManagerPanel").GetComponent<DeckManager_script>();
            BallistaBaseObject =    GameObject.Find("ZeminDoorPlayer1").transform;
            CardSpecialObject =     GameObject.Find("ballista_Object_Prefab");
        }

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CardSpecialEffect()
        {
            Debug.Log("Balista card özel efekt çalıştı");         
                    CreatedBallista = Instantiate(CardSpecialObject, BallistaBaseObject.position+Vector3.up*0.5f, BallistaBaseObject.rotation);
                    CreatedBallista.transform.localScale *= 0.3f;
                    //Balista animasyon kodunu belki bootcamp sonrası buraya yazabilirim
                    deckManager_Script.isTurnPlayer1=false;
                    Debug.Log("balista oluşturma kodu çalıştı");

        }
    public void CardOutEffect()
        {   
            CreatedBallista.GetComponent<Renderer>().material.color = Color.red;
            CreatedBallista.transform.Translate(CardSpecialObject.transform.up*Time.deltaTime);
            Destroy(CreatedBallista , 2f);
        }
}
