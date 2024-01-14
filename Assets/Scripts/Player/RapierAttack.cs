using UnityEngine;

public class RapierAttack : MonoBehaviour
{
    public Player player;
    public RandomSound sound;
    private void Start(){
        Destroy(gameObject, 0.1f);
    }

   void OnTriggerEnter2D(Collider2D other)
   {
       if(other.tag == "Enemy" || other.tag == "Boss"){
            EntityBase enemy = other.GetComponent<EntityBase>();
            enemy?.Knockback(player.transform, 2);
            enemy?.TakeDamage();
            sound?.PlayRandomSound();
       }
   }

}
