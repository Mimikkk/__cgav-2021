#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut {
    vec2 texture_coordinate;
} vsout;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vsout.texture_coordinate = vec2(texture_coordinate.x, 1.0 - texture_coordinate.y);
    gl_Position = projection * view * model * vec4(position, 1.0f);
}