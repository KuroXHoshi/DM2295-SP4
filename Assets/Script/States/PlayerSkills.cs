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
        player.PlayerAudio.MusicSource.clip = player.PlayerAudio.dash;
        player.PlayerAudio.playerdash();
    }

    public override void Update()
    {
        player.transform.position += player.transform.forward * player.GetpStats().dashSpd * player.GetpStats().moveSpd * Time.deltaTime;

        if (Vector3.Distance(prevPos, player.transform.position) > player.GetpStats().dashDist)
        {
            player.sm.SetNextState((anim.GetBool("attacking")) ? "Attack" : "Idle");
        }
    }

    public override void Exit()
    {
        anim.SetBool("dash", false);
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