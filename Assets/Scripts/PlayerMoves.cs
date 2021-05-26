using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
1.in cursor mode(游标模式),WASD select tile(or character when he is here)
2.in move mode(行动模式),active character move


*/
public class PlayerMoves : TaticsMoves
{
    [SerializeField]private bool activated = true;//only  activated one can move in one turn

    void Start()
    {
      Init();
    }


    void Update()
    {       // however, the fact is ,when object is moving ,the range cant move.
      if (activated) {
        FindSelectableTiles();//always show the moving range
        activated = false;
      }
      if (!moving) {//while isnt moving(moving is in TaticsMoves.cs)

        CheckInput();//listening for a input
      }
      else{
        //todo: moving
        Move();
      }
    }
    public void CheckInput()//listening for wasd or other “backspace/shift??”
    {
      // mouseClick ver.
      // if(Input.GetMouseButtonUp(0))//
      // {
      //   //get the click position
      //   Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      //   RaycastHit hit;
      //     //select a tile to targetTile
      //   if (Physics.Raycast(ray, out hit)) {
      //     Tile tile = null;
      //     tile = hit.collider.GetComponent<Tile>();
      //     if (tile.selectable) {//judge the tile is selectable
      //       MoveToTarget(tile);
      //     }
      //   }
      // }
      if (Input.GetAxis("Vertical")!=0 ||Input.GetAxis("Horizontal") != 0) {
              moving = true;
      }

    }

}
