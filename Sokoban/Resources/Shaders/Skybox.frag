#version 450 core
#define GAMMA (2.2)

in VsOut { vec3 position; };

uniform samplerCube environment_map;

layout (location = 0) out vec4 color;

vec3 environment_texture() {
    return textureLod(environment_map, position, 0).rgb;
}

vec3 apply_gamma_correction(vec3 color) { return pow(color, vec3(1.0 / GAMMA)); }
vec3 apply_hdr_tonemapping(vec3 color) { return color / (color + vec3(1.0)); }

void main() {
    color = vec4(apply_hdr_tonemapping(apply_gamma_correction(environment_texture())), 1);
}