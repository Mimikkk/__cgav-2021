#version 450 core

in VsOut { vec2 texture_coordinate; };

uniform sampler2D diffuse_map;

out vec4 color;

void main() {
    color = texture(diffuse_map, texture_coordinate);
}
