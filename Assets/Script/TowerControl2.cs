using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl2 : MonoBehaviour
{
    public bool turn = false;
   
    void OnMouseDown()
    {

        turn = true;
        Debug.Log("Data Masuk");
    }
    void OnMouseUp()
    {
        turn = false;
    }
}
