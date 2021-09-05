#version 450 core

in VsOut {
    vec2 texture_coordinate;
};

uniform sampler2D diffuse;

out vec4 color;

void main()
{
    vec3 dif = texture(diffuse, texture_coordinate).rgb;
    color = vec4(dif, 1.0);
}