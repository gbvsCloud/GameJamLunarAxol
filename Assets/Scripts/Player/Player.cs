using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField]
    private SpriteRenderer sRenderer;

    private float currentGravity;
   
    void Start()
    {
        currentGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float GetGravity()
    {
        return currentGravity;
    }
    
}
