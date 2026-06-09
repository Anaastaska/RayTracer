
using Util;
using System;
using System.Xml.Linq;
using rt004;
using System.Drawing;
using OpenTK.Mathematics;
using System.Security.Cryptography;
using System.Globalization;
using CommandLine;
using rt004.shared;

namespace rt004;


public class Options
{
    [Option('c', "config", Required = true, HelpText = "Path to the configuration XML file.")]
    public string configFilePath { get; set; }
}

internal class Program
{
    static Camera sceneCamera;
    static Sphere sphereXML;
    static Vector3d background;
    static List<Light> lights;
    static List<Solids> solids;
    static Dictionary<string, Shading> materialsDict;
    static String outputFilename;


    public static Vector3d getRayColor(Ray ray, int depth, int ignoreSolidId)
    {
        if (depth <= 0)
            return Vector3d.Zero; // Terminate recursion

        string MaterialName = materialsDict.Keys.First();
        Intersection firstIntersection = Intersection.False;

        foreach (Solids solid in solids)
        {
            Intersection inter = solid.Intersect(ray);
            if (inter.Intersected && inter.SolidIdx != ignoreSolidId)
            {
                if (firstIntersection == Intersection.False)
                {
                    MaterialName = solid.Shading;
                    firstIntersection = inter;
                }
                else if (inter.Distance < firstIntersection.Distance)
                {
                    MaterialName = solid.Shading;
                    firstIntersection = inter;
                }
            }
        }

        //put background color
        if (!firstIntersection.Intersected)
        {
            return background;
        }

        Shading shader1 = materialsDict[MaterialName];

        Vector3d point = ray.LookAt(firstIntersection.Distance);
        Vector3d intensity = Vector3d.Zero;

        foreach (Light lightSource in lights)
        {
            if (lightSource is AmbientLight ambientLight)
            {
                intensity += ambientLight.Reflectance(firstIntersection.Normal, point, sceneCamera.Position, shader1);
                continue; // Skip shadow check for ambient light
            }

            bool isShadow = false;
            if (!(lightSource is PointLight))
            {
                continue;
            }

            PointLight pointLight = (PointLight)lightSource;

            Vector3d Direction = pointLight.Position - firstIntersection.Position;
            Direction.Normalize();
            Ray rayToLight = new Ray() { Position = point + firstIntersection.Normal * 1e-5, Direction = Direction };

            foreach (Solids solid in solids)
            {
                Intersection inter = solid.Intersect(rayToLight);

                if (inter.Intersected && inter.SolidIdx != firstIntersection.SolidIdx)
                {
                    isShadow = true;
                    break;
                }
            }

            if (!isShadow)
            {
                intensity += lightSource.Reflectance(firstIntersection.Normal, point, sceneCamera.Position, shader1);
            }

        }
        intensity = Vector3d.Clamp(intensity, Vector3d.Zero, Vector3d.One);
        //put final pixel color
        Vector3 intensityVector = (Vector3)intensity;
        Vector3d resultColor = intensityVector * shader1.Color;


        if (depth > 0 && shader1.kS > 0) // Ensure the reflection is only computed when needed
        {
            Vector3d reflectionDirection = ray.Direction - 2 * Vector3d.Dot(ray.Direction, firstIntersection.Normal) * firstIntersection.Normal;
            reflectionDirection.Normalize();

            Ray reflectionRay = new Ray()
            {
                Position = point + firstIntersection.Normal * 1e-5, // Offset to avoid self-intersection
                Direction = reflectionDirection
            };

            Vector3d reflectedColor = getRayColor(reflectionRay, depth - 1, firstIntersection.SolidIdx);

            resultColor += shader1.kS * reflectedColor; // Multiply by specular coefficient to reduce contribution
        }

        return resultColor;


    }

    static void Main(string[] args){



        Parser.Default.ParseArguments<Options>(args)
           .WithParsed<Options>(o =>
           {
               try
               {

                   sceneCamera = null;
                   sphereXML = null;
                   background = Vector3d.Zero;
                   lights = null;
                   solids = null;
                   materialsDict = null;
                   outputFilename = null;

                   XMLParser.ParseXMLFile(ref sceneCamera, ref solids, o.configFilePath, ref background, ref lights, ref materialsDict, ref outputFilename);

                   var image = new FloatImage((int)sceneCamera.Resolution.X, (int)sceneCamera.Resolution.X, 3);
                   float[] yellowColor = { 1, 1, 0.2f };
                   float[] backgroundLst = { (float)background.X, (float)background.Y, (float)background.Z };
                   for (int y = 0; y < image.Height; y++)
                   {
                       for (int x = 0; x < image.Width; x++)
                       {
                           image.PutPixel(x, y, backgroundLst);
                       }
                   }

                   for (int x = 0; x < image.Width; x++)
                   {
                       for (int y = 0; y < image.Height; y++)
                       {
                           Ray ray = sceneCamera.CreateRay(new Vector2d(x, y));

                           Vector3d resultColor = getRayColor(ray, 8, -1);
                           float[] colorLst = { (float)resultColor.X, (float)resultColor.Y, (float)resultColor.Z };
                           image.PutPixel(x, y, colorLst);
                       }
                   }

                   image.SaveHDR(outputFilename);

               }
               catch (Exception ex)
               {
                   Console.WriteLine($"Error loading configuration: {ex.Message}");
               }
           });


    }
}
