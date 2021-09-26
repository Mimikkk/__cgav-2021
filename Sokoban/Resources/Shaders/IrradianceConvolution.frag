#version 450 core
#define PI (3.14159265358979323846)
#define SAMPLE_STEP (0.25)

in VsOut { vec3 position; };

uniform samplerCube environment_map;

out vec4 color;

vec3 environment_texture(vec3 texture_coordinate) {
    return texture(environment_map, texture_coordinate).rgb;
}
vec3 spherical_to_cartesian(float theta, float phi) {
    return vec3(sin(theta) * cos(phi), sin(theta) * sin(phi), cos(theta));
}

void main()
{
    vec3 N = normalize(position);
    vec3 irradiance = vec3(0.0);

    vec3 right = normalize(cross(vec3(0.0, 1.0, 0.0), N));
    vec3 up = normalize(cross(N, right));

    int nrSamples = 0;
    for (float phi = 0.0; phi < 2.0 * PI; phi += SAMPLE_STEP) {
        for (float theta = 0.0; theta < 0.5 * PI; theta += SAMPLE_STEP) {
            vec3 tangent = spherical_to_cartesian(theta, phi);
            vec3 world_tangent = tangent.x * right + tangent.y * up + tangent.z * N;

            irradiance += cos(theta) * sin(theta) * environment_texture(world_tangent);
            ++nrSamples;
        }
    }

    color = vec4(PI * irradiance * (1.0 / float(nrSamples)), 1.0);
}
