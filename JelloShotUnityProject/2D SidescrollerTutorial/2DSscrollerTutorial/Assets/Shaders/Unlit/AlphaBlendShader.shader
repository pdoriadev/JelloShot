// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AlphaBlendShader/Unlit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Blend DstColor SrcColor

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			// struct for vertex data
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// struct for what we want from the vertex data
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			/// Takes appdata struct as parameter "v" and returns a v2f. All it looks at on mesh is position of
			/// vertex. Mesh is local coord. system. Initializes v2f called "o". Sets o's vertex variable using 
			/// special function "UnityObjectToClipPos()".
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			half4 _Color;

			// takes v2f struct and returns a color in form of a float4. Takes potential pixels and turns them 
			// into colors on the screen. Can't change position. That was locked in by vertex function. 
			// Colors for float4 are red, green, blue, alpha. 
			float4 frag(v2f i) : SV_Target
			{
				half4 color = tex2D(_MainTex, i.uv * 2);

				// Color with UVs
			    _Color *= half4(i.uv.r, i.uv.g, i.uv.r, 2);

				// Color Tint Code
				color += _Color;
			
				return color;
			}
			ENDCG
		}
	}
}