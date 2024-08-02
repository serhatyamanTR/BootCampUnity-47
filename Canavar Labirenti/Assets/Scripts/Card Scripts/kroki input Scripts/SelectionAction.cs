using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAction : MonoBehaviour
{
    public Selection_Point_interact selection_Point_Interact;
    //public Cam_Kroki_script cam_Kroki_Script;

    public DeckManager_script deckManager_Script;
    public GameObject PrefabToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        deckManager_Script = GameObject.Find("DeckManagerPanel").GetComponent<DeckManager_script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
        {
            Debug.Log("OnmouseDown çalıştı");

            if (selection_Point_Interact.isObjectSelected)
                {
                    if (Input.GetMouseButtonDown(0) && deckManager_Script.isTurnPlayer1)
                        {
                            Debug.Log("obje selected  ve 1 butonuna basıldı");
                            //selection_Point_Interact.selectedObjectTransform.gameObject.SetActive(false);
                            Instantiate(PrefabToInstantiate, selection_Point_Interact.selectedObjectTransform.position,selection_Point_Interact.selectedObjectTransform.rotation);
                            Debug.Log("oluşturma kodu çalıştı");
                            deckManager_Script.isTurnPlayer1=false; 

                        }
                }
            
            
        }
}
