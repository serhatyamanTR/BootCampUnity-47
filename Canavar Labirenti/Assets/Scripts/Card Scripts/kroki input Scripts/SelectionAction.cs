using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionAction : MonoBehaviour
{
    public Selection_Point_interact selection_Point_Interact;
    //public Cam_Kroki_script cam_Kroki_Script;
    public GameObject PrefabToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
        {
            Debug.Log("OnmouseDown çalıştı");
            if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("1 butonuna basıldı");
                    if (selection_Point_Interact.isObjectSelected)
                        {
                            //selection_Point_Interact.selectedObjectTransform.gameObject.SetActive(false);
                            Instantiate(PrefabToInstantiate, selection_Point_Interact.selectedObjectTransform);
                            Debug.Log("oluşturma kodu çalıştı");
                        }
                }
        }
}
