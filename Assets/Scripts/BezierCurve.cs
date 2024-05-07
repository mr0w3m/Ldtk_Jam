using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve
{
    public static void GetBezierCurve(Vector3 A, Vector3 B, Vector3 C, Vector3 D, List<Vector3> positions, float resolution)
    {
        if (resolution < 0.01f)
        {
            resolution = 100;
        }

        //The resolution of the line
        //Make sure the resolution is adding up to 1, so 0.3 will give a gap at the end, but 0.2 will work
        positions.Clear();

        float t = 0;

        while (t <= 1f)
        {
            Vector3 newPos = DeCasteljausAlgorithm(A, B, C, D, t);
            positions.Add(newPos);
            t += resolution;
        }

        positions.Add(D);
    }

    public static void GetBezierCurve(Vector3 A, Vector3 B, Vector3 C, Vector3 D, Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            float lerpValue = ((float)i) / ((float)positions.Length - 1f);
            positions[i] = DeCasteljausAlgorithm(A, B, C, D, lerpValue);
        }
    }

    //The De Casteljau's Algorithm
    static Vector3 DeCasteljausAlgorithm(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        float oneMinusT = 1f - t;

        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
}