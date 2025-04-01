using System.Threading.Tasks;
using UnityEngine;

public class Dead : IState
{
    NPC npc;
    
    public Dead(NPC npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        Debug.Log("NPC er nu i Idle-tilstand");
    }

    public void Update()
    {
        if (npc.Health <= 0)
        {
            PlayDeathAnimationAndDestroy();
        }
    }

    public void Exit()
    {
        Debug.Log("NPC forlader Idle-tilstand");
    }

    void Start()
    {
        npc.TransitionToState(new Roam(npc));
    }

    private async void PlayDeathAnimationAndDestroy()
    {
        await PlayDeathAnimation(); 
        
    }

    private async Task PlayDeathAnimation()
    {
        Debug.Log("Playing death animation...");
        await Task.Delay(2000); 
    }
}
