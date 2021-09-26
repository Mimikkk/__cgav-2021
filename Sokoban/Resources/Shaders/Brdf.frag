#version 450 core
#define PI (3.14159265358979323846)
#define SAMPLE_COUNT (1024u)
in VsOut { vec2 texture_coordinate; } vs_out;
out vec2 color;

float max_dot(vec3 first, vec3 second) { return max(dot(first, second), 0); }

float radical_inverse_VdC(uint bits) {
    bits = (bits << 16u) | (bits >> 16u);
    bits = ((bits & 0x55555555u) << 1u) | ((bits & 0xAAAAAAAAu) >> 1u);
    bits = ((bits & 0x33333333u) << 2u) | ((bits & 0xCCCCCCCCu) >> 2u);
    bits = ((bits & 0x0F0F0F0Fu) << 4u) | ((bits & 0xF0F0F0F0u) >> 4u);
    bits = ((bits & 0x00FF00FFu) << 8u) | ((bits & 0xFF00FF00u) >> 8u);
    return float(bits) * 2.3283064365386963e-10;
}
vec2 hammersley(uint i, uint N) {
    return vec2(float(i)/float(N), radical_inverse_VdC(i));
}

vec3 calculate_halfway(vec2 Xi) {
    float phi = 2.0 * PI * Xi.x;
    float cosTheta = sqrt((1.0 - Xi.y) / (1.0 + (pow(a, 2) - 1.0) * Xi.y));
    float sinTheta = sqrt(1.0 - cosTheta*cosTheta);
    return vec3(cos(phi) * sinTheta, sin(phi) * sinTheta, cosTheta);
}

vec3 importance_sample_GGX(vec2 Xi, vec3 N, float roughness) {
    float a = pow(roughness, 2);
    vec3 H = calculate_halfway(Xi);

    vec3 up = abs(N.z) < 0.999 ? vec3(0.0, 0.0, 1.0) : vec3(1.0, 0.0, 0.0);
    vec3 tangent = normalize(cross(up, N));
    vec3 bitangent = cross(N, tangent);

    vec3 sample_vec = tangent * H.x + bitangent * H.y + N * H.z;
    return normalize(sample_vec);
}
float geometry_schlick_GGX(float NdotV, float roughness) {
    float k = pow(roughness, 2) / 2.0;
    return NdotV /  (k + NdotV * (1.0 - k));
}
float geometry_smith(vec3 N, vec3 V, vec3 L, float roughness) {
    return geometry_schlick_GGX(max_dot(N, V), roughness) * geometry_schlick_GGX(max_dot(N, L), roughness);
}

vec2 integrate_BRDF(float NV, float roughness) {
    vec3 V = vec3(sqrt(1.0 - pow(NV, 2)), 0, NV);
    vec3 N = vec3(0.0, 0.0, 1.0);

    float A = 0.0;
    float B = 0.0;
    for (uint i = 0u; i < SAMPLE_COUNT; ++i)
    {
        // generates a sample vector that's biased towards the
        // preferred alignment direction (importance sampling).
        vec2 Xi = hammersley(i, SAMPLE_COUNT);
        vec3 H = importance_sample_GGX(Xi, N, roughness);
        vec3 L = normalize(2.0 * dot(V, H) * H - V);

        float NL = max(L.z, 0.0);
        float NH = max(H.z, 0.0);
        float VH = max(dot(V, H), 0.0);

        if (NL > 0.0) {
            float G = geometry_smith(N, V, L, roughness);
            float G_vis = (G * VH) / (NH * NV);
            float Fc = pow(1.0 - VH, 5.0);

            A += (1.0 - Fc) * G_vis;
            B += Fc * G_vis;
        }
    }

    A /= float(SAMPLE_COUNT);
    B /= float(SAMPLE_COUNT);
    return vec2(A, B);
}

void main() {
    color = IntegrateBRDF(texture_coordinate.x, texture_coordinate.y);
}