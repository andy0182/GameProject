Shader "Mobile/SwordBoss" 
{
    Properties 
	{
		 _MainTex ("Base (RGB)", 2D) = "white" {}
		 _Color ("Main Color", Color) = (1,1,1,1)
	}
    SubShader
	{

		Tags {"RenderType"="Transparent" "Queue"="Transparent-549"} 
		//Tags { "RenderType"="Opaque" }
         Pass
		 {
		 	Blend SrcAlpha OneMinusSrcAlpha
//			ZTest LEqual
//			ZWrite Off
//			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
	
			sampler2D _MainTex;
			half4 _Color;
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			half4 _MainTex_ST;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 result = tex2D (_MainTex, i.uv)+_Color;
				result.a=_Color.a;
				return result;
			}
			ENDCG
		 }
    }
} 