using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
1.in cursor mode(游标模式),WASD select tile(or character when he is here)
2.in move mode(行动模式),active character move


*/
public class PlayerMoves : TaticsMoves
{
    public  bool activated = false;//only  activated one can move in one turn
    private bool flag = false;
    void Start()
    {
      Init();
    }


    void Update()
    {       // however, the fact is ,when object is moving ,the range cant move.
      if (activated) {
        if (!flag) {
          FindSelectableTiles();// show the moving range ,alike 上升沿触发
          flag = true; //回合结束的时候记得设置回来为false
        }


        if (!moving) {//while isnt moving(moving is in TaticsMoves.cs),set false when in Idle()
          CheckInput();
        }
        else{
          //todo: moving
          Move();
        }

      }
    }
    public void CheckInput()//listening for an input "B"means to CursorMode,"A" means confirm to set and begin to attack or other
    {
    //  if(press"a"){
        //end the moving, begin to attack/drink potions,it's in MapGenerator.cs
        //flag = false;
        //activated = false;
    //  }
    //  if (press"b") {  //go to cursor mode.
        //flage = false;
        //activated = false;
        //go to cursor mode.
    //  }
    // if(press"y"){   //  MoveBack();
    //   MoveBack();
    // }
      if (Input.GetAxis("Vertical")!=0 ||Input.GetAxis("Horizontal") != 0) {
              moving = true;
      }
    }
    //in Update()
    public void MoveToTileCenter()
    {

    }

}
