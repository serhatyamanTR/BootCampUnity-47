using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Labirent_Room_State
{
    Available,
    Current,
    Completed
}

public class Labirent_Room : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer zemin;

    public void RemoveWall(int wallToRemove)
        {
            walls[wallToRemove].gameObject.SetActive(false);
        }
        

    public void SetState(Labirent_Room_State state)
        {
            switch (state)
                {
                    case   Labirent_Room_State.Available:
                       {
                           zemin.material.color   =   Color.white;
                           break;
                       }
                    case   Labirent_Room_State.Current:
                        {
                            zemin.material.color   =   Color.magenta;
                            break;
                        }
                    case   Labirent_Room_State.Completed:
                    {
                        zemin.material.color   =   Color.grey;
                        break;
                    }
                
                }
        }
}   