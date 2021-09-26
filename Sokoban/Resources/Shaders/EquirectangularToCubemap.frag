#version 330 core

in VsOut { vec3 position; };

uniform sampler2D equirectangular_map;

out vec4 color;

vec2 sample_spherical_map(vec3 position) {
    const vec2 invAtan = vec2(0.1591, 0.3183);
    return 0.5 + invAtan * vec2(atan(position.z, position.x), asin(position.y));
}

vec3 equirectangular_texture(vec2 texture_coordinate) { return texture(equirectangular_map, texture_coordinate).rgb; }

void main() {
    color = vec4(equirectangular_texture(sample_spherical_map(normalize(position))), 1.0);
}
