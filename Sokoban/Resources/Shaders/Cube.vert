#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;

uniform mat4 model;
layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;

out VsOut {
    vec3 normal;
    vec3 position;
    vec2 texture_coordinate;
} vs_out;

void main()
{
    gl_Position = camera.projection * camera.view * model * vec4(position, 1.0);

    vs_out.position = vec3(model * vec4(position, 1.0));
    vs_out.normal = mat3(transpose(inverse(model))) * position;
    vs_out.texture_coordinate = texture_coordinate;
}