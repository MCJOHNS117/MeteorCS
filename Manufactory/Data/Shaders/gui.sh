--VERTEX--
#version 440 core

layout (location = 0) in vec2 vertex;
layout (location = 1) in vec2 textureCoord;

out vec2 TexCoords;

layout (location = 20) uniform mat4 model;
layout (location = 21) uniform mat4 projection;

void main(void)
{
	TexCoords = textureCoord;
	gl_Position = projection * model * vec4(vertex, 0.0, 1.0);
}
--VERTEX--

--FRAGMENT--
#version 450 core

in vec2 TexCoords;
out vec4 color;

uniform sampler2D textureObject;

void main(void)
{
	color = texture(textureObject, TexCoords);
}
--FRAGMENT--