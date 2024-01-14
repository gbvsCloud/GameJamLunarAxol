
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{   
    public bool turnBlack = false;
    Image image;
    public float alpha = 0;
    private void Start() {
        image = GetComponent<Image>();
        
    }
    private void Update() {
        if(turnBlack){
            if(alpha < 255){
                alpha += Time.deltaTime * 255 / 2;
            }
            image.color = new Color32(0, 0, 0, (byte)alpha);
        }
    }
    public IEnumerator LoadFinalScreen(){
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("TelaVitoria");
    } 
    public void TurnBlack(){
        turnBlack = true;
        StartCoroutine(LoadFinalScreen());
    }
}
