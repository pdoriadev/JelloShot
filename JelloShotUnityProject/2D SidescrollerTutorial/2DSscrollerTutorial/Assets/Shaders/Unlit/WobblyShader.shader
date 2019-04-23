Shader "Unlit/WobblyShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DispTexture("Texture", 2D) = "white" {}
		_Magnitude("Range", Range(0, 0.1)) = 1
		_Color("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Cull Off
		/*Tags { "RenderType"="Opaque" }
		LOD 100*/

		Pass
		{
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
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _DispTexture;
			float _Magnitude;
			float4 _Color;
	/*		float4 _MainTex_TexelSize;*/

			fixed4 frag (v2f i) : SV_Target
			{
				float2 distUv = (i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);

				float2 disp = tex2D(_DispTexture, distUv).xy;
				disp = ((disp * 2) - 1) * (_Magnitude);

				fixed4 col = tex2D(_MainTex, i.uv + disp);
				col *= _Color;
				return col;
			}
			ENDCG
		}
	}
}
