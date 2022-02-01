Shader "Skybox/Copernic360"
{
	Properties
	{
		// The equirectangular skybox texture to be rendered and deformed.
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			// Texture to be deformed and rendered.
			sampler2D _MainTex;

			// Vertical position of the centre of the skybox.
			float _Height;
			// Apparent size of skybox
			float _Radius;
			// Apparent distance to floor
			float _FloorRadius;
			// Maximum allowed displacement from centre.
			float _MovementRange;
			// Azimuthal rotation of the scene
			float _RotationY;

			static const float PI = 3.14159265f;

			float3 limitPosition(float3 pos)
			{
				float maxRadius = _Radius * _MovementRange;

				float currentRadius = length(pos.xz);

				float startRadius = maxRadius * 0; // Change this multiplier to make the mapping linear up to that point.
				if (currentRadius > startRadius)
				{
					//float clampedRadius = (maxRadius - startRadius) * (float)Math.Tanh((currentRadius - startRadius) / (maxRadius - startRadius)) + startRadius;
					float clampedRadius = (currentRadius - startRadius) / (1 + (currentRadius - startRadius) / (maxRadius - startRadius)) + startRadius;
					float correctiveFactor = clampedRadius / currentRadius;
					pos.x *= correctiveFactor;
					pos.z *= correctiveFactor;
				}

				return pos;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = v.vertex.xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 v = i.worldPos;
				float3 d = limitPosition(_WorldSpaceCameraPos - float3(0, _Height, 0));

				// Solve for intersection with cylinder.
				float2 v2 = v.xz;
				float2 d2 = d.xz;
				float R = _Radius;
				float dv = dot(d2, v2);
				float vv = dot(v2, v2);
				float dd = dot(d2, d2);

				float alpha = (sqrt(vv * (R*R - dd) + dv*dv) - dv) / vv;

				// Construct correct ellipsoid based on whether we hit cylinder
				// above or below the xz-plane.
				float rx = _Radius;
				float ry = v.y * alpha + d.y > 0 ? _Radius : _FloorRadius;
				float rz = _Radius;
				float3 A = float3(1 / (rx*rx), 1 / (ry*ry), 1 / (rz*rz));

				// Solve for intersection with ellipsoid
				float dAv = dot(d, A*v);
				float vAv = dot(v, A*v);
				float dAd = dot(d, A*d);

				alpha = (sqrt(vAv * (1 - dAd) + dAv * dAv) - dAv) / vAv;

				// Convert back to world coordinates.
				float3 Xhit = v * alpha + d;
				float x = Xhit.x;
				float y = Xhit.y;
				float z = Xhit.z;
				float r = length(Xhit.xz);

				// Convert world position to spherical coordinates
				float theta = atan2(r, y) / PI; // In units of pi
				// For the texture lookup, we want phi to start at the negative
				// z-axis and increase in a clockwise sense.
				float phi = frac((atan2(-x, -z) - _RotationY) / (2 * PI)); // In units of 2*pi

				// Sample the texture
				fixed4 color = tex2D(_MainTex, float2(phi, 1 - theta));
				return color;
			}
			ENDCG
		}
	}
}
