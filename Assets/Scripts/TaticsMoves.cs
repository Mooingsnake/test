using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaticsMoves : MonoBehaviour
{
  //Debug:

    [SerializeField]private GameObject hitColliderZ;
    [SerializeField]private GameObject hitColliderX;
    //Ingnore:    这部分别动了，没啥看头的
    private float runSpeed = 5;//调出来的通用移动速度，别改
    private Vector3 characterOffset;//就用了一次，在TargetTile()
    private float halfHeight = 0; //chche player'origin
    public  TurnBaseController turnBaseCtl;
    //Move and compute selectableTiles
    public int moveAbility ; //range of move
    public bool moving = false;//flag to show if tiles are change color
    public Vector3 direction = new Vector3();//((x,x,x))   x=0, -1, 1 (turn and show animation)
    public Vector3 velocity = new Vector3(0,0,0);//direction * speed (actually controller the transform)

    [SerializeField]private Tile currentTile = null;//chache when FindSelectableTiles()
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
    [SerializeField]private float jumpHeight = 2f; //y axis ;这是个单位，表示一个格子多高


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
       jumpHeight *= GetComponent<CharacterProperties>().JumpAbility ;
       moveAbility = GetComponent<CharacterProperties>().MoveAbility;
       halfExtents = new Vector3(jumpHeight,jumpHeight,jumpHeight);
       GameObject turnBaseManager = GameObject.Find("TurnBaseManager");
       turnBaseCtl = turnBaseManager.GetComponent<TurnBaseController>();

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
       gameObject.transform.position = turnBaseCtl.originalPos;
       gameObject.transform.rotation = turnBaseCtl.originalRot;
     }

     //in Update();
     //press WASD to Move,limit the range ,jump if need ,
     public void Move(){
       //gravity
       isGrounded = Physics.CheckSphere(transform.position, grounfCheckDistance);//(position,radius,masklayer)
        // 平移
       float moveZ = Input.GetAxis("Vertical");//-1 0 1
       float moveX = Input.GetAxis("Horizontal");//-1 0 1
       direction = new  Vector3(moveX,0,moveZ);//direction不包含垂直数值，因为之后还有水平面人物朝向问题
       velocity = new Vector3(moveX,velocity.y,moveZ);//velocity包含垂直数值和速度单位，垂直用g计算，水平用runspeed计算,是高中物理的向量概念
        // turn
        Vector3 headDirection = Vector3.RotateTowards(transform.forward, direction, 7*Time.deltaTime, 0.0f);
       transform.rotation = Quaternion.LookRotation(headDirection);//headDirection 水平朝向
       //apply
       if (isGrounded) {
         velocity.y = 0f;
         if (direction != Vector3.zero)
         {
           Run();//including jumping
         }else
         {               //check if it's on center of one tile
            RaycastHit hit;
            Physics.Raycast(transform.position+Vector3.up*10f,-Vector3.up,out hit);
             Tile tile = hit.collider.GetComponent<Tile>();
            Vector3 posA= Projection(tile.transform.position);
            Vector3 posB = Projection(transform.position);
              if (Vector3.Distance(posA,posB)>0.1f) {
                controller.Move((posA - posB) * Time.deltaTime*5.0f);
              }
               else{
                 Idle();
               }
         }
       }else
       {
           velocity.y += gravity * Time.deltaTime; //v = 1/2 g t^2
       }
        Vector3 heading  = new Vector3(velocity.x*runSpeed,velocity.y,velocity.z*runSpeed);
        controller.Move(heading * Time.deltaTime);
     }

     //in Update();
     //including run animation,and move limitation ,and jump animation
     public void Run()
     {
       //如果相安无事，就跑
      animator.SetBool("isRunning",true);
      animator.SetBool("isJumping",false);
      RaycastHit hit;
      bool isOndirection = Physics.Raycast(transform.position, direction, out hit,2);//角色方向上有障碍物
      //if(z,x分别能走){判断要不要跳}
      //else{不能走}{分别置0}
      //如果x轴能走
      Vector3 barkout = transform.position+(NormalValueX(direction.x))+Vector3.up*10;
      if(Physics.Raycast(transform.position+(NormalValueX(direction.x))+Vector3.up*10,-Vector3.up,out hit))
      {
        bool whiteTileOnX = !hit.collider.GetComponent<Tile>().selectable;
        if (whiteTileOnX) {//不能走，再看看z轴
          velocity.x = 0;
        }else{//能走，判断要不要跳
          if(isOndirection&&Vector3.Distance(hit.collider.transform.position, transform.position+Vector3.up)< 2.1f)//一个长方体，长宽高是halfextents*2)//collider from Horizontal
            {
                Jump(Math.Abs(hit.collider.transform.position.y-transform.position.y)+1.6f);//deltaHeight between collider and transform
            }
        }
      }else{velocity.x = 0;}
      //如果z能走
       if(Physics.Raycast(transform.position+(NormalValueZ(direction.z))+Vector3.up*10,-Vector3.up,out hit))
      {
      //  Debug.Log("transform.position+(direction.z>0?Vector3.forward:-Vector3.forward)"+transform.position+(direction.z>0?Vector3.forward:-Vector3.forward));
          bool whiteTileOnZ = !hit.collider.GetComponent<Tile>().selectable;
        if (whiteTileOnZ) {//不能走，结束函数
          velocity.z = 0;
          return ;
        }else{//能走,判断要不要跳
          if(isOndirection&&Vector3.Distance(hit.collider.transform.position, transform.position+Vector3.up)< 2.1f)//一个长方体，长宽高是halfextents*2)//collider from Horizontal
            {
                Jump(Math.Abs(hit.collider.transform.position.y-transform.position.y)+1.6f);//deltaHeight between collider and transform
            }
        }
      }else{  velocity.z = 0;}
   }
     public void Idle(){
       moving = false;
       animator.SetBool("isRunning",false);
       animator.SetBool("isJumping",false);
     }
     public void Jump(float deltaHeight)//s=sqrt(2gh)
     {
       animator.SetBool("isRunning",false);
       animator.SetBool("isJumping",true);
        velocity.y = Mathf.Sqrt((deltaHeight)  * -2f * gravity);
     }

     private Vector3 NormalValueX(float a){
       if (a==0) {
         return Vector3.zero;
       }else if (a>0) {
         return Vector3.right;
       }else{
         return Vector3.left;
       }
     }
     private Vector3 NormalValueZ(float a){
       if (a==0) {
         return Vector3.zero;
       }else if (a>0) {
         return Vector3.forward;
       }else{
         return Vector3.back;
       }
     }
     private Vector3 Projection(Vector3 vect3){
       vect3.y = 0;
       return vect3;
     }
}
