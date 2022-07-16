using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Vector3[] path;//TODO: program a custom editor script to move positions of path (use Handles.PositionHandle)
    private float[] tValues;

    private Rigidbody2D rig;

    private void Start() {
        rig = GetComponent<Rigidbody2D>();

        tValues = new float[path.Length - 1];
        float sum = 0;
        for (int p = 0; p < path.Length - 1; p++) {
            tValues[p] = Vector3.Distance(path[p], path[p + 1]);//save distances in the tValues
            sum += tValues[p];
        }

        for(int t = 0; t < tValues.Length; t++) {
            tValues[t] /= sum;//divide every distance by the sum of distances to normalize them
        }
    }

    private void MoveToPos(Vector2 pos, float time) {
        rig.velocity = (pos - rig.position) / time;//v = s / t
    }

    private Vector2 GetPathPos(float t) { return GetPathPos(t, 0, 0); }
    private Vector2 GetPathPos(float t, int i, float sum) {
        for(; i < tValues.Length; i++) {
            if (t < sum + tValues[i])//t value is between these points
                return Vector2.Lerp(path[i], path[i+1], (t-sum)/tValues[i]);//calculate scaled t Value
        }

        throw new System.Exception("The code should never have gotten here");
    }
}
