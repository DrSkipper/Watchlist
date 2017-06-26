// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31599,y:32830|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,ntxv:0,isnm:False|UVIN-100-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34672,y:34146,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33433,y:33552,ptlb:Texture,ptin:_Texture,glob:False;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,ntxv:0,isnm:False|UVIN-98-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:32121,ntxv:0,isnm:False|UVIN-96-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:31999,ntxv:0,isnm:False|UVIN-93-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_TexCoord,id:71,x:34297,y:32240,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33886,y:32417,cmnt:V plus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Vector1,id:73,x:35006,y:32638,v1:2;n:type:ShaderForge.SFN_Power,id:74,x:34848,y:32722,cmnt:Pixel Offset|VAL-73-OUT,EXP-85-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34848,y:32916,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:128;n:type:ShaderForge.SFN_Relay,id:84,x:34446,y:33838|IN-238-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:35047,y:33068|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34558,y:32726,cmnt:Calculate Offset by Mip|A-74-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33875,y:32028,cmnt:U minus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33875,y:31894,cmnt:U plus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33886,y:32563,cmnt:V minus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Append,id:93,x:33446,y:31998|A-105-OUT,B-72-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33916,y:32238|IN-71-UVOUT;n:type:ShaderForge.SFN_Append,id:96,x:33446,y:32120|A-105-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:98,x:33446,y:32365|A-90-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:100,x:33446,y:32491|A-87-OUT,B-106-OUT;n:type:ShaderForge.SFN_Relay,id:105,x:33916,y:32174,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:106,x:33916,y:32332,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Lerp,id:107,x:31941,y:32964|A-59-RGB,B-176-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:32959,y:33699|IN-238-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:34306,y:33693,min:0,max:5|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:33946,y:33512,cmnt:Mip Level|IN-109-OUT;n:type:ShaderForge.SFN_Tex2d,id:112,x:33111,y:32633,ntxv:0,isnm:False|UVIN-124-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:114,x:33111,y:32787,ntxv:0,isnm:False|UVIN-126-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:116,x:33111,y:31876,ntxv:0,isnm:False|UVIN-120-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:118,x:33111,y:31745,ntxv:0,isnm:False|UVIN-122-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Append,id:120,x:33446,y:31867|A-87-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:122,x:33446,y:31734|A-90-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:124,x:33446,y:32624|A-90-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:126,x:33446,y:32748|A-87-OUT,B-92-OUT;n:type:ShaderForge.SFN_Add,id:128,x:32730,y:32062,cmnt:Near|A-63-RGB,B-61-RGB,C-57-RGB,D-2-RGB;n:type:ShaderForge.SFN_Add,id:130,x:32730,y:31816,cmnt:Mid Pixels|A-118-RGB,B-116-RGB,C-112-RGB,D-114-RGB;n:type:ShaderForge.SFN_Relay,id:131,x:32759,y:32290,cmnt:Center Pixels|IN-59-RGB;n:type:ShaderForge.SFN_Multiply,id:132,x:32399,y:31738|A-140-OUT,B-130-OUT;n:type:ShaderForge.SFN_Multiply,id:133,x:32399,y:31939|A-138-OUT,B-128-OUT;n:type:ShaderForge.SFN_Multiply,id:134,x:32399,y:32143|A-136-OUT,B-131-OUT;n:type:ShaderForge.SFN_Add,id:135,x:32175,y:31699|A-219-OUT,B-174-OUT,C-132-OUT,D-133-OUT,E-134-OUT;n:type:ShaderForge.SFN_Vector1,id:136,x:32399,y:32085,v1:0.4;n:type:ShaderForge.SFN_Vector1,id:138,x:32399,y:31884,v1:0.075;n:type:ShaderForge.SFN_Vector1,id:140,x:32399,y:31680,v1:0.05;n:type:ShaderForge.SFN_Add,id:142,x:32730,y:31605,cmnt:Far Pixels|A-150-RGB,B-148-RGB,C-152-RGB,D-154-RGB;n:type:ShaderForge.SFN_Multiply,id:143,x:34337,y:32798,cmnt:Double Offset|A-86-OUT,B-144-OUT;n:type:ShaderForge.SFN_Vector1,id:144,x:34558,y:32878,v1:1.5;n:type:ShaderForge.SFN_Tex2d,id:148,x:33111,y:31622,ntxv:0,isnm:False|UVIN-164-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:150,x:33111,y:31496,ntxv:0,isnm:False|UVIN-166-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:152,x:33111,y:32922,ntxv:0,isnm:False|UVIN-168-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:154,x:33111,y:33057,ntxv:0,isnm:False|UVIN-170-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Subtract,id:156,x:33874,y:31750,cmnt:U minus minus Offset|A-71-U,B-143-OUT;n:type:ShaderForge.SFN_Add,id:158,x:33874,y:31608,cmnt:U plus plus Offset|A-71-U,B-143-OUT;n:type:ShaderForge.SFN_Add,id:160,x:33886,y:32700,cmnt:V plus plus Offset|A-71-V,B-143-OUT;n:type:ShaderForge.SFN_Subtract,id:162,x:33886,y:32846,cmnt:V minus minus Offset|A-71-V,B-143-OUT;n:type:ShaderForge.SFN_Append,id:164,x:33446,y:31595|A-156-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:166,x:33446,y:31458|A-158-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:168,x:33446,y:32883|A-105-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:170,x:33446,y:33003|A-105-OUT,B-162-OUT;n:type:ShaderForge.SFN_Vector1,id:172,x:32399,y:31489,v1:0.025;n:type:ShaderForge.SFN_Multiply,id:174,x:32399,y:31540|A-172-OUT,B-142-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:175,x:32077,y:32675,ptlb:Bias,ptin:_Bias,on:False;n:type:ShaderForge.SFN_Lerp,id:176,x:32052,y:32495|A-182-OUT,B-135-OUT,T-175-OUT;n:type:ShaderForge.SFN_Add,id:177,x:32509,y:32351|A-185-OUT,B-142-OUT,C-130-OUT,D-128-OUT,E-131-OUT;n:type:ShaderForge.SFN_Divide,id:182,x:32348,y:32400|A-177-OUT,B-183-OUT;n:type:ShaderForge.SFN_Vector1,id:183,x:32509,y:32505,v1:17;n:type:ShaderForge.SFN_Add,id:185,x:32730,y:31400,cmnt:Furthest Pixels|A-189-RGB,B-187-RGB,C-195-RGB,D-197-RGB;n:type:ShaderForge.SFN_Tex2d,id:187,x:33111,y:31394,ntxv:0,isnm:False|UVIN-191-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:189,x:33111,y:31287,ntxv:0,isnm:False|UVIN-193-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Append,id:191,x:33446,y:31323|A-158-OUT,B-162-OUT;n:type:ShaderForge.SFN_Append,id:193,x:33446,y:31196|A-158-OUT,B-160-OUT;n:type:ShaderForge.SFN_Tex2d,id:195,x:33111,y:33185,ntxv:0,isnm:False|UVIN-234-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:197,x:33111,y:33326,ntxv:0,isnm:False|UVIN-236-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Vector1,id:217,x:32399,y:31292,v1:0.025;n:type:ShaderForge.SFN_Multiply,id:219,x:32399,y:31358|A-217-OUT,B-185-OUT;n:type:ShaderForge.SFN_Append,id:234,x:33446,y:33142|A-156-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:236,x:33446,y:33276|A-156-OUT,B-162-OUT;n:type:ShaderForge.SFN_RemapRange,id:238,x:34545,y:33912,frmn:0,frmx:1,tomn:0,tomx:6|IN-3-OUT;proporder:13-3-75-175;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Blur Ultra" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        _TextureSize ("Texture Size", Float ) = 128
        [MaterialToggle] _Bias ("Bias", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float _Blur;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _TextureSize;
            uniform fixed _Bias;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_71 = i.uv0;
                float2 node_94 = node_71.rg;
                float node_238 = (_Blur*6.0+0.0);
                float node_84 = node_238;
                float node_110 = clamp(node_84,0,5); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_86 = (pow(2.0,node_84)/_TextureSize); // Calculate Offset by Mip
                float node_143 = (node_86*1.5); // Double Offset
                float node_158 = (node_71.r+node_143); // U plus plus Offset
                float node_160 = (node_71.g+node_143); // V plus plus Offset
                float2 node_193 = float2(node_158,node_160);
                float node_162 = (node_71.g-node_143); // V minus minus Offset
                float2 node_191 = float2(node_158,node_162);
                float node_156 = (node_71.r-node_143); // U minus minus Offset
                float2 node_234 = float2(node_156,node_160);
                float2 node_236 = float2(node_156,node_162);
                float3 node_185 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_193, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_191, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_234, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_236, _Texture),0.0,node_110)).rgb); // Furthest Pixels
                float node_106 = node_71.g; // V
                float2 node_166 = float2(node_158,node_106);
                float2 node_164 = float2(node_156,node_106);
                float node_105 = node_71.r; // U
                float2 node_168 = float2(node_105,node_160);
                float2 node_170 = float2(node_105,node_162);
                float3 node_142 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_166, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_164, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_168, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_170, _Texture),0.0,node_110)).rgb); // Far Pixels
                float node_90 = (node_71.r+node_86); // U plus Offset
                float node_72 = (node_71.g+node_86); // V plus Offset
                float2 node_122 = float2(node_90,node_72);
                float node_87 = (node_71.r-node_86); // U minus Offset
                float2 node_120 = float2(node_87,node_72);
                float node_92 = (node_71.g-node_86); // V minus Offset
                float2 node_124 = float2(node_90,node_92);
                float2 node_126 = float2(node_87,node_92);
                float3 node_130 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_122, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_120, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_124, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_126, _Texture),0.0,node_110)).rgb); // Mid Pixels
                float2 node_93 = float2(node_105,node_72);
                float2 node_96 = float2(node_105,node_92);
                float2 node_98 = float2(node_90,node_106);
                float2 node_100 = float2(node_87,node_106);
                float3 node_128 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_93, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_96, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_98, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_100, _Texture),0.0,node_110)).rgb); // Near
                float3 node_131 = node_59.rgb; // Center Pixels
                float3 emissive = lerp(node_59.rgb,lerp(((node_185+node_142+node_130+node_128+node_131)/17.0),((0.025*node_185)+(0.025*node_142)+(0.05*node_130)+(0.075*node_128)+(0.4*node_131)),_Bias),saturate(node_238));
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
    CustomEditor "ShaderForgeMaterialInspector"
}
