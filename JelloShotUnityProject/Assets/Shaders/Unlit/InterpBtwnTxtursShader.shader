Shader "Unlit/InterpBtwnTxturs"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecondaryTex("Second Texture", 2D) = "white" {}
		_Blend("Blend", Range(0, 1)) = 0
		_Color("Color", Color) = (1, 1, 1, 1)
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
		    Blend DstColor SrcColor

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;
			float _Blend;
			half4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_TARGET
			{
				//interpolate between the textures
				fixed4 interpValue = lerp(tex2D(_MainTex, i.uv), tex2D(_SecondaryTex, i.uv), _Blend) * _Color;
				return interpValue;
			}

		/*	fixed4 frag (v2f i) : SV_Target
			{
				float4 color = (tex2D(_MainTex, i.uv) * _Tween;
				float4 secondColor = (tex2D(_SecondTex, i.uv) * _Tween);
				color *= secondColor;
				return color;
			}*/
			ENDCG
		}
	}
}
