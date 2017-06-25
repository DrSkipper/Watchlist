// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31724,y:32625|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-92-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34401,y:33638,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33306,y:33015,ptlb:Texture,ptin:_Texture,glob:False,tex:6c7d696cb9852cc439dbfb8afbf3789f;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-87-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:31973,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-72-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:32106,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-90-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:64,x:32788,y:32624|A-207-OUT,B-57-RGB,C-2-RGB,D-206-RGB,E-232-RGB;n:type:ShaderForge.SFN_Divide,id:65,x:32616,y:32641|A-64-OUT,B-66-OUT;n:type:ShaderForge.SFN_Vector1,id:66,x:32788,y:32767,v1:9;n:type:ShaderForge.SFN_TexCoord,id:71,x:34357,y:32235,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33489,y:31951,cmnt:plus plus|A-120-OUT,B-124-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34543,y:32899,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:256;n:type:ShaderForge.SFN_Relay,id:84,x:34313,y:33472|IN-256-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:34964,y:33046|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34378,y:32700,cmnt:Calculate Offset by Mip|A-194-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33490,y:32310,cmnt:minus|A-120-OUT,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33489,y:32092,cmnt:plus|A-120-OUT,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33490,y:32449,cmnt:minus minus|A-120-OUT,B-124-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33519,y:32235|IN-120-OUT;n:type:ShaderForge.SFN_Lerp,id:107,x:32128,y:32695|A-59-RGB,B-209-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:32862,y:33515|IN-256-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:33779,y:33490,min:0,max:1|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:33632,y:33287,cmnt:Mip Level|IN-126-OUT;n:type:ShaderForge.SFN_Slider,id:112,x:35855,y:32624,ptlb:Angle,ptin:_Angle,min:0,cur:0,max:360;n:type:ShaderForge.SFN_Relay,id:118,x:34222,y:32197,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:119,x:34222,y:32328,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Append,id:120,x:34068,y:32245|A-118-OUT,B-119-OUT;n:type:ShaderForge.SFN_Vector1,id:123,x:34357,y:32877,v1:2;n:type:ShaderForge.SFN_Multiply,id:124,x:34165,y:32793,cmnt:Double Offset|A-86-OUT,B-123-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:125,x:33657,y:33608,ptlb:Use Mips,ptin:_UseMips,on:True;n:type:ShaderForge.SFN_Lerp,id:126,x:33559,y:33377|A-127-OUT,B-109-OUT,T-125-OUT;n:type:ShaderForge.SFN_Vector1,id:127,x:33808,y:33377,v1:0;n:type:ShaderForge.SFN_Vector1,id:154,x:35866,y:32707,v1:0;n:type:ShaderForge.SFN_Vector1,id:156,x:35866,y:32778,v1:360;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:173,x:35587,y:32849|IN-112-OUT,IMIN-154-OUT,IMAX-156-OUT,OMIN-175-OUT,OMAX-174-OUT;n:type:ShaderForge.SFN_Pi,id:174,x:35882,y:32993;n:type:ShaderForge.SFN_Negate,id:175,x:35866,y:32839|IN-174-OUT;n:type:ShaderForge.SFN_Sin,id:176,x:35369,y:32759|IN-173-OUT;n:type:ShaderForge.SFN_Cos,id:177,x:35369,y:32901|IN-173-OUT;n:type:ShaderForge.SFN_Append,id:178,x:35193,y:32821|A-176-OUT,B-177-OUT;n:type:ShaderForge.SFN_Relay,id:179,x:35011,y:32821,cmnt:Offset Direction|IN-178-OUT;n:type:ShaderForge.SFN_Multiply,id:194,x:34772,y:32855|A-179-OUT,B-85-OUT;n:type:ShaderForge.SFN_Subtract,id:196,x:33490,y:32594,cmnt:minus minus minus|A-120-OUT,B-200-OUT;n:type:ShaderForge.SFN_Add,id:198,x:33489,y:31807,cmnt:plus plus plus|A-120-OUT,B-200-OUT;n:type:ShaderForge.SFN_Multiply,id:200,x:33995,y:32887,cmnt:Tripple Ofsset|A-86-OUT,B-202-OUT;n:type:ShaderForge.SFN_Vector1,id:202,x:34338,y:32969,v1:3;n:type:ShaderForge.SFN_Tex2d,id:204,x:33111,y:31829,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-198-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:206,x:33111,y:32622,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-196-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:207,x:32788,y:32491|A-230-RGB,B-204-RGB,C-61-RGB,D-63-RGB,E-59-RGB;n:type:ShaderForge.SFN_Lerp,id:209,x:32144,y:32402|A-65-OUT,B-223-OUT,T-253-OUT;n:type:ShaderForge.SFN_Relay,id:210,x:32836,y:31917|IN-59-RGB;n:type:ShaderForge.SFN_Add,id:211,x:32806,y:31638|A-63-RGB,B-57-RGB;n:type:ShaderForge.SFN_Add,id:212,x:32806,y:31267,cmnt:ppp|A-204-RGB,B-204-RGB;n:type:ShaderForge.SFN_Add,id:213,x:32786,y:31055,cmnt:pppp|A-230-RGB,B-232-RGB;n:type:ShaderForge.SFN_Add,id:218,x:32321,y:31358|A-213-OUT,B-248-OUT,C-241-OUT,D-244-OUT,E-220-OUT;n:type:ShaderForge.SFN_Multiply,id:220,x:32643,y:31905|A-210-OUT,B-222-OUT;n:type:ShaderForge.SFN_Vector1,id:222,x:32643,y:32040,v1:32;n:type:ShaderForge.SFN_Divide,id:223,x:31947,y:31852|A-218-OUT,B-225-OUT;n:type:ShaderForge.SFN_Vector1,id:225,x:32155,y:32001,v1:64;n:type:ShaderForge.SFN_Add,id:228,x:32806,y:31457|A-61-RGB,B-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:230,x:33111,y:31727,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-236-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:232,x:33111,y:32729,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-234-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Subtract,id:234,x:33490,y:32744,cmnt:minus minus minus minus|A-120-OUT,B-238-OUT;n:type:ShaderForge.SFN_Add,id:236,x:33489,y:31665,cmnt:plus plus plus plus|A-120-OUT,B-238-OUT;n:type:ShaderForge.SFN_Multiply,id:238,x:33844,y:33006,cmnt:Quad Offset|A-86-OUT,B-240-OUT;n:type:ShaderForge.SFN_Vector1,id:240,x:34325,y:33086,v1:4;n:type:ShaderForge.SFN_Multiply,id:241,x:32634,y:31510|A-228-OUT,B-242-OUT;n:type:ShaderForge.SFN_Vector1,id:242,x:32643,y:31638,v1:4;n:type:ShaderForge.SFN_Multiply,id:244,x:32643,y:31700|A-211-OUT,B-246-OUT;n:type:ShaderForge.SFN_Vector1,id:246,x:32643,y:31832,v1:8;n:type:ShaderForge.SFN_Multiply,id:248,x:32634,y:31334|A-212-OUT,B-250-OUT;n:type:ShaderForge.SFN_Vector1,id:250,x:32634,y:31457,v1:2;n:type:ShaderForge.SFN_Slider,id:253,x:32128,y:32546,ptlb:Bias,ptin:_Bias,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:256,x:34424,y:33456,frmn:0,frmx:1,tomn:0,tomx:8|IN-3-OUT;proporder:13-3-75-125-112-253;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Directional Blur Medium" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        _TextureSize ("Texture Size", Float ) = 256
        [MaterialToggle] _UseMips ("Use Mips", Float ) = 0
        _Angle ("Angle", Range(0, 360)) = 0
        _Bias ("Bias", Range(0, 1)) = 0
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
            uniform float _Angle;
            uniform fixed _UseMips;
            uniform float _Bias;
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
                float2 node_120 = float2(node_71.r,node_71.g);
                float2 node_94 = node_120;
                float node_256 = (_Blur*8.0+0.0);
                float node_84 = node_256;
                float node_110 = lerp(0.0,clamp(node_84,0,1),_UseMips); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_154 = 0.0;
                float node_174 = 3.141592654;
                float node_175 = (-1*node_174);
                float node_173 = (node_175 + ( (_Angle - node_154) * (node_174 - node_175) ) / (360.0 - node_154));
                float2 node_86 = ((float2(sin(node_173),cos(node_173))*node_84)/_TextureSize); // Calculate Offset by Mip
                float2 node_238 = (node_86*4.0); // Quad Offset
                float2 node_236 = (node_120+node_238); // plus plus plus plus
                float4 node_230 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_236, _Texture),0.0,node_110));
                float2 node_200 = (node_86*3.0); // Tripple Ofsset
                float2 node_198 = (node_120+node_200); // plus plus plus
                float4 node_204 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_198, _Texture),0.0,node_110));
                float2 node_124 = (node_86*2.0); // Double Offset
                float2 node_72 = (node_120+node_124); // plus plus
                float4 node_61 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_72, _Texture),0.0,node_110));
                float2 node_90 = (node_120+node_86); // plus
                float4 node_63 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_90, _Texture),0.0,node_110));
                float2 node_87 = (node_120-node_86); // minus
                float4 node_57 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_87, _Texture),0.0,node_110));
                float2 node_92 = (node_120-node_124); // minus minus
                float4 node_2 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_92, _Texture),0.0,node_110));
                float2 node_196 = (node_120-node_200); // minus minus minus
                float2 node_234 = (node_120-node_238); // minus minus minus minus
                float4 node_232 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_234, _Texture),0.0,node_110));
                float3 emissive = lerp(node_59.rgb,lerp((((node_230.rgb+node_204.rgb+node_61.rgb+node_63.rgb+node_59.rgb)+node_57.rgb+node_2.rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_196, _Texture),0.0,node_110)).rgb+node_232.rgb)/9.0),(((node_230.rgb+node_232.rgb)+((node_204.rgb+node_204.rgb)*2.0)+((node_61.rgb+node_2.rgb)*4.0)+((node_63.rgb+node_57.rgb)*8.0)+(node_59.rgb*32.0))/64.0),_Bias),saturate(node_256));
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
