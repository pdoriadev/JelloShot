// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BasicShader/Unlit"
{
	Properties
	{
		_Tween("Range", Range(0,1)) = 0
	}

	SubShader
	{

		Tags
		{
			"PreviewType" = "Plane"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			// struct for vertex data
			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD1;
			};

			// struct for what we want from the vertex data
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD1;
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

			float _Tween;

			// takes v2f struct and returns a color in form of a float4. Takes potential pixels and turns them 
			// into colors on the xcreen. Can't change position. That was locked in by vertex function. 
			// Colors for float4 are red, green, blue, alpha. 
			float4 frag(v2f i) : SV_Target
			{
				float4 color = float4(i.uv.r *1, i.uv.r *1, i.uv.r  * 2, 1 * _Tween);
				return color;

			}
			ENDCG
		}
	}
}