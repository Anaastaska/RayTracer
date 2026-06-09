﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace rt004
{
    public class XMLParser
    {
        public static String loadOutputFilename(XElement elem)
        {
            string filename = elem.Value;

            if (!filename.EndsWith(".hdr", StringComparison.OrdinalIgnoreCase))
            {
                // Append ".hdr" if it doesn't end with ".hdr"
                filename += ".hdr";
            }
            return filename;
        }

        public static Vector2d loadVector2d(XElement element)
        {
            float xValue;
            float.TryParse(element.Element("X").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out xValue);
            float yValue;
            float.TryParse(element.Element("Y").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out yValue);
            return new Vector2d(xValue, yValue);
        }

        public static Vector3d loadVector3d(XElement element)
        {
            float xValue;
            float.TryParse(element.Element("X").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out xValue);
            float yValue;
            float.TryParse(element.Element("Y").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out yValue);
            float zValue;
            float.TryParse(element.Element("Z").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out zValue);
            return new Vector3d(xValue, yValue, zValue);
        }

        public static Vector3d loadColorVector3d(XElement element)
        {
            float rValue;
            float.TryParse(element.Element("R").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out rValue);
            float gValue;
            float.TryParse(element.Element("G").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out gValue);
            float bValue;
            float.TryParse(element.Element("B").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out bValue);
            return new Vector3d(rValue, gValue, bValue);
        }

        public static Camera LoadCamera(XElement elem)
        {
            XElement ResElem = elem.Element("Resolution");
            XElement PositionElem = elem.Element("Position");
            XElement DirectionElem = elem.Element("Direction");
            XElement FovElem = elem.Element("FOV");
            Vector2d Resolution = loadVector2d(ResElem);
            Vector3d Position = loadVector3d(PositionElem);
            Vector3d Direction = loadVector3d(DirectionElem);
            float Fov;

            if (Resolution.X * Resolution.Y > 4096 * 4096)
            {
                Console.WriteLine("Error: Resolution should be less than 4K.");
                Environment.Exit(1); // Exit the program with an error code
            }

            if (!float.TryParse(FovElem.Value, out Fov) || Fov < 40 || Fov > 120)
            {
                Console.WriteLine("Error: FOV must be between 40 and 120.");
                Environment.Exit(1); // Exit the program with an error code
            }

            Camera camera = new Camera(Resolution.X, Resolution.Y);
            camera.Position = new Vector3d(Position.X, Position.Y, Position.Z);
            camera.Direction = new Vector3d(Direction.X, Direction.Y, Direction.Z);
            camera.FOV = Fov;
            return camera;
        }

        public static Vector3d LoadBackgroundColor(XElement elem)
        {
            // Load the color vector from the XML element
            Vector3d RGBColor = loadColorVector3d(elem);

            // Check if each RGB component is within the valid range (0.0 to 1.0)
            if (RGBColor.X < 0.0 || RGBColor.X > 1.0 ||
                RGBColor.Y < 0.0 || RGBColor.Y > 1.0 ||
                RGBColor.Z < 0.0 || RGBColor.Z > 1.0)
            {
                Console.WriteLine("Error: Each RGB value must be between 0.0 and 1.0.");
                Environment.Exit(1);
            }

            return RGBColor;
        }

        public static Light LoadAmbientLight(XElement elem)
        {
            Vector3d Intensity = loadVector3d(elem.Element("Intensity"));
            Light light = new AmbientLight(Intensity);
            return light;
        }

        public static Light LoadPointLight(XElement elem)
        {
            Vector3d Intensity = loadVector3d(elem.Element("Intensity"));
            Vector3d Position = loadVector3d(elem.Element("Position"));
            Light light = new PointLight(Intensity, Position);
            return light;
        }
        public static List<Light> LoadLights(XElement elem)
        {
            List<Light> Result = new List<Light>();
            foreach (XElement lightElem in elem.Elements())
            {
                XName ElemName = lightElem.Name;
                if (ElemName == "Ambient")
                {
                    Result.Add(LoadAmbientLight(lightElem));
                }
                else if (ElemName == "Point")
                {
                    Result.Add(LoadPointLight(lightElem));
                }
            }
            return Result;
        }

        public static Solids LoadSpheres(XElement elem, int solidIdx)
        {
            string XMLshade = elem.Element("Material").Value;
            Vector3d XMLPosition = loadVector3d(elem.Element("Position"));
            float XMLradius;
            if (!float.TryParse(elem.Element("Radius").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out XMLradius) || XMLradius <= 0.0f)
            {
                Console.WriteLine("Error: Radius must be greater than 0.0.");
                Environment.Exit(1); // Exit the program with an error code
            }
            Sphere sphere = new Sphere();
            sphere.Radius = XMLradius;
            sphere.Position = XMLPosition;
            sphere.Shading = XMLshade;
            sphere.Id = solidIdx;
            return sphere;
        }

        public static Solids LoadPlane(XElement elem, int SolidIdx)
        {
            string XMLshade = elem.Element("Material").Value;
            Vector3d XMLNormal = loadVector3d(elem.Element("Normal"));
            Vector3d XMLPosition = loadVector3d(elem.Element("Position"));

            Plane plane = new Plane();
            plane.Normal = XMLNormal;
            plane.Shading = XMLshade;
            plane.Position = XMLPosition;
            plane.Id = SolidIdx;
            return plane;
        }

        public static List<Solids> LoadSolids(XElement elem)
        {
            List<Solids> Result = new List<Solids>();
            int solidIdx = 0;
            foreach (XElement solidsElem in elem.Elements())
            {
                XName ElemName = solidsElem.Name;
                if (ElemName == "Sphere")
                {
                    Result.Add(LoadSpheres(solidsElem, solidIdx));
                }
                else if (ElemName == "Plane")
                {
                    Result.Add(LoadPlane(solidsElem, solidIdx));
                }
                solidIdx++;
            }
            return Result;
        }



        public static Dictionary<string, Shading> LoadDictionary(XElement elem)
        {
            Dictionary<string, Shading> MaterialsMap = new Dictionary<string, Shading>();
            foreach (XElement materialsElem in elem.Elements())
            {
                float kA;
                // Parse and validate the ambient coefficient
                if (!float.TryParse(materialsElem.Element("Ambient").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out kA) || kA <= 0.0f)
                {
                    Console.WriteLine("Error: Ambient coefficient (kA) must be greater than 0.0.");
                    Environment.Exit(1); // Exit the program with an error code
                }
                float kD;
                // Parse and validate the diffuse coefficient
                if (!float.TryParse(materialsElem.Element("Diffuse").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out kD) || kD <= 0.0f)
                {
                    Console.WriteLine("Error: Diffuse coefficient (kD) must be greater than 0.0.");
                    Environment.Exit(1); // Exit the program with an error code
                }
                float kS;
                // Parse and validate the specular coefficient
                if (!float.TryParse(materialsElem.Element("Specular").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out kS) || kS <= 0.0f)
                {
                    Console.WriteLine("Error: Specular coefficient (kS) must be greater than 0.0.");
                    Environment.Exit(1); // Exit the program with an error code
                }
                Shading shader1 = new Shading();
                shader1.kA = kA;
                shader1.kD = kD;
                shader1.kS = kS;
                double highlight;
                if (!double.TryParse(materialsElem.Element("Highlight").Value, NumberStyles.Float, CultureInfo.InvariantCulture, out highlight) || highlight <= 0.0)
                {
                    Console.WriteLine("Error: Highlight coefficient must be greater than 0.0.");
                    Environment.Exit(1); // Exit the program with an error code
                }
                shader1.highlight = highlight;
                //RGB
                Vector3d vector3D = loadColorVector3d(materialsElem.Element("Color"));
                if (vector3D.X < 0.0 || vector3D.X > 1.0 || vector3D.Y < 0.0 || vector3D.Y > 1.0 || vector3D.Z < 0.0 || vector3D.Z > 1.0)
                {
                    Console.WriteLine("Error: RGB values must be between 0.0 and 1.0.");
                    Environment.Exit(1); // Exit the program with an error code
                }
                shader1.Color = vector3D;
                //find key
                string Name = materialsElem.Element("Name").Value;
                MaterialsMap[Name] = shader1;
            }
            return MaterialsMap;
        }

        public static void ParseXMLFile(ref Camera camera, ref List<Solids> solids, string configFile,
            ref Vector3d backgroundColor, ref List<Light> Lights, ref Dictionary<string, Shading> myDictionary, ref String outputFilename)
        {
            XDocument doc = XDocument.Load(configFile);
            IEnumerable<XElement> Elem = doc.Element("Config").Elements();

            foreach (XElement elem in Elem)
            {
                XName ElemName = elem.Name;
                if (ElemName == "Camera")
                {
                    camera = LoadCamera(elem);
                }
                else if (ElemName == "BackgroundColor")
                {
                    backgroundColor = LoadBackgroundColor(elem);
                }
                else if (ElemName == "LightSources")
                {
                    Lights = LoadLights(elem);
                }
                else if (ElemName == "LightSources")
                {
                    Lights = LoadLights(elem);
                }
                else if (ElemName == "Solids")
                {
                    solids = LoadSolids(elem);
                }
                else if (ElemName == "BRDF")
                {
                    myDictionary = LoadDictionary(elem);
                }
                else if (ElemName == "OutputFile")
                {
                    outputFilename = loadOutputFilename(elem);
                }
            }
        }

    }
}