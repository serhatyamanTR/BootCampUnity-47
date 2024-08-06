using System.Collections;
using System.Collections.Generic;
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
                        other.gameObject.GetComponent<MeshRenderer>().enabled=false;
                        other.gameObject.GetComponent<Collider>().enabled=false;
                        disabledWall = other.gameObject;
                        gameObject.GetComponent<Rigidbody>().detectCollisions = false; //sadece bir kereliğinne duvarları yokediyor.
                    }
            }
        void OnTriggerExit(Collider other)
            {
                Debug.Log("triggerdan çıktı = "+ other.gameObject.name);
                if(other.CompareTag("wall"))
                    {
                        other.gameObject.GetComponent<MeshRenderer>().enabled=true;
                        other.gameObject.GetComponent<Collider>().enabled=true;
                        Debug.Log("obje açıldı = "+ other.gameObject.name);
                    }
            }        
    }
