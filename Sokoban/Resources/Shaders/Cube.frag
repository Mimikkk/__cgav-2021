#version 450 core

in VsOut {
    vec3 normal;
    vec3 position;
    vec2 texture_coordinate;
};

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
};

uniform Material material;
uniform Light light;

layout (std140, binding = 0) uniform CameraBlock {
    vec3 position;
    mat4 view;
    mat4 projection;
} camera;


out vec4 color;

void main()
{
    vec4 dif = texture(material.diffuse, texture_coordinate);
    vec4 ambient = light.ambient * texture(material.diffuse, texture_coordinate);

    vec3 norm = normalize(normal);
    vec3 lightDirection = normalize(light.position - position);
    float diff = max(dot(norm, lightDirection), 0.0);
    vec4 diffuse = light.diffuse * (diff * texture(material.diffuse, texture_coordinate));

    vec3 viewDirection = normalize(camera.position - position);
    vec3 reflectDirection = reflect(-lightDirection, norm);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), material.shininess);


    vec4 specular = light.specular * (spec * texture(material.specular, texture_coordinate));

    vec4 result = ambient + diffuse + specular;
    color = result;
}
