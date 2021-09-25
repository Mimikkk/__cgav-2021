#version 450 core

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


vec2 MapParallax(vec2 coord, vec3 view_direction) {
    const float minLayers = 36;
    const float maxLayers = 128;

    float numLayers = mix(maxLayers, minLayers, abs(dot(vec3(0.0, 0.0, 1.0), view_direction)));

    // calculate the size of each layer
    float layerDepth = 1.0 / numLayers;

    // depth of current layer
    float currentLayerDepth = 0.0;

    // the amount to shift the texture coordinates per layer (from vector P)
    vec2 P = view_direction.xy / view_direction.z * height_scale;
    vec2 deltaTexCoords = P / numLayers;

    // get initial values
    vec2  currentTexCoords = coord;
    float currentDepthMapValue = texture(displacement_map, currentTexCoords).r;

    while (currentLayerDepth < currentDepthMapValue)
    {
        // shift texture coordinates along direction of P
        currentTexCoords -= deltaTexCoords;
        // get depthmap value at current texture coordinates
        currentDepthMapValue = texture(displacement_map, currentTexCoords).r;
        // get depth of next layer
        currentLayerDepth += layerDepth;
    }

    // get texture coordinates before collision (reverse operations)
    vec2 prevTexCoords = currentTexCoords + deltaTexCoords;

    // get depth after and before collision for linear interpolation
    float afterDepth  = currentDepthMapValue - currentLayerDepth;
    float beforeDepth = texture(displacement_map, prevTexCoords).r - currentLayerDepth + layerDepth;

    // interpolation of texture coordinates
    float weight = afterDepth / (afterDepth - beforeDepth);
    vec2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);

    return finalTexCoords;
}

bool ShouldDiscard(float x, float y) {
    return x > 1.0 || x < 0.0 || y > 1.0 || x < 0.0;
}
bool ShouldDiscard(vec2 vec) {
    return ShouldDiscard(vec.x, vec.y);
}
void DiscardTextureCoordinate(vec2 coordinate) {
    if (is_discardable && ShouldDiscard(coordinate)) discard;
}

void main() {
    vec3 view_direction = normalize(tangent_view_position - tangent_position);

    vec2 offset_texture_coord = MapParallax(texture_coordinate, view_direction);
    DiscardTextureCoordinate(offset_texture_coord);

    vec3 light_direction = normalize(tangent_light_position - tangent_position);
    vec3 diffuse_color = texture(diffuse_map, offset_texture_coord).rgb;
    vec3 normal = normalize(2*texture(normal_map, offset_texture_coord).rgb -1);

    vec3 reflect_direction = reflect(-light_direction, normal);
    vec3 halfway_direction = normalize(light_direction + view_direction);

    vec3 ambient = 0.1 * diffuse_color;

    vec3 diffuse = max(dot(reflect_direction, normal), 0.0) * diffuse_color;
    vec3 specular = vec3(0.2) * pow(max(dot(normal, halfway_direction), 0.0), 32.0);

    color = vec4(ambient + diffuse + specular, 1.0);
}