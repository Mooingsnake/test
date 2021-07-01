using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;


    public List<Tile> adjacencyList = new List<Tile>();

    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public Collider[] colliders ;
    public Material planeMat;

    void Start()
    {
        // gameObject.AddComponent<Rigidbody>();
        // Rigidbody rigidbody = GetComponent<Rigidbody>();
        // rigidbody.useGravity = false;
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

    // Update is called once per frame
    void Update()
    {

        if (current)
        {
            //   GetComponent<Renderer>().material.color = Color.magenta;
             planeMat.SetFloat("_IsCurrent", 1.0f);
        }
        else if (selectable)
         {
            // GetComponent<Renderer>().material.color = Color.red;
             planeMat.SetFloat("_IsSelectable", 1.0f);
        }
        else
        {
            // GetComponent<Renderer>().material.color = Color.white;
            // planeMat.SetColor("_TileColor", Color.clear);
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




}
