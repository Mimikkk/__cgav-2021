#version 450
in vec3 f_Normal;
in vec3 f_Pos;
in vec2 f_TexCoords;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light light;
uniform vec3 viewPos;

out vec4 FragColor;

void main()
{
    vec3 ambient = light.ambient * texture(material.diffuse, f_TexCoords).rgb;

    vec3 norm = normalize(f_Normal);
    vec3 lightDirection = normalize(light.position - f_Pos);
    float diff = max(dot(norm, lightDirection), 0.0);
    vec3 diffuse = light.diffuse * (diff * texture(material.diffuse, f_TexCoords).rgb);

    vec3 viewDirection = normalize(viewPos - f_Pos);
    vec3 reflectDirection = reflect(-lightDirection, norm);
    float spec = pow(max(dot(viewDirection, reflectDirection), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * texture(material.specular, f_TexCoords).rgb);

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}