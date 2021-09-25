#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;

layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;

out VsOut {
    vec2 texture_coordinate;
    vec3 world_position;
    vec3 normal;
} vs_out;

uniform mat4 model;

void main() {
    vs_out.texture_coordinate = texture_coordinate;
    vs_out.world_position = vec3(model * vec4(position, 1));
    vs_out.normal = mat3(model) * normal;
    
    gl_Position = camera.projection * camera.view * model * vec4(vs_out.world_position, 1.0);
}
