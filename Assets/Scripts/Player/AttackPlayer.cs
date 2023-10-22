using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class AttackPlayer : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.R;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Input.GetKey(keyCode))
        {

        }
    }

}
