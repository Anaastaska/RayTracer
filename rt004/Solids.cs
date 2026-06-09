using Microsoft.VisualBasic;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rt004;
public abstract class Solids
{
    public string Shading;
    public Vector3d Position;

    public int Id;
    public abstract Intersection Intersect(Ray ray);
}
