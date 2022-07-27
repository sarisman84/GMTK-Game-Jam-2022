using UnityEngine;
public static class ParticleIntegration
{
    public static void Jump(PollingStation station)
    {
        var particleManager = station.particleManager;
        var player = station.movementController.gameObject;


        particleManager.Spawn("Jump", player.transform.position - new Vector3(0, player.GetComponent<BoxCollider2D>().bounds.size.y / 2f, 0));
    }
}

