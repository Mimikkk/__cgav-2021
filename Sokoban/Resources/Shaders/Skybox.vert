#version 450 core

layout (location = 0) in vec3 position;

layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;

out VsOut { vec3 texture_coordinate; } vs_out;

vec4 calculate_skybox_position() {
    return (camera.projection * mat4(mat3(camera.view)) * vec4(position, 1.0)).xyww;
}

void main() {
    gl_Position = calculate_skybox_position();
    vs_out.texture_coordinate = position;
}  
