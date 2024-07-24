using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gate_object_script : MonoBehaviour
    {
        private GameObject disabledWall;
        // Start is called before the first frame update
        void Start()
            {

            }

        // Update is called once per frame
        void Update()
            {
                if (gameObject.activeSelf==false)
                    {
                        disabledWall.SetActive(true);
                    }
            }
        void OnTriggerEnter(Collider other)
            {
                Debug.Log("triggera girdi = "+ other.gameObject.name);
                if(other.CompareTag("wall"))
                    {
                        Debug.Log("obje kapandı = "+ other.gameObject.name);
                        other.gameObject.SetActive(false);
                        disabledWall = other.gameObject;
                        gameObject.GetComponent<Rigidbody>().detectCollisions = false; //sadece bir kereliğinne duvarları yokediyor.
                    }
            }
        void OnTriggerExit(Collider other)
            {
                Debug.Log("triggerdan çıktı = "+ other.gameObject.name);
                if(other.CompareTag("wall"))
                    {
                        Debug.Log("obje açıldı = "+ other.gameObject.name);
                        other.gameObject.SetActive(true);
                    }
            }        
    }
