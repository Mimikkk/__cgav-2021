#version 450 core
#define MIN_LAYERS (16)
#define MAX_LAYERS (128)
#define RIGHT (vec3(0.0, 0.0, 1.0))


in VsOut {
    vec3 position;
    vec3 tangent_position;
    vec2 texture_coordinate;
    vec3 tangent_light_position;
    vec3 tangent_view_position;
};

uniform sampler2D diffuse_map;
uniform sampler2D normal_map;
uniform sampler2D displacement_map;

uniform float height_scale;
uniform bool is_discardable;

out vec4 color;

float max_dot(vec3 first, vec3 second) { return max(dot(first, second), 0); }

float displacement_texture(vec2 coordinate) { return texture(displacement_map, coordinate).r; }

vec2 map_parallax(vec2 coord, vec3 view_direction) {
    float layer_count = mix(MAX_LAYERS, MIN_LAYERS, abs(dot(RIGHT, view_direction)));

    float layer_depth = 1.0 / layer_count;
    float current_depth = 0.0;

    vec2 P = view_direction.xy / view_direction.z * height_scale;
    vec2 sample_size = P / layer_count;

    vec2  current_coordinate = coord;
    float depth_value = displacement_texture(current_coordinate);

    while (current_depth < depth_value)
    {
        current_coordinate -= sample_size;
        depth_value = displacement_texture(current_coordinate);
        current_depth += layer_depth;
    }

    vec2 previous_coordinate = current_coordinate + sample_size;

    float after_depth  = depth_value - current_depth;
    float before_depth = displacement_texture(previous_coordinate) - current_depth + layer_depth;

    float weight = after_depth / (after_depth - before_depth);

    return previous_coordinate * weight + current_coordinate * (1.0 - weight);
}

bool should_discard(float x, float y) {
    return x > 1.0 || x < 0.0 || y > 1.0 || x < 0.0;
}
bool should_discard(vec2 vec) {
    return should_discard(vec.x, vec.y);
}
void discard_texture_coordinate(vec2 coordinate) {
    if (is_discardable && should_discard(coordinate)) discard;
}

vec3 normal_texture(vec2 coord) { return normalize(2 * texture(normal_map, coord).rgb - 1); }

void main() {
    vec3 view_direction = normalize(tangent_view_position - tangent_position);

    vec2 offset_texture_coord = map_parallax(texture_coordinate, view_direction);
    discard_texture_coordinate(offset_texture_coord);

    vec3 light_direction = normalize(tangent_light_position - tangent_position);
    vec3 diffuse_color = texture(diffuse_map, offset_texture_coord).rgb;
    vec3 normal = normal_texture(offset_texture_coord);

    vec3 reflect_direction = reflect(-light_direction, normal);
    vec3 halfway_direction = normalize(light_direction + view_direction);

    vec3 ambient = 0.1 * diffuse_color;

    vec3 diffuse = diffuse_color * max_dot(reflect_direction, normal);
    vec3 specular = vec3(0.2) * pow(max_dot(normal, halfway_direction), 32.0);

    color = vec4(ambient + diffuse + specular, 1.0);
}
