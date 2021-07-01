using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
1.in cursor mode(游标模式),WASD select tile(or character when he is here)
2.in move mode(行动模式),active character move
3.in conbat mode

*/
public class PlayerMoves : TaticsMoves
{
    public  bool activated = false;//only  activated one can move in one turn
    private bool flag = false;

    void Start()
    {
      Init();
      GameObject gObj = GameObject.Find("TurnBaseManager");
      turnBaseCtl = gObj.GetComponent<TurnBaseController>();
    }
    void Update()
    {       // however, the fact is ,when object is moving ,the range cant move.
      if (activated) {
        if (!flag) {// show the moving range ,alike 上升沿触发
          FindSelectableTiles();
          flag = true; //set false when its turn is over
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
     // if(Input.GetKeyUp(KeyCode.A)){// go to confirm mode
     //    //confirm movement.set activated false, begin to attack/drink potions
     //    turnBaseCtl.modeType = TurnBaseController.CombatMode;
     //    flag = false;
     //    activated = false;//reboot after CombatMode is over,and set true in TurnBaseController.cs
     // }
     // if (Input.GetKeyUp(KeyCode.B)) {//go to cursor mode.
     //    flag = false;
     //    activated = false;
     //    turnBaseCtl.modeType = TurnBaseController.CursorMode;
     //  //  go to cursor mode.
     // }
     //  if(Input.GetKeyUp(KeyCode.Y)){   //  MoveBack();
     //    MoveBack();
     //  }
      if (Input.GetAxis("Vertical")!=0 ||Input.GetAxis("Horizontal") != 0) {
              moving = true;
      }
    }
}
