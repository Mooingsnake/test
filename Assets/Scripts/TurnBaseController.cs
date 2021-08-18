using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnBaseController : MonoBehaviour
{
    // frequently called by other scripts,so attributes are public
    // record character's pos ,to go back, with its towards: originalRot
    public Vector3 originalPos;
    public Quaternion originalRot;

    // targetTile ,updated for movemode,cursormode,combatmode
    public Tile targetTile;
    public Dictionary<Vector3, Tile> map;

    // MoveMode & CursorMode
     public int modeType;
     public   const int MoveMode = 1;
     public   const int CursorMode = 2;
     public   const int CombatMode = 3;

     [SerializeField]private GameObject currentCharacter;// cant change unless next turn

    void Start()
    {
        
        modeType = MoveMode;
    }

    void Update()
    {
        targetTile = GetTargetTile(currentCharacter);
    }

    //  get tile that is selected in different mode
    Tile GetTargetTile(GameObject currentCharacter)
    {
        Tile targetTile;
        switch (modeType)
        {
            case MoveMode:
                targetTile = GetTargetTile_MoveMode();
                break;
            case CursorMode:
                targetTile = GetTargetTile_CursorMode();
                break;
            default:
                targetTile = GetTargetTile_CursorMode(); // same as Cursormode,so there be.
                break;     
        }
        return targetTile;       
    }

    // get tile that character is standing
    Tile GetTargetTile_MoveMode()
    {
        currentCharacter = GetCurrentCharacter();
        Physics.Raycast(currentCharacter.transform.position + Vector3.up * 10f, -Vector3.up, out RaycastHit hit);
        Tile tile = hit.collider.GetComponent<Tile>();
        return tile;
    }

    // get tile that can be switched by WASD or mouse click
    Tile GetTargetTile_CursorMode()
    {
        Tile targetTile = new Tile();
        return targetTile;
    }

    // get current character 
    GameObject GetCurrentCharacter()
    {
        GameObject current  = GameObject.FindGameObjectWithTag("Player");
        return current;
    }



}
