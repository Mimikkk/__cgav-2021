#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut { vec2 texture_coordinate; } vs_out;

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view; mat4 projection; } camera;

uniform mat4 model;

void main() {
    vs_out.texture_coordinate = texture_coordinate;
    gl_Position = camera.projection * camera.view * model * vec4(position, 1.0);
}
