# Ray Tracer (C#)

![Showcase](rt004/outputRay(png).png)

A CPU-based ray tracer written in C# as part of a Computer Graphics university project.

The goal of this project was to build a custom rendering pipeline from scratch and gain a deeper understanding of ray tracing, lighting models, shading, and recursive rendering techniques.

---

## Features

### Camera

* Perspective camera
* Configurable field of view (FOV)
* Adjustable image resolution
* Custom camera position and direction

### Geometry

* Sphere intersection
* Plane intersection

### Lighting

* Ambient lights
* Point lights
* Directional lights

### Materials

* Phong shading model
* Ambient component
* Diffuse component
* Specular highlights
* Configurable material parameters

### Ray Tracing

* Primary ray generation
* Shadow rays
* Recursive ray tracing
* Configurable recursion depth

### Scene Configuration

* XML-based scene description
* Custom materials
* Multiple light sources
* Configurable camera settings

---

## Technical Details

### Implemented From Scratch

* Ray generation from a perspective camera
* Ray-sphere intersection tests
* Ray-plane intersection tests
* Phong illumination model
* Shadow ray calculations
* Recursive ray tracing
* Scene loading from XML

### Technologies

* C#
* .NET
* XML scene configuration
* Object-oriented rendering architecture

---

## Rendering Pipeline

1. Generate primary rays from the camera.
2. Find the closest intersection with scene geometry.
3. Evaluate material properties at the hit point.
4. Calculate lighting contribution from all light sources.
5. Cast shadow rays to determine visibility.
6. Spawn additional rays for recursive tracing.
7. Accumulate the final pixel color.
8. Save the rendered image.

---

## Example Results

### Basic Phong Shading

![Phong](images/phong.png)

### Shadows

![Shadows](images/shadows.png)

### Recursive Ray Tracing

![RayTracing](images/raytracing.png)

---

## Learning Outcomes

This project helped me gain practical experience with:

* Computer graphics fundamentals
* Ray tracing algorithms
* Lighting and shading models
* Surface-material interaction
* Recursive rendering techniques
* Rendering architecture design

---

## Author

**Anastasia Grigoryan**

Technical Artist | Unreal Engine | Computer Graphics
