#version 450 core

in struct { vec2 texCoord; } vsout;
out vec4 color;

uniform sampler2D diffuse;

void main()
{
    vec3 dif = texture(diffuse, vsout.texCoord).rgb;
    color = vec4(dif, 1.0);
}