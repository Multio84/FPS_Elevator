using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform playerStartpoint;
    public Transform elevatorStartpoint;
    [HideInInspector] public int number;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] FloorCallButton floorCallButton;


    private void Awake()
    {
        numberText.text = "Level " + number.ToString();
        //if (number != 0) 
    }


}
