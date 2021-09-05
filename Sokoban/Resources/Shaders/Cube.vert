#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;

layout (std140, binding = 1) uniform MatrixBlock {
    mat4 model;
    mat4 view;
    mat4 projection;
};

out VsOut {
    vec3 normal;
    vec3 position;
    vec2 texture_coordinate;
} vs_out;

void main()
{
    gl_Position = projection * view * model * vec4(position, 1.0);
    vs_out.position = vec3(model * vec4(position, 1.0));
    vs_out.normal = mat3(transpose(inverse(model))) * position;
    vs_out.texture_coordinate = texture_coordinate;
}