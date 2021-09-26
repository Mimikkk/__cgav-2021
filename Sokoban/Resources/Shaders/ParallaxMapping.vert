#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

uniform mat4 model;
uniform vec3 light_position;

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view; mat4 projection; } camera;

out VsOut {
    vec3 position;
    vec3 tangent_position;
    vec2 texture_coordinate;
    vec3 tangent_light_position;
    vec3 tangent_view_position;
} vs_out;

mat3 rotation = mat3(model);
mat3 calculate_tbn() {
    vec3 T = normalize(rotation * tangent);
    vec3 B = normalize(rotation * biTangent);
    vec3 N = normalize(rotation * normal);
    return mat3(T, B, N);
}

void main() {
    vs_out.position = vec3(model * vec4(position, 1.0));
    vs_out.texture_coordinate = texture_coordinate;

    mat3 TBN = transpose(calculate_tbn());
    vs_out.tangent_light_position = TBN * light_position;
    vs_out.tangent_view_position  = TBN * camera.position;
    vs_out.tangent_position  = TBN * vs_out.position;

    gl_Position = camera.projection * camera.view * model * vec4(position, 1.0);
}
