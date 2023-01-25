using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePhone : MonoBehaviour
{

    // Update is called once per frame

    public float rotateSpeed = 90.0f;
    public GameObject unityRotate;
    public Animator loliUnityChanAnim;

    void Update()
    {

        if (Input.acceleration != Vector3.zero)
        {
            //Part of the animation
            //Example with cube
            Vector3 dir = Vector3.zero;
            dir.x = -Input.acceleration.y;
            dir.z = Input.acceleration.x;

            // clamp acceleration vector to unit sphere
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;

            loliUnityChanAnim.SetBool("IsRun", true);
            unityRotate.transform.rotation = Quaternion.Euler(0, 0, -dir.x * rotateSpeed);
            UIManager.instance.loadChargeSlide.value += 0.1f * Time.deltaTime;
        }
        else
        {
            loliUnityChanAnim.SetBool("IsRun", false);

        }

        if (UIManager.instance.loadChargeSlide.value == 1)
        {

            UIManager.instance.LoadNextResult();
            UIManager.instance.loadChargeSlide.value = 0;
            loliUnityChanAnim.SetBool("IsRun", false);


        }
    }




}





