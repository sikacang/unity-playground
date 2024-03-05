using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class LinePath : PathSceneTool
{
    [SerializeField]
    private LineRenderer line;

    protected override void PathUpdated()
    {
        if (line != null)
        {
            Vector3[] points = new Vector3[path.NumPoints];
            for (int i = 0; i < path.NumPoints; i++)
            {
                points[i] = path.GetPoint(i);
            }

            line.positionCount = points.Length;
            line.SetPositions(points);
        }
    }
}
