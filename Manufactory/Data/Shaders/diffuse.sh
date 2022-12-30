--VERTEX--
#version 440 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 textureCoordinate;

out vec2 vs_textureCoordinate;
out vec4 vs_color;

layout (location = 12) uniform vec4 color;

layout (location = 20) uniform mat4 projection;
layout (location = 21) uniform mat4 view;
layout (location = 22) uniform mat4 model;

void main(void)
{
	gl_Position = projection * view * model * vec4(position, 1.0f);
	vs_textureCoordinate = textureCoordinate;
	vs_color = color;
}
--VERTEX--

--FRAGMENT--
#version 450 core

in vec2 vs_textureCoordinate;
in vec4 vs_color;
uniform sampler2D textureObject;
out vec4 color;

void main(void)
{
	//color = texture2D(textureObject, vs_textureCoordinate.st);
	//color = color * vs_color;
	color = vs_color;
}
--FRAGMENT--