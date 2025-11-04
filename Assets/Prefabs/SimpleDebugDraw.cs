
using UnityEngine;

public static class SimpleDebugDraw
{
    public static void Line(Vector3 start, Vector3 end, Color color, float duration = 0f)
    {
        Debug.DrawLine(start, end, color, duration);
    }

    public static void Arrow(Vector3 start, Vector3 direction, Color color, float duration = 0f)
    {
        Debug.DrawLine(start, direction + start, color, duration);

        // Drawing the arrow head

        float headLength = 0.2f;
        float headAngle = 20f;

        // Get the rotation of the angle I want to the left and right
        Quaternion leftRot = Quaternion.AngleAxis(180f - headAngle, Vector3.forward);
        Quaternion rightRot = Quaternion.AngleAxis(180f + headAngle, Vector3.forward);

        Vector3 leftVector = leftRot * direction;
        Vector3 rightVector = rightRot * direction;

        Vector3 leftPoint = direction + leftVector * headLength;
        Vector3 rightPoint = direction + rightVector * headLength;

        Debug.DrawLine(direction + start, leftPoint + start, color, duration);
        Debug.DrawLine(direction + start, rightPoint + start, color, duration);
    }
}
