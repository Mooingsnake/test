using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnBaseController : MonoBehaviour
{
  //frequently called by other scripts,so attributes are public
    //record character's pos ,to go back
    public Vector3 originalPos;
    public Quaternion originalRot;

    //MoveMode & CursorMode
     public int modeType;
     public   const int MoveMode = 1;
     public   const int CursorMode = 2;
     public   const int CombatMode = 3;

     [SerializeField]private GameObject currentCharacter;//cant change unless next turn

    //only called when load the scene
    void Start()
    {
      modeType = CursorMode;
    }

    // Update is called once per frame
    void Update()
    {
      switch(modeType){
          case MoveMode:

              break;
          case CursorMode:

              break;
          default:

              break;
      }
    }
}
