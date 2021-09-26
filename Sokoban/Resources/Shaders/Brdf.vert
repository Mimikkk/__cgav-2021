#version 450 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut { vec2 texture_coordinate; } vs_out;

void main()
{
    vs_out.texture_coordinate = texture_coordinate;
    gl_Position = vec4(position, 1.0);
}