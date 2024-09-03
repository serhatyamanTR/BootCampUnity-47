using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class Labirent_Generator_Script : MonoBehaviour
{
    [SerializeField] public Labirent_Room  Labirent_Room_Prefab;
    [SerializeField] public Vector2Int Labirent_size;
    [SerializeField] public float RoomSize;

    [SerializeField] private bool isThisCreatedLabirent; //üretilen labirentlerin scriptlerini tekrar tekrar çalışmasın diye
    public float waitTime = 0f;
    public List<Labirent_Room> rooms = new List<Labirent_Room>();
    public List<Labirent_Room> deadEnds = new List<Labirent_Room>();
    public bool isSeedUsed = false; // Control whether to use the seed or not
    public int seed = 0; //default seed numarası

    //----------navmesh değişkenleri
    public Vector3 volumeCenter;  // Volume'un merkez noktası
    public Vector3 volumeSize;    // Volume'un boyutu
    public NavMeshCollectGeometry collectGeometry; // Toplanacak objelerin türü

    private void Start()
        {
            if(isSeedUsed)
                {
                    InitSeed(seed);
                }
            
            StartCoroutine(Generate_Labirent(Labirent_size));
            
            

        }

    private void ConfigureNavMesh()
    {
        NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();

        if (navMeshSurface != null)
        {
            // CollectObjects ayarı
            navMeshSurface.useGeometry = collectGeometry;

            //
            volumeSize = new Vector3(Labirent_size.x, 0.3f, Labirent_size.y) * RoomSize;
            volumeCenter = new Vector3(0, 0.1f, 0);
            navMeshSurface.size = volumeSize;
            navMeshSurface.center = volumeCenter;

            // NavMesh'i oluştur
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found on the LabirentGeneratorObject.");
        }
    }

    
    void InitSeed(int seed)
        {
            Random.InitState(seed);
        }

    IEnumerator Generate_Labirent(Vector2Int size)
        {
            if  (
                    !(  
                        size.x%2 == 0 
                        ||
                        size.y%2 == 0 
                        ||
                        isThisCreatedLabirent
                    )
                ) //generator x ve y boyutlarına tek sayı yazılırsa çalışır
                {
                    //Creating rooms

                    for (int x = 0; x < size.x; x++)
                        {

                            for(int y = 0; y < size.y; y++)
                            
                            {
                                Vector3 Room_position   =   new Vector3(
                                                                            (0.5f+x-(size.x/2f))*RoomSize,   //odanın x koordinatı
                                                                            0 ,                         //odanın y koordinatı
                                                                            (0.5f+y-(size.y/2f))*RoomSize    //odanın z koordinatı
                                                                        ) + transform.position;         //odaların koordinatlarını ana objenin lokasyonu kadar öteliyoruz ki objennin olduğu yerde üretilsin
                                Labirent_Room newRoom   =   Instantiate(Labirent_Room_Prefab , Room_position , Quaternion.identity , transform );
                                newRoom.transform.localScale = Vector3.one*RoomSize;
                                
                                rooms.Add(newRoom);
                                Debug.Log("Oluşturulan Oadanın indexi = " +rooms.IndexOf(newRoom));
                                newRoom.name = "Labirent_room (Clone) ("+(rooms.IndexOf(newRoom)+1)+")";
                                yield return null;
                            }   
                        }
                    
                    List<Labirent_Room> currentPath     =   new List<Labirent_Room>();
                    List<Labirent_Room> complatedRooms  =   new List<Labirent_Room>();
                    
                    //choose base rooms, base roomlarının duvarları kapatılacak

                    /*
                    base rooms with baseRoomIndex:

                                    [0][3][6]
                                    [1][4][7]
                                    [2][5][8]

                    */
                    List<Labirent_Room> baseRooms       = new List<Labirent_Room>();
                    List<int> baseRoomIndexs = new List<int>();
                    for ( int i=0; i<=9; i++)
                        {
                            if( i<3 )
                                {
                                    baseRoomIndexs.Add((rooms.Count/2)-Labirent_size.y-1+i);
                                }
                                else if ( i<6 )
                                    {
                                        baseRoomIndexs.Add((rooms.Count/2)-4+i);
                                    }
                                    else if ( i<=9 )
                                        {
                                            baseRoomIndexs.Add((rooms.Count/2)+Labirent_size.y-7 + i);
                                        }
                        }
                    foreach (int i in baseRoomIndexs)
                        {
                            baseRooms.Add(rooms[i]);
                            Debug.Log("base odalasının indexi = "+i);
                        }



                    //Cleanin base walls
                            baseRooms[0].RemoveWall(0);
                            //baseRooms[0].RemoveWall(1);
                            baseRooms[0].RemoveWall(2);
                            //baseRooms[0].RemoveWall(3);
                            baseRooms[1].RemoveWall(0);
                            //baseRooms[1].RemoveWall(1);
                            baseRooms[1].RemoveWall(2);
                            baseRooms[1].RemoveWall(3);
                            baseRooms[2].RemoveWall(0);
                            //baseRooms[2].RemoveWall(1);
                            //baseRooms[2].RemoveWall(2);
                            baseRooms[2].RemoveWall(3);
                            baseRooms[3].RemoveWall(0);
                            baseRooms[3].RemoveWall(1);
                            baseRooms[3].RemoveWall(2);
                            //baseRooms[3].RemoveWall(3);
                            baseRooms[4].RemoveWall(0);
                            baseRooms[4].RemoveWall(1);
                            baseRooms[4].RemoveWall(2);
                            baseRooms[4].RemoveWall(3);
                            baseRooms[5].RemoveWall(0);
                            baseRooms[5].RemoveWall(1);
                            //baseRooms[5].RemoveWall(2);
                            baseRooms[5].RemoveWall(3);
                            //baseRooms[6].RemoveWall(0);
                            baseRooms[6].RemoveWall(1);
                            baseRooms[6].RemoveWall(2);
                            //baseRooms[6].RemoveWall(3);
                            //baseRooms[7].RemoveWall(0);
                            baseRooms[7].RemoveWall(1);
                            baseRooms[7].RemoveWall(2);
                            baseRooms[7].RemoveWall(3);
                            //baseRooms[8].RemoveWall(0);
                            baseRooms[8].RemoveWall(1);
                            //baseRooms[8].RemoveWall(2);
                            baseRooms[8].RemoveWall(3);
                    
                    //labirentin iki giriş kapılarını açıyoruz ve uygun şekilde duvarlarını deaktif ediyorum
                    int kuzeyExitIndex = Mathf.CeilToInt(Labirent_size.x/2f)*Labirent_size.y-1;
                    int guneyExitIndex = Mathf.FloorToInt(Labirent_size.x/2f)*Labirent_size.y;
                    
                    rooms[kuzeyExitIndex].RemoveWall(2); //kuzey kapısı
                    Debug.Log("Kuzey kapısının indexi = "+ kuzeyExitIndex);

                    rooms[guneyExitIndex].RemoveWall(3); //güney kapısı
                    Debug.Log("Güney kapısının indexi = "+ guneyExitIndex);
                    
                    //choose startin room
                    currentPath.Add(rooms[kuzeyExitIndex]);
                    currentPath[0].SetState(Labirent_Room_State.Current);
                    
                    


                    //yield return new WaitForSeconds(2);
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
                                    Debug.Log("Seçilen oda = " + rooms.IndexOf(chosenRoom));
                                    Debug.Log("Seçilen oda = " + chosenDirection);
                                    switch (possibleNextDirections[chosenDirection])
                                        {
                                            // 0 = x+,  1= x-,  2= z+,  3=z- //
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
                            if(waitTime > 0)
                                {
                                      yield return new WaitForSeconds(waitTime);
                                }

                              
                            
                        }
                        
                        IdentifyDeadEnds();

                        isThisCreatedLabirent = true; // labirent üretildikten sonra bu obje artık üretilmiş bir labirent olduğu için
                        transform.name = "Created_Labirent_"+size.x+"x"+size.y+"_"+seed;

                        ConfigureNavMesh(); //oluşturulan navmesh uyguluyorum

                        


                }
                else if (!isThisCreatedLabirent)
                {
                    Debug.LogError("Labirent Boyutuna Çift Sayı yazdınız, lütfen tek sayı yazınız.");
                }
                else if (isThisCreatedLabirent)
                    {
                        Debug.LogWarning("önceden üretilmiş bir labirent kullanıyorsunuz");
                        ConfigureNavMesh();
                    }
        }

    void IdentifyDeadEnds()
        {
            // deadEnds.Clear(); // Assuming deadEnds is also accessible class-wide
            foreach (Labirent_Room room in rooms) // Direct access to 'nodes'
                {
                    if (room.RoomActiveWallCount() == 3)
                        {
                            deadEnds.Add(room);
                        }
                }
        }
    public void RegenerateMaze()
        {
            foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                rooms.Clear();
                deadEnds.Clear();
                Generate_Labirent(Labirent_size);
        }
}
