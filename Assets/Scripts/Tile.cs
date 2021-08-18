using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Settings")]
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public TurnBaseController turnBaseCtl;//在地图生成器中生成时被赋值

    public List<Tile> adjacencyList = new List<Tile>();

    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public Collider[] colliders ;
    public Material planeMat;

    void Start()
    {
        //【warning】本来想放在tile初始化的时候同时绑定的，这样就不用遍历寻找了，但是我没找到正确的函数设置子级
          var re = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer render in re)
        {
            if (render.materials[0].HasProperty("_ModeType"))
            {
                planeMat = render.materials[0];
                return;
            }
        }
    }

    void Update()
    {
        //get the turnbaseController
        SetModeType(turnBaseCtl.modeType);

        //shader by the tile's condition
        if (current)
        {
             planeMat.SetFloat("_IsCurrent", 1.0f);
        }
        else if (selectable)
         {
             planeMat.SetFloat("_IsSelectable", 1.0f);
        }
        else
        {
            //no special -> alpha
            ResetShader();
        }
    }
    //Reset the tile to default state
    public void Reset()
    {
        adjacencyList.Clear();
        walkable = true;
        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
        gameObject.GetComponent<BoxCollider>().size =Vector3.one;
    }

    public void FindNeighbors(float jumpHeight)
    {
      Reset();

      CheckTile(Vector3.forward, jumpHeight);
      CheckTile(-Vector3.forward, jumpHeight);
      CheckTile(Vector3.right, jumpHeight);
      CheckTile(-Vector3.right, jumpHeight);

    }

    public void CheckTile(Vector3 direction, float jumpHeight)
    {
      Vector3 halfExtents = new Vector3(0.25f,(1 + jumpHeight)/ 2.0f, 0.25f);
      colliders = Physics.OverlapBox(transform.position + direction, halfExtents);//一个长方体，长宽高是halfextents*2
      foreach (Collider item in colliders)
      {
        Tile tile = item.GetComponent<Tile>();
        if (tile != null && tile.walkable&&tile.gameObject!=gameObject ) {//not null,is walkable,not itself
          RaycastHit hit;
          if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit)) {
              adjacencyList.Add(tile);
          }
        }
      }
    }

    private void ResetShader()
    {
        planeMat.SetFloat("_IsCurrent", 0f);
        planeMat.SetFloat("_ModeType", 0f);
        planeMat.SetFloat("_IsSelectable", 0f);
    }

    private void SetModeType(int typeMode)
    {
        planeMat.SetFloat("_ModeType", (float)typeMode);
    }







}
