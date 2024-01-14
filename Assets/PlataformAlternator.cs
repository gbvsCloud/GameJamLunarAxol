using UnityEngine;
using UnityEngine.Tilemaps;

public class PlataformAlternator : MonoBehaviour
{
    public Tilemap tilemap;
    public TilemapCollider2D tilemapCollider;
    public float collisionTimer = 0;
    public bool plataformDisable = false;

    private void Start() {
        
    }
    public void Update()
    {
        if(collisionTimer >= 1){        
            plataformDisable = true;   
        }
        if(collisionTimer <= 0){
            plataformDisable = false;
        }     
        
        if(plataformDisable){
            collisionTimer -= Time.deltaTime * 0.5f;
            if(tilemap.color.a != 50){
                tilemap.color = new Color32(255, 255, 255, 50);
                tilemapCollider.enabled = false;  
                       
            }   
        }else{ 
            if(tilemap.color.a != 255){
                tilemap.color = new Color32(255, 255, 255, 255);
                tilemapCollider.enabled = true;   
            }      
        }
        
        tilemap.RefreshAllTiles();
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if(other.transform.CompareTag("Player") && !plataformDisable){
            if(collisionTimer < 1){
                collisionTimer += Time.deltaTime;
            }
        }
        
    }


    
}
