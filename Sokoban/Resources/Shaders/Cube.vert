#version 450 core
layout (location = 0) in vec3 position;

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view, projection; } camera;

out VsOut { vec3 position; } vs_out;

void main()
{
    vs_out.position = position;
    gl_Position =  camera.projection * camera.view * vec4(position, 1.0);
}