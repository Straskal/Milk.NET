#version 450 core

layout(location = 0) in vec2 position;
layout(location = 1) in vec4 color;

uniform mat4 projectionMatrix;

out vec4 Color;

void main() 
{
	Color = color;
	gl_Position = projectionMatrix * vec4(position, 0.0, 1.0);
}