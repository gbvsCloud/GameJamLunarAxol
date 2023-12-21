
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StateAttack : StateBase
{
    private Player player;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public float attackAnimationDuration;
    public string currentTrigger;
    public override void OnStateEnter(params object[] objs)
    {
        player = objs[0] as Player;
        player.attacking = true;
        
        rigidBody = player.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();
     
        
        switch(player.currentAttack){
            case 0:
                currentTrigger = "Attack";
                GetAttackDuration("Player_Attacking");
                player.attackOvertime = 0.25f + attackAnimationDuration;
                break;
            case 1:
                currentTrigger = "SecondAttack";
                GetAttackDuration("Player_Second_Attack");
                break;
        }
        animator.SetTrigger(currentTrigger);

        player.Attack();
         if(player.currentAttack == player.attacks.Length - 1){
            player.currentAttack = 0;
            player.attackOvertime = 0;
        }
        if(player.currentAttack < player.attacks.Length - 1)
            player.currentAttack++;
        
       
        
    }  
    public override void OnStateStay()
    {
        //CheckStateSwitch();
        attackAnimationDuration -= Time.deltaTime;
        if(attackAnimationDuration <= 0){
            animator.ResetTrigger(currentTrigger);
            
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public void GetAttackDuration(string animationName){
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if(clip.name == animationName){
                attackAnimationDuration = clip.length;
            }
        }
    }

    public override void OnStateExit()
    {
        player.attacking = false;
        animator.ResetTrigger(currentTrigger);
            
    }
    public override void CheckStateSwitch()
    {   
        if(attackAnimationDuration > 0) return;


        if(player.xInput == 0 && player.isGrounded){
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }
        else if(rigidBody.velocity.y < 0 && !player.isGrounded){
            player.stateMachine.SwitchState(Player.States.FALLING, player);
        }else if(player.xInput != 0 && player.isGrounded){
            player.stateMachine.SwitchState(Player.States.RUNNING, player);
        }
    }




    

}