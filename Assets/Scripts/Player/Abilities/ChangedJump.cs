using UnityEngine;

[CreateAssetMenu(fileName = "New Changed Jump", menuName = "Abilities/Changed Jump", order = 3)]
public class ChangedJump : ScriptableAbility
{
    protected override void OnActivation(PollingStation station)
    {
        //if (player.jumpOverride.Count < 3)//dont exceed 3 override jumps
        //    player.jumpOverride.Push(DoChangedJump);
    }

    public void OnEndEffect(MovementController player)//TODO: find a better solution to use the offset vel and stuff
    {
        //player.offsetVel = Vector2.zero;
    }

    protected override bool OnFixedUpdate(PollingStation station)
    {
        //player.offsetVel = addVel;
        //addVel *= Mathf.Pow(pushForceDamp, Time.deltaTime);
        return true;
    }

    protected override void OnDeactivation(PollingStation station)
    {
        
    }
}



