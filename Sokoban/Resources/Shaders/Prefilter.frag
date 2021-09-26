#version 450 core
#define PI (3.14159265358979323846)
#define SAMPLE_COUNT (1024u)
#define CUBE_RESOLUTION (512.0)
#define CUBE_RESOLUTION2 (262144.0)

in VsOut { vec3 position; };

out vec4 color;

uniform samplerCube environment_map;
uniform float roughness;

float max_dot(vec3 first, vec3 second) { return max(dot(first, second), 0); }

float distribution_GGX(vec3 N, vec3 H, float roughness) {
    float a2 = pow(roughness, 4);
    float NH2 = pow(max_dot(N, H), 2);

    return a2 / (PI * pow(1 + (a2 - 1) * NH2, 2));
}

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

vec3 calculate_halfway(vec2 Xi, float roughness) {
    float a = pow(roughness, 2);
    float phi = 2.0 * PI * Xi.x;
    float cosTheta = sqrt((1.0 - Xi.y) / (1.0 + (pow(a, 2) - 1.0) * Xi.y));
    float sinTheta = sqrt(1.0 - cosTheta*cosTheta);
    return vec3(cos(phi) * sinTheta, sin(phi) * sinTheta, cosTheta);
}

vec3 importance_sample_GGX(vec2 Xi, vec3 N, float roughness) {
    vec3 H = calculate_halfway(Xi, roughness);

    vec3 up = abs(N.z) < 0.999 ? vec3(0.0, 0.0, 1.0) : vec3(1.0, 0.0, 0.0);
    vec3 tangent = normalize(cross(up, N));
    vec3 bitangent = cross(N, tangent);

    vec3 sample_vec = tangent * H.x + bitangent * H.y + N * H.z;
    return normalize(sample_vec);
}

vec3 environment_texture(vec3 texture_coordinate, float mipLevel) {
    return textureLod(environment_map, texture_coordinate, mipLevel).rgb;
}

void main() {
    vec3 N = normalize(position);

    vec3 R = N;
    vec3 V = R;

    vec3 prefiltered_color = vec3(0.0);
    float total_weight = 0.0;

    for (uint i = 0u; i < SAMPLE_COUNT; ++i) {
        vec2 Xi = hammersley(i, SAMPLE_COUNT);
        vec3 H = importance_sample_GGX(Xi, N, roughness);
        vec3 L  = normalize(2.0 * dot(V, H) * H - V);

        float NL = max_dot(N, L);
        if (NL > 0.0) {
            float D   = distribution_GGX(N, H, roughness);
            float NH = max_dot(N, H);
            float HV = max_dot(H, V);
            float pdf = D * NH / (4.0 * HV) + 0.000001;

            float sa_texel  = 4.0 * PI / (6.0 * CUBE_RESOLUTION2);
            float sa_sample = 1.0 / (float(SAMPLE_COUNT) * pdf + 0.0001);

            float mipLevel = roughness == 0.0 ? 0.0 : 0.5 * log2(sa_sample / sa_texel);

            prefiltered_color += environment_texture(L, mipLevel) * NL;
            total_weight += NL;
        }
    }

    color = vec4(prefiltered_color / total_weight, 1.0);
}
