using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    private LineRenderer line;
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Draw(List<Vector2> points)
    {
        line.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    public void Clear()
    {
        line.positionCount = 0;
    }
}
