using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaticsMoves : MonoBehaviour
{
  //Debug:



    //Move and compute selectableTiles
    public int moveAbility = 5; //range of move

    private float halfHeight = 0; //chche player'origin
    private Vector3 characterOffset;
    public bool moving = false;//flag to show if tiles are change color
    public Vector3 direction = new Vector3();//((x,x,x))   x=0, -1, 1
    public Vector3 yVelocity = new Vector3(0,0,0);//direction * speed
    [SerializeField] private float runSpeed = 5;
    public Tile currentTile = null;//chache when FindSelectableTiles()

    public Stack<Tile> path = new Stack<Tile>();
    public List<Tile> selectableTiles = new List<Tile>();
    public GameObject[] tiles = null;

    //controll the character
    private CharacterController controller;
    private Vector3 halfExtents;
    //animator
    private Animator animator;

    //gravity
    [SerializeField]private bool isGrounded;
    [SerializeField]private float grounfCheckDistance = 0.2f;
    [SerializeField]private LayerMask groundMask = 2;
    [SerializeField]private float gravity = -9.81f;

    //jump
    [SerializeField]private float jumpHeight = 1f; //y axis ;1 per tile

    //cache the height,
    // tiles List
    //currentTile where the gameObject(assigned by) is .
    //combine CharacterController,Animator
    protected void Init()
     {
       halfHeight = GetComponent<Collider>().bounds.extents.y *0.5f;
       characterOffset = new Vector3(0,halfHeight,0);
       tiles =  GameObject.FindGameObjectsWithTag("Tile");
       FindCurrentTile();
       controller = GetComponent<CharacterController>();
       animator = GetComponentInChildren<Animator>();
       halfExtents = new Vector3(halfHeight,halfHeight,halfHeight);
     }

     public void FindCurrentTile()
     {
       Tile tile =  FindTargetTile(gameObject);
       currentTile = tile;
       currentTile.current = true;
     }

     public void ComputeAdjacencyList()
     {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
       foreach(GameObject t in tiles)
       {
         Tile tile = t.GetComponent<Tile>();
         tile.FindNeighbors(jumpHeight);
       }
     }

     public Tile FindTargetTile(GameObject target)
     {
       RaycastHit hit;
       Tile tile = null;
      // target.transform.position + characterOffset = (0,2,0)
       if (Physics.Raycast(target.transform.position + characterOffset , -Vector3.up, out hit))
       // to avoid cast itself
       {
         tile = hit.collider.GetComponent<Tile>();

       }
       return tile;
     }

     public void FindSelectableTiles()
     {
       ComputeAdjacencyList();//? 需要吗?
       FindCurrentTile();
       Queue<Tile> process = new Queue<Tile>();
       process.Enqueue(currentTile);
       currentTile.selectable = true;

       while(process.Count > 0){
          Tile tile =  process.Dequeue();
          tile.visited = true; //
          tile.selectable = true;
          selectableTiles.Add(tile);
          if (tile.distance < moveAbility) {
            foreach(Tile t in tile.adjacencyList)
            {
                if (!t.visited) {
                  process.Enqueue(t);

                  t.parent = tile;
                  t.distance = tile.distance + 1+(int)t.transform.position.y;
              }
            }
          }
       }
     }

     public void MoveToTarget(Tile tile)
     {
       path.Clear();
       tile.target = true;
       moving = true;

       while(tile.parent != null)
       {
         tile = tile.parent;
         path.Push(tile);
       }
     }

     //back to this turn's original place
     public void MoveBack(){

     }
     //press WASD to Move,limit the range ,jump if need ,
     public void Move(){
       //gravity
       isGrounded = Physics.CheckSphere(transform.position, grounfCheckDistance);//(position,radius,masklayer)
       // 平移
       float moveZ = Input.GetAxis("Vertical");//-1 0 1
       float moveX = Input.GetAxis("Horizontal");//-1 0 1
       direction = new  Vector3(moveX,0,moveZ);//direction不包含垂直数值，因为之后还有水平面人物朝向问题
       yVelocity = new Vector3(0,yVelocity.y,0);//velocity包含垂直数值和速度单位，垂直用g计算，水平用runspeed计算,是高中物理的向量概念
        // turn
        Vector3 headDirection = Vector3.RotateTowards(transform.forward, direction, 7*Time.deltaTime, 0.0f);
       transform.rotation = Quaternion.LookRotation(headDirection);//headDirection 水平朝向
       //apply
       if (isGrounded) {
         yVelocity.y = 0;
         if (direction != Vector3.zero)
         {
           Run();
         }else
         {
           Idle();
         }
       }else
       {
           yVelocity.y += gravity * Time.deltaTime; //v = 1/2 g t^2
       }
        Vector3 heading  = new Vector3(direction.x*runSpeed,yVelocity.y,direction.z*runSpeed);
        controller.Move(heading * Time.deltaTime);
     }

     //including run animation,and move limitation ,and jump animation
     public void Run()
     {
      animator.SetBool("isRunning",true);
      //   along the direction ,if you collide the right cube, you jump
      RaycastHit hit;
      bool isOndirection = Physics.Raycast(transform.position, direction, out hit,2);
      if (isOndirection) {
        Debug.Log(hit.collider.transform.position+"hit.collider.transform.position");
        Debug.Log(transform.position+"transform.position");
      }
      if(isOndirection&&Vector3.Distance(hit.collider.transform.position, transform.position)< 2.1f)//一个长方体，长宽高是halfextents*2)//collider from Horizontal
      {
        if (hit.collider.GetComponent<Tile>().selectable) {
          Jump();
        }
      }
      //only walk on walkable tiles
      Collider[] colliders  = Physics.OverlapBox(transform.position, halfExtents);
      for(int i =0;i<colliders.Length;i++){
        if(colliders[i].GetComponent<Tile>()!=null&&!colliders[i].GetComponent<Tile>().selectable)
        {
          Vector3 fenceScale = new Vector3(1f,20f,1f);
          colliders[i].GetComponent<BoxCollider>().size =fenceScale ;
        }
      }
     }
     public void Idle(){
       // if (Condition) {//not on center of the tile
       //   //step to the nearest tile center
       // }
       moving = false;
       animator.SetBool("isRunning",false);
       animator.SetBool("isJumping",false);
     }
     public void Jump()//s=sqrt(2gh)
     {
       animator.SetBool("isRunning",false);
       animator.SetBool("isJumping",true);
        yVelocity.y = Mathf.Sqrt(jumpHeight * 2f * -2f * gravity);
     }


}
