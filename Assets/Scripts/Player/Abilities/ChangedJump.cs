//using UnityEngine;

//[CreateAssetMenu(fileName = "New Changed Jump", menuName = "Player Abilities/Changed Jump", order = 3)]
//public class ChangedJump : ScriptableAbility
//{
//    public Vector2 jumpForce = new Vector2(0, 20);
//    public float pushForceDamp = 0.1f;
//    private Vector2 addVel = Vector2.zero;

//    private void DoChangedJump(MovementController player)
//    {
//        Debug.Log("Changed Jump");
//        player.vel.y += jumpForce.y;
//        addVel = Vector2.right * jumpForce.x * -player.facingDir;
//    }

<<<<<<< HEAD
//    public override bool ApplyEffect(MovementController player)
//    {
//        if (player.jumpOverride.Count < 3)//dont exceed 3 override jumps
//            player.jumpOverride.Push(DoChangedJump);
//        return false;
//    }

//    public override void OnEndEffect(MovementController player)
//    {
//        player.offsetVel = Vector2.zero;
//    }

//    public override bool UpdateEffect(MovementController player)
//    {
//        player.offsetVel = addVel;
//        addVel *= Mathf.Pow(pushForceDamp, Time.deltaTime);
//        return true;
//    }

//    public override void OnGizmosDraw(MovementController player)
//    {
//    }
//}
=======
    protected override void OnActivation(MovementController player, AbilityController abilityController)
    {
        if (player.jumpOverride.Count < 3)//dont exceed 3 override jumps
            player.jumpOverride.Push(DoChangedJump);
    }

    public void OnEndEffect(MovementController player)//TODO: find a better solution to use the offset vel and stuff
    {
        player.offsetVel = Vector2.zero;
    }

    protected override bool OnFixedUpdate(MovementController player, AbilityController abilityController)
    {
        player.offsetVel = addVel;
        addVel *= Mathf.Pow(pushForceDamp, Time.deltaTime);
        return true;
    }
}
>>>>>>> NewMain
