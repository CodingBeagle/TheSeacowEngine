#version 330
in vec2 TexCoord;

out vec4 color;

uniform bool isTexture;
uniform vec3 userColor;
uniform sampler2D ourTexture;

void main()
{
	if (isTexture)
	{
		color = texture(ourTexture, TexCoord);
	}
	else
	{
		color = vec4(userColor.x, userColor.y, userColor.z, 1.0);
	}
}