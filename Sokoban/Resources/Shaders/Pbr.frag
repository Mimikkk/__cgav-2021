#version 450 core
#define PI (3.14159265358979323846)
#define GAMMA (2.2)
#define DIA_ELECTRIC_REFLECTANCE (vec3(0.04))
#define LIGHT_COUNT (50)
#define MAX_REFLECTION_LOD (4.0)
#define MIN_LAYERS (16)
#define MAX_LAYERS (128)
#define RIGHT (vec3(0.0, 0.0, 1.0))

in VsOut { vec2 texture_coordinate; mat3 TBN; vec3 world_position; };

// TODO abstract Material into uniform block
uniform sampler2D albedo_map;
uniform sampler2D normal_map;
uniform sampler2D metallic_map;
uniform sampler2D roughness_map;
uniform sampler2D ambient_occlusion_map;

// Parallax
uniform sampler2D displacement_map;
uniform float height_scale;

// IBL
uniform samplerCube irradiance_map;
uniform samplerCube prefilter_map;
uniform sampler2D brdf_LUT_map;

// TODO abstract Light into uniform block[]

struct Light { vec3 position, color; };
uniform Light lights[LIGHT_COUNT];

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view, projection; } camera;

uniform mat4 model;

out vec4 fragment_color;

float max_dot(vec3 first, vec3 second) { return max(dot(first, second), 0); }

vec2 coordinate;
vec3 N, V, R;
vec3 albedo;
float roughness, metallic;


vec3 color_texture() { return texture(albedo_map, coordinate).rgb; }
vec3 normal_texture() { return texture(normal_map, coordinate).rgb; }
float metallic_texture() { return texture(metallic_map, coordinate).r; }
float roughness_texture() { return texture(roughness_map, coordinate).r; }
float ambient_occlusion_texture() { return texture(ambient_occlusion_map, coordinate).r; }
float displacement_texture(vec2 coord) { return texture(displacement_map, coord).r; }


vec3 irradiance_texture(vec3 N) { return texture(irradiance_map, N).rgb; }
vec3 prefilter_texture(vec3 R) { return textureLod(prefilter_map, R, roughness * MAX_REFLECTION_LOD).rgb; }
vec2 brdf_texture(float NV) { return texture(brdf_LUT_map, vec2(NV, roughness)).rg; }

vec3 normal_from_map() { return normalize(TBN * (2 * normal_texture() - 1)); }

float distribution_GGX(vec3 N, vec3 H) {
    float a2 = pow(roughness, 4);
    float NH2 = pow(max_dot(N, H), 2);

    return a2 / (PI * pow(1 + (a2 - 1) * NH2, 2));
}
float geometry_schlick_GGX(float NV) {
    float k = pow(roughness, 2) / 2.0;
    return NV / (k + NV * (1 - k));
}
float geometry_smith(vec3 N, vec3 V, vec3 L) {
    return geometry_schlick_GGX(max_dot(N, V)) * geometry_schlick_GGX(max_dot(N, L));
}
vec3 fresnel_schlick(float cosTheta, vec3 F0) { return F0 + (1 - F0) * pow(max(1 - cosTheta, 0), 5); }
vec3 fresnel_schlick_roughness(float cosTheta, vec3 F0) {
    return F0 + (max(vec3(1.0 - roughness), F0) - F0) * pow(max(1.0 - cosTheta, 0.0), 5.0);
}


vec3 calculate_F0() {
    return mix(DIA_ELECTRIC_REFLECTANCE, albedo, metallic);
}
vec3 calculate_radiance(Light light) {
    float attenuation = 1.0 / pow(length(light.position - world_position), 2);
    return light.color * attenuation;
}
vec3 calculate_reflectance() {
    vec3 outgoing_radiance = vec3(0);

    vec3 F0 = calculate_F0();
    for (int i = 0; i < LIGHT_COUNT; ++i) {
        vec3 L = normalize(lights[i].position - world_position);
        vec3 H = normalize(V + L);

        float NDF = distribution_GGX(N, H);
        float G = geometry_smith(N, V, L);
        vec3 F = fresnel_schlick(max_dot(H, V), F0);

        vec3 kS = F;
        vec3 kD = (vec3(1.0) - kS) * (1 - metallic);

        vec3 specular = vec3(1) / (4 * max_dot(N, V) * max_dot(N, L) + 0.00001);
        vec3 radiance = calculate_radiance(lights[i]);
        outgoing_radiance += (NDF * G * F) * (kD * albedo / PI + specular) * radiance * max_dot(N, L);
    }

    return outgoing_radiance;
}

vec3 apply_gamma_correction(vec3 color) { return pow(color, vec3(1.0 / GAMMA)); }
vec3 apply_hdr_tonemapping(vec3 color) { return color / (color + vec3(1.0)); }
vec3 calculate_ambience(vec3 albedo) { return vec3(0.03) * albedo * ambient_occlusion_texture(); }
vec3 apply_ambience(vec3 color, vec3 albedo) { return color + calculate_ambience(albedo); }
vec3 calculate_albedo() { return pow(color_texture(), vec3(GAMMA)); }

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

void main() {
    vec3 view_direction = normalize(inverse(TBN) * camera.position - inverse(TBN) * world_position);
    coordinate = map_parallax(texture_coordinate, view_direction);
    coordinate = texture_coordinate;

    N = normal_from_map();
    V = normalize(camera.position - world_position);
    R = reflect(-V, N);
    roughness = roughness_texture();
    metallic = metallic_texture();
    albedo = calculate_albedo();

    vec3 F0 = calculate_F0();
    vec3 F = fresnel_schlick_roughness(max_dot(N, V), F0);
    vec3 kS = F;
    vec3 kD = (1.0 - kS) * (1.0 - metallic);
    vec3 diffuse = albedo * irradiance_texture(N);

    vec3 prefilter = prefilter_texture(R);
    vec2 brdf = brdf_texture(max_dot(N, V));

    vec3 specular = prefilter * (F * brdf.x + brdf.y);
    vec3 ambient = (kD * diffuse + specular) * ambient_occlusion_texture();
    vec3 color = ambient + calculate_reflectance();

    fragment_color = vec4(apply_gamma_correction(apply_hdr_tonemapping(color)), 1);
}
