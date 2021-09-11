#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texture_coordinate;
layout (location = 2) in vec3 normal;

uniform mat4 model;
layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;

void main()
{
    gl_Position = camera.projection * camera.view * model * vec4(position, 1);
}