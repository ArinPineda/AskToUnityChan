using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;

public class TouchUnityChan : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
        Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {


                gameObject.GetComponent<Animator>().SetBool("Next", true);


            }
        }
        



    }
}
