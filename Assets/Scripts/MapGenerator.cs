using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{

    //GenerateMap needs
    public GameObject initTile;//to lock the object(Tile),later will be deleted
    public float tileOffset = 2.00f;
    public GameObject map;

    //MoveMode & CursorMode
    public int modeType;
    [SerializeField] private const int MoveMode = 1;
    [SerializeField] private const int CursorMode = 2;

    //ActiveCharacter,there is only one character activate in a moment
    //changes when its motion is over.
    public GameObject activeCharacter;
    public List<GameObject> initCharacters = new List<GameObject>();
    [SerializeField]private Vector3 bornPos;//born position,different from every map;saved by a file

    void Awake()
    {

    }
    void Start()
    {
      GenerateMap();
      modeType = 2;//set to CursorMode first
      GenerateCharacters();
    }


    void Update()
    {
      //todo: switch the mode
    }
    public void GenerateMap()
     {
        GameObject map =GameObject.Find("Map");
       initTile = GameObject.FindGameObjectWithTag("Tile");
       Vector3 pos ;
       GameObject tile;
       for(int x = 0; x < 20; x++)
       {
         for (int y = 0; y < 20; y++)
         {
           if (x==4) {
             pos = new Vector3(x * tileOffset, tileOffset, y * tileOffset);
             tile =(GameObject)Instantiate(initTile, pos, Quaternion.identity);
             tile.transform.parent = map.transform;
           }
           pos = new Vector3(x * tileOffset, 0, y * tileOffset);
           tile =(GameObject)Instantiate(initTile, pos, Quaternion.identity);
              tile.transform.parent = map.transform;

         }
       }
       Destroy(initTile);
     }

     public void GenerateCharacters()
     {
       //needs to chanege when many roles loaded
    //   initCharacters.add(GameObject.FindGameObjectWithTag("Jaye"));
       Vector3 pos = new Vector3(0.0f,1.00f,0.0f);
       foreach(GameObject character in initCharacters){
            GameObject new_character =(GameObject)Instantiate(character, pos, Quaternion.identity);
            new_character.AddComponent<CharacterController>();
            CharacterController controller = new_character.GetComponent<CharacterController>();
            controller.center = new Vector3(0, 1, 0);
            controller.skinWidth = 0.0001f;
            new_character.AddComponent<PlayerMoves>();
            Destroy(character);
            pos.z ++;
       }



       //1.001 for raycast,see in TaticsMoves.cs
     }
}
