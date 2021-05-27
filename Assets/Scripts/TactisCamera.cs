using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
1.follow the mousePosition
2.when active obj is moving ,should go back to the selected object and follow it
*/
public class TactisCamera : MonoBehaviour
{
   [SerializeField]private float cameraSpeed = 3;

  [SerializeField]private float minFov = 80f;
  [SerializeField]private float maxFov = 140f;
  [SerializeField]private float sentivity= 300f;
  [SerializeField]private float fov;

  [SerializeField]private Vector3 standardPos = new Vector3(-0.2f,4.9f,-1.2f);//(realtive pos between default character and camera)
  //follow the mousePosition,when out of screen
  void FixedUpdate ()
    {      //controll transform
      float mouseX = Input.mousePosition.x;
      float mouseZ = Input.mousePosition.y;
      Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
    //  Rect biggerRect = new Rect(0,, Screen.width*1.1f,Screen.height*1.1f);

      if (!screenRect.Contains(Input.mousePosition)){
      mouseX =  (mouseX - 0.5f * Screen.width)/Screen.width;
      mouseZ =  (mouseZ - 0.5f * Screen.height)/Screen.height;
      Vector3 direction = new Vector3(mouseX,mouseZ,0);
      transform.Translate(direction * cameraSpeed * Time.deltaTime);
      }
      //controll scale by mouseScroll
      fov = Camera.main.fieldOfView;
      fov+= Input.GetAxis("Mouse ScrollWheel") * sentivity;
      fov = Mathf.Clamp(fov, minFov,maxFov);
      Camera.main.fieldOfView = fov;

      //come back to active character's pos by  mouse right
      if (Input.GetMouseButtonUp(1)) {
      //  gameObject.transform.position  = GameObject.Find("Map").GetComponent<MapGenerator>().activeCharacter.transform.position+standardPos;
      }


  }


  public void RotateLeft()
  {
      transform.Rotate(Vector3.up, 90, Space.World);
  }
  public void RotateRight()
  {
      transform.Rotate(Vector3.up, -90,Space.World);
  }



}
