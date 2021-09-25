#version 450 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texture_coordinate;
layout (location = 3) in vec3 tangent;
layout (location = 4) in vec3 biTangent;

out VsOut { vec2 texture_coordinate; vec3 world_position, normal, tangent, biTangent; } vs_out;

layout (std140, binding = 0) uniform CameraBlock { vec3 position; mat4 view, projection; } camera;
uniform mat4 model;

void main()
{
    vs_out.texture_coordinate = texture_coordinate;
    vs_out.world_position = vec3(model * vec4(position, 1.0));
    vs_out.normal = mat3(model) * normal;
    vs_out.tangent = tangent;
    vs_out.biTangent = biTangent;
    
    gl_Position =  camera.projection * camera.view * vec4(vs_out.world_position, 1.0);
}