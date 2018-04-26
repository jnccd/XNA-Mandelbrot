#define Iterations 254
#define PI 3.14159265f
float4x4 MatrixTransform;
float timer;
float camX;
float camY;
float mouseWheel;
float mouseX;
float mouseY;

void SpriteVertexShader(inout float4 vColor : COLOR0, inout float2 texCoord : TEXCOORD0, inout float4 position : POSITION0)
{
	position = mul(position, MatrixTransform);
}

uniform extern texture ScreenTexture;
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 HSVtoRGB(float H, float S, float V)
{
	H = H % 360;
	if (S > 1)
		S = 1;
	if (S < 0)
		S = 0;
	if (V > 1)
		V = 1;
	if (V < 0)
		V = 0;

	//using wikipedia formulas https://de.wikipedia.org/wiki/HSV-Farbraum
	int hi = H / 60;
	float f = (H / 60 - hi);
	float p = V * (1 - S);
	float q = V * (1 - S * f);
	float t = V * (1 - S * (1 - f));

	if (hi == 0 || hi == 6)
		return float4(V, t, p, 1);
	else if (hi == 1)
		return float4(q, V, p, 1);
	else if (hi == 2)
		return float4(p, V, t, 1);
	else if (hi == 3)
		return float4(p, q, V, 1);
	else if (hi == 4)
		return float4(t, p, V, 1);
	else if (hi == 5)
		return float4(V, p, q, 1);
	else
		return float4(0, 0, 0, 0);
}

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{
	double cre = (2 * texCoord.x - 1) * mouseWheel - camX;
	double cim = ((2 * texCoord.y - 1) * mouseWheel - camY) * 9 / 16;

	float x = 0, y = 0;
	float count = 0;

	[loop] for (int i = 0; i < (mouseX + 0.5f) * 256; i++)
	{
		if (x*x + y*y < 1)
			count++;

		float xtemp = x*x - y*y + cre;
		y = 2 * x*y + cim;
		x = xtemp;
	}

	//count = sqrt(x * x + y * y);

	return HSVtoRGB((atan2(y, x) + PI) / (PI * 2) * 360, 1, (count * 10.0));
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}