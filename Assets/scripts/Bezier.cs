using System;
using UnityEngine;

[Serializable]
public class Bezier
{
    private float Ax;
    private float Ay;
    private float Az;
    private Vector3 b0 = Vector3.zero;
    private Vector3 b1 = Vector3.zero;
    private Vector3 b2 = Vector3.zero;
    private Vector3 b3 = Vector3.zero;
    private float Bx;
    private float By;
    private float Bz;
    private float Cx;
    private float Cy;
    private float Cz;
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Transform p3;
    public float ti;

    public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Transform v3)
    {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
        this.p3 = v3;
    }

    private void CheckConstant()
    {
        if (((this.p0 != this.b0) || (this.p1 != this.b1)) || ((this.p2 != this.b2) || (this.p3.position != this.b3)))
        {
            this.SetConstant();
            this.b0 = this.p0;
            this.b1 = this.p1;
            this.b2 = this.p2;
            this.b3 = this.p3.position;
        }
    }

    public Vector3 GetPointAtTime(float t)
    {
        this.CheckConstant();
        float num = t * t;
        float num2 = (t * t) * t;
        float x = (((this.Ax * num2) + (this.Bx * num)) + (this.Cx * t)) + this.p0.x;
        float y = (((this.Ay * num2) + (this.By * num)) + (this.Cy * t)) + this.p0.y;
        return new Vector3(x, y, (((this.Az * num2) + (this.Bz * num)) + (this.Cz * t)) + this.p0.z);
    }

    private void SetConstant()
    {
        this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);
        this.Bx = (3f * ((this.p3.position.x + this.p2.x) - (this.p0.x + this.p1.x))) - this.Cx;
        this.Ax = ((this.p3.position.x - this.p0.x) - this.Cx) - this.Bx;
        this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);
        this.By = (3f * ((this.p3.position.y + this.p2.y) - (this.p0.y + this.p1.y))) - this.Cy;
        this.Ay = ((this.p3.position.y - this.p0.y) - this.Cy) - this.By;
        this.Cz = 3f * ((this.p0.z + this.p1.z) - this.p0.z);
        this.Bz = (3f * ((this.p3.position.z + this.p2.z) - (this.p0.z + this.p1.z))) - this.Cz;
        this.Az = ((this.p3.position.z - this.p0.z) - this.Cz) - this.Bz;
    }
}

