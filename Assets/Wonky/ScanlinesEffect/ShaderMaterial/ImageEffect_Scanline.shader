// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ImageEffect_Scanline"
{
	Properties
	{
		_Color ("Main Color", color) = (0.1,1,0,1)
		_MainTex ("Texture", 2D) = "white" {}
		_ScanTex ("Scanline Texture", 2D) = "white" {}
		_distSlider ("Distort Slider", float) = 0
		_Distortion1 ("Distortion1", float) = 1
		_Distortion2 ("Distortion2", float) = 1
		_WaveSpeed ("Wave Speed", float) = 1
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _ScanTex;
			fixed _distSlider;
			fixed _Distortion1;
			fixed _Distortion2;
			fixed _WaveSpeed;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 uv = i.uv;

				fixed distW = _distSlider * 0.01;
				fixed4 scan = tex2D(_ScanTex, uv+fixed2(0,_Time.z));
				fixed3 distUV = fixed3(scan.r*2-1, sin(uv.y*40.0-_Time.w*10*_WaveSpeed)+sin(uv.y*15.72-_Time.w*100*_WaveSpeed)*0.5*10, 0);
				fixed4 src = tex2D(_MainTex, uv + distUV.rb*distW*_Distortion1 + distUV.gb*distW*0.3*_Distortion2);

				fixed lerpW = saturate(abs(_distSlider)*0.3);

				return src * lerp(1,scan,lerpW*2) * lerp(1,_Color, lerpW);
			}
			ENDCG
		}
	}
}
