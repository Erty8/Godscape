using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexObject 
{
    public readonly int Q;
    public readonly int R;
    public readonly int S;
    readonly float width_multiplier = Mathf.Sqrt(3)/2;


    public HexObject(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q+r);
    }

    public Vector3 Position()
    {
        float radius = 1f;
        float height = radius * 2;
        float width = height * width_multiplier;
        float vert = height * 0.75f;
        float horiz = width;
        return new Vector3(horiz * this.R,           
            0,
            vert * (this.Q + this.R / 2)
            );
    }
}
