using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionKoorScript : MonoBehaviour
{
    public Transform SelectionPoint;
    private Vector3 base_position;
    private readonly Cam_Kroki_script SelectionKoor;

    // Start is called before the first frame update
    void Start()
    {
        base_position = SelectionPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        SelectionPoint.position = SelectionKoor; 
        Debug.Log("Slection Point Position = " + transform.position);
    }
}
