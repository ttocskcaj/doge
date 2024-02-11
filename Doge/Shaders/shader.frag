#version 330 core

in vec2 frag_texCoords;
in vec3 frag_normal;
in vec3 frag_pos;

out vec4 out_color;

uniform sampler2D tex;
uniform vec3 lightColour;
uniform vec3 lightPos;
uniform vec3 viewPos;

void main()
{
    // Get the texture color
    vec4 texColor = texture(tex, frag_texCoords);
    vec4 premultipliedColor = vec4(texColor.rgb * texColor.a, texColor.a);
    vec4 backgroundColor = vec4(0.8, 0.8, 0.8, 1.0);
    vec4 objectColor = premultipliedColor + backgroundColor * (1.0 - premultipliedColor.a);

    // Ambient Lighting
    float ambientStrength = 0.145;
    vec3 ambient = ambientStrength * lightColour;

    // Diffuse lighting
    vec3 norm = normalize(frag_normal);
    vec3 lightDir = normalize(lightPos - frag_pos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColour;
    
    // Specular Lighting
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - frag_pos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColour;

    // Final output
    vec4 result = vec4(ambient + diffuse + specular, 1) * objectColor;
    out_color = result;
}