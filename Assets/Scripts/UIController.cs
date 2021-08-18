using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //��������Ϸ�ڵ�ui������������У���˵���ǲ��ù�
    [Header("CHBar")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text MagicText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider magicSlider;
    [Header("BottonPanel")]
    [SerializeField] private Button btn_L;
    [SerializeField] private Button btn_K;
    [SerializeField] private Button btn_J;


    // Update is called once per frame
    void Update()
    {
        int health = 5;
        healthText.text = "HEALTH : " + health;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health--;
        }
    }
}
