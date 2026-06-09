using OpenTK.Mathematics;
using System.Reflection.Metadata;

namespace rt004;


public abstract class Light
{
    public Vector3d Intensity { get; }

    protected Light(Vector3d intensity)
    {
        Intensity = intensity;
    }

    public abstract Vector3d Reflectance(Vector3d normal, Vector3d point, Vector3d cameraPosition, Shading shader);

}

public class AmbientLight : Light
{
    public AmbientLight(Vector3d intensity) : base(intensity) { }

    public override Vector3d Reflectance(Vector3d normal, Vector3d point, Vector3d cameraPosition, Shading shader)
    {
        return Intensity * shader.kA;
    }
}

public class PointLight : Light
{
    public Vector3d Position { get; }

    public PointLight(Vector3d intensity, Vector3d position) : base(intensity)
    {
        Position = position;
    }

    public override Vector3d Reflectance(Vector3d normal, Vector3d point, Vector3d cameraPosition, Shading shader)
    {
        Vector3d kDiffuse, kSpecular;

        Vector3d lightDirection = Position - point;
        Vector3d viewDirection = cameraPosition - point;
        double kDpoint = Vector3d.Dot(normal, lightDirection.Normalized());

        Vector3d reflectionDirection = 2 * normal * kDpoint - lightDirection;
        double kSpoint = Vector3d.Dot(reflectionDirection, viewDirection.Normalized());

        //Diffuze
        if (kDpoint > 0) kDiffuse = Intensity * shader.kD * kDpoint;
        else
        {
            kDiffuse = Vector3d.Zero;
        }
        //Specular
        if (kSpoint > 0) kSpecular = Intensity * shader.kS * Math.Pow(kSpoint, shader.highlight);
        else
        {
            kSpecular = Vector3d.Zero;
        }
        return kDiffuse + kSpecular;
    }
}

public class DirectionalLight : Light
{
    public Vector3d Direction { get; }

    public DirectionalLight(Vector3d intensity, Vector3d direction) : base(intensity)
    {
        Direction = direction.Normalized();
    }

    public override Vector3d Reflectance(Vector3d normal, Vector3d point, Vector3d cameraPosition, Shading shader)
    {

        Vector3d kDiffuse, kSpecular;

        Vector3d lightDirection = -Direction;
        Vector3d viewDirection = cameraPosition - point;

        // Calculate the diffuse component
        double kDpoint = Vector3d.Dot(normal, lightDirection);
        if (kDpoint > 0) kDiffuse = Intensity * shader.kD * kDpoint;
        else
        {
            kDiffuse = Vector3d.Zero;
        }

        // Calculate the reflection direction for specular component
        Vector3d reflectionDirection = 2 * normal * kDpoint - lightDirection;

        // Calculate the specular component
        double kSpoint = Vector3d.Dot(reflectionDirection, viewDirection.Normalized());
        if (kSpoint > 0) kSpecular = Intensity * shader.kS * Math.Pow(kSpoint, shader.highlight);
        else
        {
            kSpecular = Vector3d.Zero;
        }

        // Return the sum of diffuse and specular components
        return kDiffuse + kSpecular;
    }
}


//Need to test
public class Spotlight : PointLight
{
    public Vector3d Direction { get; set; }
    public double CutoffAngle { get; set; } // in radians

    public Spotlight(Vector3d intensity, Vector3d position, Vector3d direction, double cutoffAngle)
        : base(intensity, position)
    {
        Direction = direction.Normalized();
        CutoffAngle = cutoffAngle;
    }

    public override Vector3d Reflectance(Vector3d normal, Vector3d point, Vector3d cameraPosition, Shading shader)
    {
        Vector3d lightDirection = (Position - point).Normalized();
        double angle = Math.Acos(Vector3d.Dot(lightDirection, Direction));

        if (angle > CutoffAngle)
        {
            return Vector3d.Zero; // Light only inside the cone
        }

        // Rest of the PointLight logic
        return base.Reflectance(normal, point, cameraPosition, shader);
    }
}