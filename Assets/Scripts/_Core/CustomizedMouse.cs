using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizedMouse : MonoBehaviour
{
    public GameObject mouseObj;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        mouseObj.transform.position = new Vector2(mousePos.x, mousePos.y);
    }


}
