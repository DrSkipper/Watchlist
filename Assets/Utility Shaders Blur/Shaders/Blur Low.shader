// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31778,y:32737|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,ntxv:0,isnm:False|UVIN-100-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34216,y:33415,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33296,y:32661,ptlb:Texture,ptin:_Texture,glob:False;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,ntxv:0,isnm:False|UVIN-98-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:32121,ntxv:0,isnm:False|UVIN-96-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:31999,ntxv:0,isnm:False|UVIN-93-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:64,x:32860,y:32235|A-63-RGB,B-61-RGB,C-59-RGB,D-57-RGB,E-2-RGB;n:type:ShaderForge.SFN_Divide,id:65,x:32691,y:32296|A-64-OUT,B-66-OUT;n:type:ShaderForge.SFN_Vector1,id:66,x:32860,y:32378,v1:5;n:type:ShaderForge.SFN_TexCoord,id:71,x:34297,y:32240,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33887,y:32313,cmnt:V plus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Vector1,id:73,x:34825,y:32619,v1:2;n:type:ShaderForge.SFN_Power,id:74,x:34667,y:32703,cmnt:Pixel Offset|VAL-73-OUT,EXP-85-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34667,y:32897,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:128;n:type:ShaderForge.SFN_Relay,id:84,x:34102,y:33124|IN-111-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:34866,y:33049|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34377,y:32707,cmnt:Calculate Offset by Mip|A-74-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33881,y:32124,cmnt:U minus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33881,y:31990,cmnt:U plus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33887,y:32459,cmnt:V minus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Append,id:93,x:33446,y:31998|A-105-OUT,B-72-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33916,y:32238|IN-71-UVOUT;n:type:ShaderForge.SFN_Append,id:96,x:33446,y:32120|A-105-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:98,x:33446,y:32365|A-90-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:100,x:33446,y:32491|A-87-OUT,B-106-OUT;n:type:ShaderForge.SFN_Relay,id:105,x:33898,y:31883,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:106,x:33908,y:32624,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Lerp,id:107,x:32536,y:32553|A-59-RGB,B-65-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:32915,y:32718|IN-111-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:33962,y:32979,min:0,max:5|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:33815,y:32776,cmnt:Mip Level|IN-109-OUT;n:type:ShaderForge.SFN_RemapRange,id:111,x:34202,y:33182,frmn:0,frmx:1,tomn:0,tomx:6|IN-3-OUT;proporder:13-3-75;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Blur Low" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        _TextureSize ("Texture Size", Float ) = 128
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
                float node_111 = (_Blur*6.0+0.0);
                float node_84 = node_111;
                float node_110 = clamp(node_84,0,5); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_105 = node_71.r; // U
                float node_86 = (pow(2.0,node_84)/_TextureSize); // Calculate Offset by Mip
                float2 node_93 = float2(node_105,(node_71.g+node_86));
                float2 node_96 = float2(node_105,(node_71.g-node_86));
                float node_106 = node_71.g; // V
                float2 node_98 = float2((node_71.r+node_86),node_106);
                float2 node_100 = float2((node_71.r-node_86),node_106);
                float3 emissive = lerp(node_59.rgb,((tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_93, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_96, _Texture),0.0,node_110)).rgb+node_59.rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_98, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_100, _Texture),0.0,node_110)).rgb)/5.0),saturate(node_111));
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
