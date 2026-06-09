using System.Drawing;
using OpenTK.Mathematics;
//simple Phong Shading Model
namespace rt004;

public class Shading
{
    public string shade;
    public Vector3d Color;
    //ambient, diffuze and specular
    public double kA, kD, kS;
    public double highlight;
}