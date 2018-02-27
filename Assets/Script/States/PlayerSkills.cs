using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    private Player player;
    private Animator anim;
    private Vector3 prevPos;
    

    public Dash(Player _player) : base("Dash")
    {
        player = _player;
        anim = player.GetAnim();
    }

    public override void Enter()
    {
        prevPos = player.transform.position;
        anim.SetBool("dash", true);
        player.PlayerAudio.playerdash();
    }

    public override void Update()
    {

        if (Vector3.Distance(prevPos, player.transform.position) > player.GetpStats().dashDist)
        {
            player.sm.SetNextState((anim.GetBool("attacking")) ? "Attack" : "Idle");
        }

        player.transform.position += player.transform.forward * player.GetpStats().dashSpd * player.GetpStats().moveSpd * Time.deltaTime;
        anim.SetBool("dash", false);
    }

    public override void Exit()
    {
    }
}

public class Bash : State
{
    public Bash(Player _player) : base("Bash")
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}