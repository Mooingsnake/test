using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{

    //GenerateMap needs
    public GameObject initTile;//to lock the object(Tile),later will be deleted
    public float tileOffset = 2.00f;
    public Dictionary<Vector3, Tile> map;



    //ActiveCharacter,there is only one character activate in a moment  ？需要做成单例么？
    //change when its motion is over.
    public GameObject activeCharacter;
    [SerializeField]private GameObject fastestCharacter;  // 还没用，感觉应该每次选择next role的时候抽出来，而不是在这里设置变量
    public List<GameObject> initCharacters = new List<GameObject>(); // 还没用
    [SerializeField]private Vector3 bornPos; // born position,different from every map;saved by a file，还没用
    private TurnBaseController turnBaseCtl;  

    void Awake()
    {

    }
    void Start()
    {
      GetTurnBaseCtl();  // GameObject.Find("TurnBaseManager");找到那个空gameobject
      turnBaseCtl.map = map; // 给控制器挂上map信息方便查找，不过这个map还没用起来
      GenerateMap();  // 地图生成
      GenerateCharacters();
      ActivateCharacter();  // 这里要改，用了fastestPlayer这个变量，playermoves改成激活态
    }


    void Update()
    {
      
    }
    public void GenerateMap()
     {
        // GameObject map =GameObject.Find("Map");
       initTile = GameObject.FindGameObjectWithTag("Tile");
        map = new Dictionary<Vector3, Tile>();
       for(int x = 0; x < 20; x++)
       {
         for (int z = 0; z < 20; z++)
         {
           if (x==4) {// 凸出一列测试走路
              InitTile(x, 1, z);
           }
              InitTile(x, 0, z);
           }
       }
       Destroy(initTile);
     }

     public void GenerateCharacters()
     {
    // needs to change when many roles loaded
    // initCharacters.add(GameObject.FindGameObjectWithTag("Jaye"));
       Vector3 pos = new Vector3(0.0f,1.00f,0.0f);
        float maxSpeed = 0f;

       foreach(GameObject character in initCharacters){
            GameObject new_character =(GameObject)Instantiate(character, pos, Quaternion.identity);
            new_character.AddComponent<CharacterController>();
            CharacterController controller = new_character.GetComponent<CharacterController>();
            controller.center = new Vector3(0, 1, 0);
            controller.skinWidth = 0.0001f;
            new_character.AddComponent<PlayerMoves>();
            Destroy(character);
            //after judge who is the fastest
            pos.z +=2;//改成和tile对应是不是会比较好，tile抽象出来变成坐标，然后就只要对坐标操作就好
            if (maxSpeed<new_character.GetComponent<CharacterProperties>().ch_Speed) {
              maxSpeed = new_character.GetComponent<CharacterProperties>().ch_Speed;
              fastestCharacter = new_character;
            }
       }

     }
     public void ActivateCharacter()
     {
       //select the fastestCharacter to activate
       activeCharacter = fastestCharacter;
       PlayerMoves pm = activeCharacter.GetComponent<PlayerMoves>();
       pm.activated = true;
     }

    private void InitTile(float x, float y, float z)
    {

        // 根据位置生成tile
        Vector3 pos = new Vector3(x * tileOffset, y * tileOffset, z * tileOffset);
        Tile tile= Instantiate(initTile, pos, Quaternion.identity).GetComponent<Tile>();

        // tile都放在字典里面
        map.Add(pos, tile);

        // 把回合控制器给我接上去
        tile.turnBaseCtl = turnBaseCtl;
    }

    private void GetTurnBaseCtl()
    {
        //Manager是专门用在editor界面的名称
        GameObject turnBaseManager = GameObject.Find("TurnBaseManager");
        turnBaseCtl = turnBaseManager.GetComponent<TurnBaseController>();
    }
}
