using UnityEngine;

public class RapierAttack : MonoBehaviour
{
    public Player player;
    private bool hitEnemy = false;
    private void Start(){
        Destroy(gameObject, 0.1f);
    }

   void OnTriggerEnter2D(Collider2D other)
   {
       if(hitEnemy) return; 

       if(other.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.Knockback(player.transform, 2);
            enemy.TakeDamage();
            hitEnemy = true;
       }
   }

}
