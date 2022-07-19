//using UnityEngine;
//using System.Collections;

//[CreateAssetMenu(fileName = "New Dash", menuName = "Player Abilities/Dash", order = 2)]
//public class Dash : ScriptableAbility
//{
//    //[SerializeField] float Distance = 20f;
//    //[SerializeField] float Speed = 30;
//    //float dash = 0;
//    //int dashDir;

//    //public override bool ApplyEffect(MovementController player)
//    //{
//    //    Debug.Log("Dash");
//    //    dash = Distance / Speed;// = duration
//    //    dashDir = player.facingDir;
//    //    player.takeInput = false;
//    //    player.GravityScale = 0;


//    //    return false;
//    //}

//    //public override void OnEndEffect(MovementController player)
//    //{
//    //    dash = 0;
//    //    player.takeInput = true;
//    //    player.GravityScale = 1;
//    //}

//    //public override bool UpdateEffect(MovementController player)
//    //{
//    //    if (dash == 0) return false;//no dash effect active

//    //    if (dash > 0)
//    //    {
//    //        dash -= Time.fixedDeltaTime;
//    //        player.vel.x = dashDir * Speed;
//    //        player.vel.y = 0;
//    //    }
//    //    else
//    //    {
//    //        OnEndEffect(player);
//    //    }

//    //    return true;
//    //}


//    //public override void OnGizmosDraw(MovementController player)
//    //{
//    //}
//}