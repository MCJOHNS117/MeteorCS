--VERTEX--
#version 440 core

layout (location = 0) in vec2 position;
layout (location = 1) in vec2 textureCoordinate;

out vec2 vs_textureCoordinate;

layout (location = 3) uniform vec2 translation;

void main(void)
{
	gl_Position = vec4(position, 0.0, 1.0);
	vs_textureCoordinate = textureCoordinate;
}
--VERTEX--

--FRAGMENT--
#version 450 core

in vec2 vs_textureCoordinate;

uniform vec4 textColor;
uniform sampler2D fontAtlas;

out vec4 color;

void main(void)
{
	color = vec4(textColor.xyz, texture(fontAtlas, vs_textureCoordinate).w);
}
--FRAGMENT--