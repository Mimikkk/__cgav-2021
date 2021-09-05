#version 450 core

layout (location = 0) in vec3 position;

layout (std140, binding = 0) uniform CameraBlock {
    vec3 _;
    mat4 view;
    mat4 projection;
};

out VsOut { vec3 texture_coordinate; } vs_out;

vec4 calculate_skybox_position() {
    return (projection * mat4(mat3(view)) * vec4(position, 1.0)).xyww;
}

void main() {
    vs_out.texture_coordinate = position;
    gl_Position = calculate_skybox_position();
}  
