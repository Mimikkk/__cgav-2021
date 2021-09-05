#version 450 core

in VsOut { vec3 texture_coordinate; };

uniform samplerCube skybox;

layout (location = 0) out vec4 color;

vec4 apply_skybox_cubemap() {
    return texture(skybox, texture_coordinate);
}

void main() {
    color = apply_skybox_cubemap();
}