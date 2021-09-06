#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut {
    vec3 position;
    vec3 tangent_position;
    vec2 texture_coordinate;
    vec3 tangent_light_position;
    vec3 tangent_view_position;
} vs_out;

layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;

uniform mat4 model;
uniform vec3 light_position;

void main()
{
    vs_out.position = vec3(model * vec4(position, 1.0));
    vs_out.texture_coordinate = texture_coordinate;

    vec3 T = normalize(mat3(model) * tangent);
    vec3 B = normalize(mat3(model) * biTangent);
    vec3 N = normalize(mat3(model) * normal);
    mat3 TBN = transpose(mat3(T, B, N));

    vs_out.tangent_light_position = TBN * light_position;

    vs_out.tangent_view_position  = TBN * camera.position;
    vs_out.tangent_position  = TBN * vs_out.position;

    gl_Position = camera.projection * camera.view * model * vec4(position, 1.0);
}