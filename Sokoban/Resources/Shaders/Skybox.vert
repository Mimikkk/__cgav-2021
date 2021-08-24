﻿#version 330 core
layout (location = 0) in vec3 position;

out vec3 textureCoordinate;

uniform mat4 projection;
uniform mat4 view;

void main()
{
    textureCoordinate = position;
    gl_Position = (projection * view * vec4(position, 1.0)).xyww;
}  