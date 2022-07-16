using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Vector3[] path;

    [Range(0, 1)] public float currentT;
    public float speed = 1;

    [HideInInspector] public bool looping = false;


    private float[] tValues;
    private float distSum;

    private Rigidbody2D rig;

    private int pathI = 0;
    private float pathSum = 0;

    [HideInInspector] public Vector3 positionOnStart;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        tValues = new float[path.Length - 1];
        distSum = 0;
        for (int p = 0; p < path.Length - 1; p++)
        {
            tValues[p] = Vector3.Distance(path[p], path[p + 1]);//save distances in the tValues
            distSum += tValues[p];
        }

        for (int t = 0; t < tValues.Length; t++)
        {
            tValues[t] /= distSum;//divide every distance by the sum of distances to normalize them
        }
        positionOnStart = transform.position;
    }

    private void MoveToPos(Vector2 pos, float time)
    {
        rig.velocity = (pos - rig.position) / time;//v = s / t
    }

    private Vector2 GetPathPos(float t)
    {
        for (; pathI < tValues.Length; pathI++)
        {
            if (t < pathSum + tValues[pathI])//t value is between these points
                return Vector2.Lerp(path[pathI], path[pathI + 1], (t - pathSum) / tValues[pathI]);//calculate scaled t Value
            pathSum += tValues[pathI];
        }
        Debug.LogError("Path Position Calculation failed");
        return path[pathI];//return something -> calculation failed
    }

    private float DistToPercent(float dist) { return dist / distSum; }
    private void MoveAlongPath(float speed, float deltatime)
    {
        float dt = DistToPercent(speed * deltatime);//get the change in t
        currentT += dt;
        Vector2 pos = GetPathPos(currentT);
        MoveToPos(pos, deltatime);
    }
}
