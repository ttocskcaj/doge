#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoords;
layout (location = 2) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 frag_texCoords;
out vec3 frag_normal;
out vec3 frag_pos;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    frag_texCoords = aTexCoords;
    frag_normal = mat3(transpose(inverse(model))) * aNormal; // Adjusted line
    frag_pos = vec3(model * vec4(aPosition, 1.0));
}
