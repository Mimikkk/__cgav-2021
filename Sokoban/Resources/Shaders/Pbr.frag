#version 450 core
#define PI (3.14159265358979323846)
#define GAMMA (2.2)
#define DIA_ELECTRIC_REFLECTANCE (vec3(0.04))
#define LIGHT_COUNT (50)

in VsOut { vec2 texture_coordinate; mat3 TBN; vec3 world_position; };

// TODO abstract Material into uniform block
uniform sampler2D albedo_map;
uniform sampler2D normal_map;
uniform sampler2D metallic_map;
uniform sampler2D roughness_map;
uniform sampler2D ambient_occlusion_map;

// TODO abstract Light into uniform block[]
uniform vec3 light_positions[LIGHT_COUNT];
uniform vec3 light_colors[LIGHT_COUNT];

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view, projection; } camera;

uniform mat4 model;

out vec4 fragment_color;

vec3 normal_from_map();
float distribution_GGX(vec3 N, vec3 H, float roughness);
float geometry_schlick_GGX(float NV, float roughness);
float geometry_smith(vec3 N, vec3 V, vec3 L, float roughness);
vec3 fresnel_schlick(float cosTheta, vec3 F0);

vec3 correct_gamma(vec3);
vec3 apply_hdr_tonemapping(vec3);
vec4 apply_diffuse(vec3);

vec3 calculate_albedo();
vec3 color_texture();
vec3 normal_texture();
float metallic_texture();
float roughness_texture();
float ambient_occlusion_texture();

float max_dot(vec3, vec3);
vec3 calculate_F0(vec3 albedo, float metallic) {
    return mix(DIA_ELECTRIC_REFLECTANCE, albedo, metallic);
}
vec3 calculate_radiance(vec3 light_position, vec3 light_color) {
    float attenuation = 1.0 / pow(length(light_position - world_position), 2);
    return light_color * attenuation;
}
vec3 calculate_reflectance(vec3 albedo, float metallic, float roughness) {
    vec3 N = normal_from_map();
    vec3 V = normalize(camera.position - world_position);

    vec3 outgoing_radiance = vec3(0);
    vec3 F0 = calculate_F0(albedo, metallic);
    for (int i = 0; i < LIGHT_COUNT; ++i) {
        vec3 L = normalize(light_positions[i] - world_position);
        vec3 H = normalize(V + L);

        float NDF = distribution_GGX(N, H, roughness);
        float G = geometry_smith(N, V, L, roughness);
        vec3 F = fresnel_schlick(max_dot(H, V), F0);

        vec3 kS = F;
        vec3 kD = (vec3(1.0) - kS) * (1 - metallic);

        vec3 specular = vec3(1) / (4 * max_dot(N, V) * max_dot(N, L) + 0.00001);
        vec3 radiance = calculate_radiance(light_positions[i], light_colors[i]);
        outgoing_radiance += (NDF * G * F) * (kD * albedo / PI + specular) * radiance * max_dot(N, L);
    }

    return outgoing_radiance;
}

vec3 calculate_ambience(vec3 albedo) { return vec3(0.03) * albedo * ambient_occlusion_texture(); }
vec3 apply_ambience(vec3 color, vec3 albedo) { return color + calculate_ambience(albedo); }

void main() {
    vec3 albedo = calculate_albedo();
    vec3 reflectance = calculate_reflectance(albedo, metallic_texture(), roughness_texture());

    fragment_color = apply_diffuse(correct_gamma(apply_hdr_tonemapping(apply_ambience(reflectance, albedo))));
}

vec3 calculate_albedo() { return pow(color_texture(), vec3(2.2)); }
vec3 color_texture() { return texture(albedo_map, texture_coordinate).rgb; }
vec3 normal_texture() { return texture(normal_map, texture_coordinate).xyz; }
float metallic_texture() { return texture(metallic_map, texture_coordinate).r; }
float roughness_texture() { return texture(roughness_map, texture_coordinate).r; }
float ambient_occlusion_texture() { return texture(ambient_occlusion_map, texture_coordinate).r; }

vec3 calculate_normal_map_tangent() { return 2 * normal_texture() - 1; }

float max_dot(vec3 first, vec3 second) { return max(dot(first, second), 0); }
vec3 normal_from_map() { return normalize(TBN * calculate_normal_map_tangent()); }

float distribution_GGX(vec3 N, vec3 H, float roughness) {
    float a2 = pow(roughness, 4);
    float NH2 = pow(max_dot(N, H), 2);

    return a2 / (PI * pow(1 + (a2 - 1) * NH2, 2));
}
float geometry_schlick_GGX(float NV, float roughness) {
    float k = pow(1 + roughness, 2) / 8;
    return NV / (k + NV * (1 - k));
}

float geometry_smith(vec3 N, vec3 V, vec3 L, float roughness) {
    return geometry_schlick_GGX(max_dot(N, V), roughness) * geometry_schlick_GGX(max_dot(N, L), roughness);
}
vec3 fresnel_schlick(float cosTheta, vec3 F0) { return F0 + (1 - F0) * pow(max(1 - cosTheta, 0), 5); }
vec3 correct_gamma(vec3 color) { return pow(color, vec3(1.0 / GAMMA)); }
vec3 apply_hdr_tonemapping(vec3 color) { return color / (color + vec3(1.0)); }
vec4 apply_diffuse(vec3 color) { return mix(vec4(color, 1.0), vec4(color_texture(), 1.0), 0.2); }
