#version 450 core

in VsOut {
    vec2 texture_coordinate;
};

uniform sampler2D diffuse_map;

void main() {
    vec3 color = texture(diffuse_map, texture_coordinate).xyz;

    gl_FragColor = vec4(texture(diffuse_map, texture_coordinate).xyz, 1);
}
