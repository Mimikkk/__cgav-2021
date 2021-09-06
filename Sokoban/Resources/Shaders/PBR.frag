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

out vec4 color;

vec2 MapParallax(vec2 coord, vec3 view_direction) {

    const float minLayers = 8;
    const float maxLayers = 32;

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
    if (ShouldDiscard(coordinate)) discard;
}

void main() {
    // offset texture coordinates with Parallax Mapping
    vec3 view_direction = normalize(tangent_view_position - tangent_position);
    vec2 paralax_texture_coord = texture_coordinate;

    paralax_texture_coord = MapParallax(texture_coordinate, view_direction);
    DiscardTextureCoordinate(paralax_texture_coord);

    // obtain normal from normal map
    vec3 normal = texture(normal_map, paralax_texture_coord).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    // get diffuse color
    vec3 diffuse_color = texture(diffuse_map, paralax_texture_coord).rgb;

    // ambient
    vec3 ambient = 0.1 * diffuse_color;

    // diffuse
    vec3 light_direction = normalize(tangent_light_position - tangent_position);
    float diff = max(dot(light_direction, normal), 0.0);
    vec3 diffuse = diff * diffuse_color;

    // specular    
    vec3 reflect_direction = reflect(-light_direction, normal);
    vec3 halfwayDirection = normalize(light_direction + view_direction);
    float spec = pow(max(dot(normal, halfwayDirection), 0.0), 32.0);

    vec3 specular = vec3(0.2) * spec;
    color = vec4(ambient + diffuse + specular, 1.0);
}