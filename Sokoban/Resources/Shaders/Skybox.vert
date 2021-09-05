#version 450 core

layout (location = 0) in vec3 position;

out VertexData {
    vec3 texture_coordinate;
} vs_out;

layout (std140, binding = 0) uniform MatrixBlock {
    mat4 view;
    mat4 projection;
};

vec4 calculate_skybox_position() {
    return (projection * view * vec4(position, 1.0)).xyww;
}

void main() {
    vs_out.texture_coordinate = position;
    gl_Position = calculate_skybox_position();
}  
