#version 450 core

layout (location = 0) out vec4 FragColor;

in VertexData {
    vec3 texture_coordinate;
} fs_in;

uniform samplerCube skybox;

vec4 apply_skybox_cubemap() {
    return texture(skybox, fs_in.texture_coordinate);
}

void main() {
    FragColor = apply_skybox_cubemap();
}