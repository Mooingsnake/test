using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaticsMoves : MonoBehaviour
{
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
    //animator
    private Animator animator;

    //gravity
    [SerializeField]private bool isGrounded;
    [SerializeField]private float grounfCheckDistance = 0.2f;
    [SerializeField]private LayerMask groundMask = 2;
    [SerializeField]private float gravity = -9.81f;

    //jump
    [SerializeField]private float jumpHeight = 2; //y axis

    //cache the height,
    // tiles List
    //currentTile where the gameObject(assigned by) is .
    //combine CharacterController,Animator
    protected void Init()
     {
       halfHeight = GetComponent<Collider>().bounds.extents.y;
       characterOffset = new Vector3(0,halfHeight,0);
       tiles =  GameObject.FindGameObjectsWithTag("Tile");
       FindCurrentTile();
       controller = GetComponent<CharacterController>();
       animator = GetComponentInChildren<Animator>();
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
       // if (isGrounded && velocity.y < 0) {//stop falling when grounded
       //   velocity.y = 0;
       // }

       // 平移
       float moveZ = Input.GetAxis("Vertical");//-1 0 1
       float moveX = Input.GetAxis("Horizontal");//-1 0 1
       direction = new  Vector3(moveX,0,moveZ);//direction不包含垂直数值，因为之后还有水平面人物朝向问题
       yVelocity = new Vector3(0,yVelocity.y,0);//velocity包含垂直数值和速度单位，垂直用g计算，水平用runspeed计算,是高中物理的向量概念
        // turn
        Vector3 headDirection = Vector3.RotateTowards(transform.forward, direction, 7*Time.deltaTime, 0.0f);
       transform.rotation = Quaternion.LookRotation(headDirection);//headDirection 水平朝向
            //  Debug.Log(isGrounded);
       if (isGrounded) {
         yVelocity.y = 0;
         if (direction != Vector3.zero)
         {
          // Debug.Log("run");
           Run();
         }else
         {
           Idle();
           moving = false;
         }
       }else{
           yVelocity.y += gravity * Time.deltaTime; //v = 1/2 g t^2
       }
      Vector3 heading  = new Vector3(direction.x*runSpeed,yVelocity.y,direction.z*runSpeed);
  //      controller.Move(direction * Time.deltaTime);
    //    if (isGrounded ) {//stop falling when grounded
    // //     velocity.y = 0;
    //    }else{
    //       // velocity.y += gravity * Time.deltaTime; //v = 1/2 g t^2
    //    }
        controller.Move(heading * Time.deltaTime);
     }

     //including animation,and controller.move
     public void Run()
     {
    //   Jump();if（colider a new collider on x,z axis）
      animator.SetBool("isRunning",true);
      RaycastHit hit;
      bool isOndirection = Physics.Raycast(transform.position + characterOffset , direction, out hit);
      if(isOndirection&&Vector3.Distance(hit.collider.transform.position, transform.position)< 0.2f)//一个长方体，长宽高是halfextents*2)//collider from Horizontal
      {
        Jump();
      }
     }
     public void Idle(){
       animator.SetBool("isRunning",false);
       animator.SetBool("isJumping",false);
     }
     public void Jump()//s=sqrt(2gh)
     {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
     }


}
