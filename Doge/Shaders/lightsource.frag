
#version 330 core
out vec4 FragColor;

uniform vec3 modelColour;

void main()
{
    FragColor = vec4(modelColour, 1);
}
