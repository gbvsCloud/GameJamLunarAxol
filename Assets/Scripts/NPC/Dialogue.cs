using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameComponent;
    public Image backgroundComponent;
    public Image imageComponent;
    public Sprite speakerImage;
    public float textSpeed = 0.1f;
    public float fasterTextSpeed = 0.01f;
    public Actor[] actors;
    public Message[] messages;

    public UnityEvent onDialogueEnd;

    int currentMessage = -1;
    public bool writing = false;
    public bool accelerateWrite = false;

 
    private void OnEnable() {
        currentMessage = -1;
    }
    public void DisplayMessage(){
        
        
        if(writing){
            accelerateWrite = true;
            return;
        }

        currentMessage++;

        if(currentMessage >= messages.Length){
            currentMessage = 0;
            
            onDialogueEnd.Invoke();
            gameObject.SetActive(false);  
            return;     
        }
        
        Message messageToDisplay = messages[currentMessage];
        StopAllCoroutines();
        StartCoroutine(TypeLine(messageToDisplay.message));
        Actor actorToDisplay = actors[messageToDisplay.actorID];
        imageComponent.sprite = actorToDisplay.sprite;
        nameComponent.text = actorToDisplay.name;
        backgroundComponent.sprite = actorToDisplay.dialogueBoxSprite;

    }

    public void EndMessage(){
        currentMessage = -1;
        StopAllCoroutines();
        writing = false;
        gameObject.SetActive(false);
    }

    IEnumerator TypeLine(string message){    

        textComponent.text = string.Empty;
        writing = true;
        accelerateWrite = false;
        foreach(char c in message){
            textComponent.text += c;
            if(c != ' '){
                if(!accelerateWrite)
                    yield return new WaitForSeconds(textSpeed);
                else
                    yield return new WaitForSeconds(fasterTextSpeed);
            }
            else
                yield return new WaitForSeconds(0);
        }
        writing = false;
    }

    /*void NextLine(){
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }else{
            textComponent.text = string.Empty;
            index = 0;
            gameObject.SetActive(false);
        }
    }*/
    [System.Serializable]
    public class Message{
        public int actorID;
        public string message;
    }
    [System.Serializable]
    public class Actor{
        public string name;
        public Sprite sprite;
        public Sprite dialogueBoxSprite;
        public float fontSize;
    }

}


