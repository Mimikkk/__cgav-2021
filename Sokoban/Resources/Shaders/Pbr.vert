#version 450 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut { vec2 texture_coordinate; mat3 TBN; vec3 world_position; } vs_out;

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view, projection; } camera;
uniform mat4 model;

mat3 rotation = mat3(model);
mat3 calculate_tbn() {
    mat3 rotation = mat3(model);
    vec3 T = normalize(rotation * tangent);
    vec3 B = normalize(rotation * biTangent);
    vec3 N = normalize(rotation * normal);
    return mat3(T, B, N);
}
vec3 calculate_world_position() { return vec3(model * vec4(position, 1.0)); }

void main() {
    vs_out.texture_coordinate = texture_coordinate;
    vs_out.world_position = calculate_world_position();
    vs_out.TBN = calculate_tbn();

    gl_Position =  camera.projection * camera.view * vec4(vs_out.world_position, 1.0);
}