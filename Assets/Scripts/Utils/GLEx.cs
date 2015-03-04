using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct LineData
{
    public Vector3 startPoint;
    public Vector3 endPont;
    public Color color;       
}

public static class GLEx
{
    static Material lineMaterial; 
    static string shader = "Shader \"Lines/Colored Blended\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha BindChannels { Bind \"Color\",color } ZWrite On Cull Front Fog { Mode Off } } } }";

    static GLEx()
    {
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    public static void DrawMesh( Mesh _mesh, Transform _transform, Color _color )
    {
        if (_mesh == null)
        {
            return;
        }
        Vector3[] vertices = _mesh.vertices;
        int[] triangles = _mesh.triangles;
        List<Vector3> linesArray = new List<Vector3>();
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            linesArray.Add(vertices[triangles[i * 3]]);
            linesArray.Add(vertices[triangles[i * 3 + 1]]);
            linesArray.Add(vertices[triangles[i * 3 + 2]]);
        }
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(_transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        GL.Color(_color);
        for (int i = 0; i < linesArray.Count / 3; i++)
        {
            GL.Vertex(linesArray[i * 3]);
            GL.Vertex(linesArray[i * 3 + 1]);

            GL.Vertex(linesArray[i * 3 + 1]);
            GL.Vertex(linesArray[i * 3 + 2]);

            GL.Vertex(linesArray[i * 3 + 2]);
            GL.Vertex(linesArray[i * 3]);
        }
        GL.End();
        GL.PopMatrix();
    }

    public static void DrawLine( Vector3 startPos, Vector3 endPos, Color _color)
    {
        GL.PushMatrix();
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        Line(startPos, endPos, _color);
        GL.End();
        GL.PopMatrix();
    }

    public static void Line(Vector3 startPos, Vector3 endPos, Color _color)
    {
        GL.Color(_color);
        GL.Vertex(startPos);
        GL.Vertex(endPos);
    }

    public static void DrawLines(List<LineData> _lineData)
    {
        GL.PushMatrix();
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        for (int i = 0; i < _lineData.Count;i++ )
        {
            Line(_lineData[i].startPoint, _lineData[i].endPont, _lineData[i].color);
        }
        GL.End();
        GL.PopMatrix();
    }

    public static void DrawCircle(Quaternion _rot, Vector3 _center, float _radius, Color _color)
    {
        //
        float two_pi = 2.0f * Mathf.PI;
        float segments = 32.0f;
        float step = two_pi / segments;
        float theta = 0.0f;

        //
        Vector3 last = _center + _rot * (_radius * new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta)));
        theta += step;

        //
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_color);
        for (int i = 1; i <= segments; ++i)
        {
            Vector3 cur = _center + _rot * (_radius * new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta)));
            GL.Vertex(last);
            GL.Vertex(cur);
            last = cur;
            theta += step;
        }
        GL.End();
        GL.PopMatrix();
    }

    public static void DrawCircle( Vector3 _centerd, float _radius, Color _color )
    {
        DrawCircle(Quaternion.identity, _centerd, _radius, _color);
    }

    public static void ConeArc(Quaternion _rot, Vector3 _center, Vector3 _forward, float _radius, float _degree)
    {
        _degree = Mathf.Clamp(_degree, 0, 360);
        int segments = 32;
        float step = _degree * Mathf.Deg2Rad / segments;
        float deta = 0f;
        float theta = 0f;
        Vector3 up = Vector3.Cross(Vector3.right, _forward);
        if (up.y < 0)
        {
            theta = Mathf.Acos(Vector3.Dot(Vector3.right, _forward));
        }
        else
        {
            theta = -1 * Mathf.Acos(Vector3.Dot(Vector3.right, _forward));
        }

        Vector3 last1 = _center + _rot * (_radius * new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta)));
        Vector3 last2 = last1;
        deta += step;

        for (int i = 1; i <= segments / 2; ++i)
        {
            Vector3 cur = _center + _rot * (_radius * new Vector3(Mathf.Cos(theta + deta), 0.0f, Mathf.Sin(theta + deta)));
            GL.Vertex(last1);
            GL.Vertex(cur);
            last1 = cur;

            cur = _center + _rot * (_radius * new Vector3(Mathf.Cos(theta - deta), 0.0f, Mathf.Sin(theta - deta)));
            GL.Vertex(last2);
            GL.Vertex(cur);
            last2 = cur;

            deta += step;
        }
    }

    public static void DrawConeArc(Vector3 _center, Vector3 _forward, float _radius, float _degree, Color _color)
    {
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_color);
        ConeArc(Quaternion.identity, _center, _forward, _radius, _degree);
        GL.End();
        GL.PopMatrix();
    }

    public static void DrawRectangle(Vector3 _center, Vector3 _forward, float _areaX, float _areaY, Color _color )
    {
        Vector3 _left = _center + Vector3.Cross(Vector3.up, _forward) * _areaX;
        Vector3 _right = _center + Vector3.Cross(_forward, Vector3.up) * _areaX;
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_color);
        GL.Vertex(_center);
        GL.Vertex(_center + _forward * _areaY);
        GL.Vertex(_left);
        GL.Vertex(_left + _forward * _areaY);
        GL.Vertex(_right);
        GL.Vertex(_right + _forward * _areaY);
        GL.Vertex(_left);
        GL.Vertex(_right);
        GL.Vertex(_left + _forward * _areaY);
        GL.Vertex(_right + _forward * _areaY);
        GL.End();
        GL.PopMatrix();
    }


    public static void DrawRectangleEx(Vector3 _center, Vector3 _forward, float _areaX, float _areaY, Color _color)
    {
        Vector3 _left = _center + Vector3.Cross(Vector3.up, _forward) * _areaX;
        Vector3 _right = _center + Vector3.Cross(_forward, Vector3.up) * _areaX;
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_color);
        GL.Vertex(_left + _forward * _areaY);
        GL.Vertex(_right + _forward * _areaY);

        GL.Vertex(_left  -_forward * _areaY);
        GL.Vertex(_right -_forward * _areaY);

        GL.Vertex(_left + _forward * _areaY);
        GL.Vertex(_left - _forward * _areaY);

        GL.Vertex(_right + _forward * _areaY);
        GL.Vertex(_right - _forward * _areaY);
        GL.End();
        GL.PopMatrix();
    }

    public static void DrawCone(Vector3 _center, Vector3 _forward, float _radius,float _angle,Color _color)
    {
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_color);
        GL.Vertex(_center);
        GL.Vertex(_center + _forward * _radius);

        Quaternion q1 = Quaternion.Euler(0, _angle / 2, 0);
        Vector3 _left = (q1 * _forward).normalized;
        GL.Vertex(_center);
        GL.Vertex(_center + _left * _radius);

        Quaternion q2 = Quaternion.Euler(0, -_angle / 2, 0);
        Vector3 _right = (q2 * _forward).normalized;
        GL.Vertex(_center);
        GL.Vertex(_center + _right * _radius);

        ConeArc(Quaternion.identity, _center, _forward, _radius, _angle);
        GL.End();
        GL.PopMatrix();
    }
}

