using UnityEngine;

public static class PathLine
{
    public static void PathLineDesign(LineRenderer line, Color startColor = new Color(), Color endColor = new Color(), Material lineMat = null, float startWidth = 1, float endWidth = 1)
    {
        if (lineMat != null)
        {
            line.material = lineMat;
        }
        if (startColor != null)
        {
            line.startColor = startColor;
        }
        if (endColor != null)
        {
            line.endColor = endColor;
        }
        line.startWidth = startWidth;
        line.endWidth = endWidth;
    }

    public static void SetPathLine(LineRenderer line, Vector3[] linePos)
    {
        line.positionCount = linePos.Length;
        line.SetPositions(linePos);
        _line = line;
    }
    public static LineRenderer _line;
}