using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour, IPlayerGround
{
    public Vector3[] path;

    public float currentT;
    public float speed = 5;

    [HideInInspector] public bool looping = false;

    private float[] tValues;
    private float distSum;

    private Rigidbody2D rig;

    [HideInInspector] public Vector3 positionOnStart;

    private void Start() {
        positionOnStart = transform.position;
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

    private Vector3 GetPathPos(int i) {
        return path[i] + positionOnStart;
    }
    private void MoveToPos(Vector2 pos, float time) {
        //rig.velocity = (pos - rig.position) / time;//v = s / t
        rig.position = pos;
    }

    private Vector2 GetPathPos(float t) {//TODO: FIX THE ERROR HERE -> ITS NOT LINEAR
        float sum = 0;
        for(int i = 0; i < tValues.Length; i++) {
            if (t < sum + tValues[i])//t value is between these points
                return Vector2.Lerp(GetPathPos(i), GetPathPos(i+1), (t-sum)/tValues[i]);//calculate scaled t Value
            sum += tValues[i];
        }
        Debug.LogError("Path Position Calculation failed");
        return Vector2.zero;//return something -> calculation failed
    }

    private float DistToPercent(float dist) { return dist / distSum; }
    private void MoveAlongPath(float speed, float deltatime)
    {   
        float dt = DistToPercent(speed * deltatime);//get the change in t
        currentT += dt;

        const float maxT = 0.99f;
        float loopT = maxT - Mathf.Abs((currentT % 2)*maxT - maxT);

        nextPos = GetPathPos(loopT);
    }

    Vector2 nextPos;
    private void FixedUpdate() {
        MoveToPos(nextPos, Time.fixedDeltaTime);
        MoveAlongPath(speed, Time.fixedDeltaTime);
    }


    void IPlayerGround.OnPlayerStand(MovementController player) {
        Vector2 move = nextPos - rig.position;
        player.rig.position += move;
    }
}
