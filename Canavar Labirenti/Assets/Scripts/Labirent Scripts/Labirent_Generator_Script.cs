using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labirent_Generator_Script : MonoBehaviour
{
    [SerializeField] Labirent_Room  Labirent_Room_Prefab;
    [SerializeField] Vector2Int Labirent_size;

    private void Start()
        {
            StartCoroutine(Generate_Labirent(Labirent_size));

        }

        IEnumerator Generate_Labirent(Vector2Int size)
            {
                List<Labirent_Room> rooms = new List<Labirent_Room>();

                //Creating rooms

                for (int x = 0; x < size.x; x++)
                    {

                     for(int y = 0; y < size.y; y++)
                        
                        {
                            Vector3 Room_position   =   new Vector3( x-(size.x/2f) , 0 , y-(size.y/2f) );
                            Labirent_Room newRoom   =   Instantiate(Labirent_Room_Prefab , Room_position , Quaternion.identity , transform );
                            rooms.Add(newRoom);
                            Debug.Log("Yeni Room Pozisyonu" + Room_position);

                            yield return null;
                        }   
                    }
                
                List<Labirent_Room> currentPath     =   new List<Labirent_Room>();
                List<Labirent_Room> complatedRooms  =   new List<Labirent_Room>();

                //choose startin room
                currentPath.Add(rooms[Random.Range(0, rooms.Count)]);

                currentPath[0].SetState(Labirent_Room_State.Current);

                while (complatedRooms.Count < rooms.Count)
                    {
                        //check rooms next to the current room

                        List<int>possibleNextRooms      =   new List<int>();
                        List<int>possibleNextDirections =   new List<int>();

                        int currentRoomIndex            =   rooms.IndexOf(currentPath[currentPath.Count-1]);
                        int currentRoomX                =   currentRoomIndex / size.y;
                        int currentRoomY                =   currentRoomIndex % size.y; 
                        
                        if  (
                                currentRoomX < size.x - 1
                            )

                            {
                                // Check Room to the right of the current room
                                if  (
                                        !complatedRooms.Contains(rooms[currentRoomIndex+size.y])
                                        &&
                                        !currentPath.Contains(rooms[currentRoomIndex+size.y])
                                    )

                                    {
                                        possibleNextDirections.Add(1);
                                        possibleNextRooms.Add(currentRoomIndex+size.y);
                                    }
                            }


                        if (currentRoomX > 0)
                            {
                                // Check Room to the left of the current room
                                if  (
                                        !complatedRooms.Contains(rooms[currentRoomIndex-size.y]) 
                                        &&
                                        !currentPath.Contains(rooms[currentRoomIndex - size.y])
                                    )

                                    {
                                        possibleNextDirections.Add(2);
                                        possibleNextRooms.Add(currentRoomIndex - size.y);
                                    }
                            }
                        if (currentRoomY < size.y - 1)
                            {
                                // Check Room above the current room
                                if  (
                                        !complatedRooms.Contains(rooms[currentRoomIndex + 1])
                                        &&
                                        !currentPath.Contains(rooms[currentRoomIndex + 1])
                                    )

                                    {
                                        possibleNextDirections.Add(3);
                                        possibleNextRooms.Add(currentRoomIndex + 1);
                                    }
                            }

            if (currentRoomY > 0)
                {
                    // Check node below the current room
                    if  (
                            !complatedRooms.Contains(rooms[currentRoomIndex - 1]) 
                            &&
                            !currentPath.Contains(rooms[currentRoomIndex - 1])
                        )

                        {
                            possibleNextDirections.Add(4);
                            possibleNextRooms.Add(currentRoomIndex - 1);
                        }
                }

            // Choose next room
            if (possibleNextDirections.Count > 0)
                {
                    int chosenDirection = Random.Range(0, possibleNextDirections.Count);
                    Labirent_Room chosenRoom = rooms[possibleNextRooms[chosenDirection]]; 

                    switch (possibleNextDirections[chosenDirection])
                        {
                            case 1:
                                chosenRoom.RemoveWall(1);
                                currentPath[currentPath.Count - 1].RemoveWall(0);
                                break;
                            case 2:
                                chosenRoom.RemoveWall(0);
                                currentPath[currentPath.Count - 1].RemoveWall(1);
                                break;
                            case 3:
                                chosenRoom.RemoveWall(3);
                                currentPath[currentPath.Count - 1].RemoveWall(2);
                                break;
                            case 4:
                                chosenRoom.RemoveWall(2);
                                currentPath[currentPath.Count - 1].RemoveWall(3);
                                break;
                        }
                

                    currentPath.Add(chosenRoom);
                    chosenRoom.SetState(Labirent_Room_State.Current);
                }
                else
                {
                    complatedRooms.Add(currentPath[currentPath.Count - 1]);

                    currentPath[currentPath.Count-1].SetState(Labirent_Room_State.Completed);

                    currentPath.RemoveAt(currentPath.Count-1);

                }

            yield return new    WaitForSeconds(0.05f);
        }
    }
}
