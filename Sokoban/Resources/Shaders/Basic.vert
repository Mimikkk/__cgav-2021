#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

void main() {
    gl_Position = position;
}
