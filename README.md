# Documentation of the solution

## Author
Anastasia Grigoryan

-----

## Checkpoint 1

## Command line arguments
-c        ---Config file. <br />

## Config file (input data)
<OutputFile> - Name of the output file. <br />
<Width> - Width of the image. <br />
<Height>- Height of the image. <br />
<Patels> - Number of patels in flower. <br />

### Algorithm
Program create an image with random gradually colored flower in the center of the image. User could choose amount of patels and image size.

### Use of AI
Used for color matching in float values (for example which float values use for soft dark green color).

## Checkpoint 2

## Command line arguments
-c        ---Default config file. <br />

## Config file (input data)
Config file is config.xml. <br />

All settings are in Config node:
```
<Config>
...
</Config>
```
Setting of file name and background color:
```
<OutputFile> - Name of the output file. <br />
<BackgroundColor> - Color of background. <br />
```
Camera settings inside of Camera node: <br />
```
<Camera>
	<Resolution> - resolution of the image. <br />
	<Position> - position of the camera. <br />
	<Direction> - direction of the camera ray. <br />
	<FOV> - Field of view. <br />
</Camera>
```
Light sources settings inside of LightSources node (have three options: Ambient, Point, Directional): <br />
```
	<LightSources>
		<Ambient> - type of light. <br /> 
			<Intensity> - intensity of the light. <br />
		<Point> - type of the light. <br />
			<Intensity> - intensity of the light. <br />
			<Position> - position of the light. <br />
		...
	</LightSources>
 ```
Material/BRDF setting inside of BRDF node: <br />
```
	<Shader>
		<Name> - name of material. <br />
		<Ambient> - ambient coeficent. <br />
		<Diffuse> - diffuse coeficent. <br />
		<Specular> - specular coeficent. <br />
		<Highlight> - highlight intensity. <br />
		<Color> - color of material. <br />
	</Shader>
```
Solids inside of solids node (2 shapes: spheres and planes): <br />
```
<Solids>
	<Sphere>
		<Material> - material name. <br />
		<Position> - Position of the sphere. <br />
		<Radius> - radius of the sphere. <br />
	</Sphere>
	<Plane>
		<Material> - material name. <br />
		<Position> - Position of the plane. <br />
		<Normal> - Normal vector of the plane. <br />
	</Plane>
</Solids>
```
### Algorithm
Program create simple perspective camera, which generates rays. Simulate simple Phong material and different types of lightning. Contains 2 solids (plane and sphere) and find intersection for rays and these solids to calculate color of the output pixel based on position, light and material.

### Use of AI
Used to found out what i need to use to read float values from xml file without rounding, as i have the issue that my 0.2 value rounds to 0. So chatGPT said to me use: 
using System.Globalization;
and 
float.TryParse(..., NumberStyles.Float, CultureInfo.InvariantCulture, out ...); instead of float.TryParse(element, out x);

## Checkpoint 3

## Command line arguments
-c        ---Default config file. <br />

## Config file (input data)
Config file is the same as in the CheckPoint 2.

### Algorithm
Program add shadows to the image with solids and based on lights calculate ray tracing on chosen depth.
Result looks like in the image below:

![Sample result](rt004/outputRay(png).png)



### Use of AI
No usage
