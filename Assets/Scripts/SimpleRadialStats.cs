using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRadialStats : Graphic
{
    [SerializeField, Range(0, 20)] int[] _stats;
    [SerializeField] float _radius = 20f;
    [SerializeField] float _scale = 1f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        int count = _stats.Length;
        float angleStep = 360f / count;
        Vector2 center = rectTransform.rect.center;

        UIVertex centerVertex = UIVertex.simpleVert;
        centerVertex.color = color;
        centerVertex.position = center;
        vh.AddVert(centerVertex);

        for (int i = 0;i<count;i++)
        {
            float normalized = Mathf.Clamp01((float)_stats[i]/_radius);
            float angle = Mathf.Deg2Rad * (angleStep * i - 90);

            Vector2 pos = center + new Vector2(
                Mathf.Cos(angle),
                -Mathf.Sin(angle)) * _radius * normalized * _scale;

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = pos;
            vh.AddVert(vert);
        }

        for (int i = 1; i <= count; i++)
        {
            //If i equals count, Next equals 1 otherwise it equals i+1
            int next = (i == count) ? 1 : i + 1;
            vh.AddTriangle(0, i, next);
        }
    }

    public void SetStat(Stats stats, int value)
    {
        _stats[(int)stats] = value;
    }
}
