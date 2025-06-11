Shader "Unlit/Mine/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Colour", Color) = (1,1,1,1)
		_Value("OutlineAmt", Float) = 1
	}
	SubShader
	{
		Tags{"Queue" = "Transparent"}

		Pass
		{
			ZTest Off
			Cull Off
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vertFunction
			#pragma fragment fragmentFunction
			
			#include "UnityCG.cginc"


			sampler2D _MainTex;

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			
			v2f vertFunction (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			fixed4 _Color;
			float4 _MainTex_TexelSize;

			fixed4 fragmentFunction(v2f i) : COLOR
			{
				half4 c = tex2D(_MainTex, i.uv);
				c.rgb *= c.a;
				half4 outlineC = _Color;
				outlineC.a *= ceil(c.a);
				outlineC.rgb *= outlineC.a;

				fixed upA = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
				fixed downA = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
				fixed rightA = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
				fixed leftA = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

				return lerp(outlineC, c, ceil(upA * downA * rightA * leftA));
			}

			ENDCG
		}
	}
}
