#version 330
layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord;

uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

out vec2 TexCoord;

void main()
{
	gl_Position = projection * view * model * vec4(position.x, position.y, position.z, 1.0);
	TexCoord = vec2(texCoord.x, 1.0f - texCoord.y);
}