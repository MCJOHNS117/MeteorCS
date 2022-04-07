--VERTEX--
#version 440 core

layout (location = 0) in vec4 position;
layout (location = 1) in vec2 textureCoordinate;

out vec2 vs_textureCoordinate;

layout (location = 20) uniform mat4 projection;
layout (location = 21) uniform mat4 modelView;

void main(void)
{
	gl_Position = projection * modelView * position;
	vs_textureCoordinate = textureCoordinate;
}
--VERTEX--

--FRAGMENT--
#version 450 core

in vec2 vs_textureCoordinate;
uniform sampler2D textureObject;
out vec4 color;

void main(void)
{
	color = texture(textureObject, vs_textureCoordinate);
}
--FRAGMENT--