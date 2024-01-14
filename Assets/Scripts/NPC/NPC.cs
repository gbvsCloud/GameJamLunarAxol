using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public Transform playerTrans;
    public bool inRange = false;
    public SpriteRenderer spriteRenderer;
    public GameObject interactButton;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        
        if(inRange){
            
            if(playerTrans.position.x > transform.position.x){
                spriteRenderer.flipX = false;
            }else{
                spriteRenderer.flipX = true;
            }
        }
        if(inRange && Input.GetKeyDown(KeyCode.E)){
            if(Input.GetKeyDown(KeyCode.E)){       
                dialogue.gameObject.SetActive(true);
                dialogue.DisplayMessage();
            }
        }

        if(!inRange){
            if(dialogue.gameObject.activeSelf) dialogue.EndMessage();
            spriteRenderer.flipX = false;
        
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerTrans = other.GetComponent<Transform>();
            inRange = true;
            interactButton?.SetActive(inRange);
        } 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")) inRange = false;
        interactButton?.SetActive(inRange);
    }       
    public virtual void EndDialogue(){

    }

}
