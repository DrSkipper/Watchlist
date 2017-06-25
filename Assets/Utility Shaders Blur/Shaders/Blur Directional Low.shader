// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31778,y:32737|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-92-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34431,y:33549,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33314,y:32819,ptlb:Texture,ptin:_Texture,glob:False,tex:6c7d696cb9852cc439dbfb8afbf3789f;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-87-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:32121,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-72-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:31999,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-90-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:64,x:32860,y:32235|A-63-RGB,B-61-RGB,C-59-RGB,D-57-RGB,E-2-RGB;n:type:ShaderForge.SFN_Divide,id:65,x:32691,y:32296|A-64-OUT,B-66-OUT;n:type:ShaderForge.SFN_Vector1,id:66,x:32860,y:32378,v1:5;n:type:ShaderForge.SFN_TexCoord,id:71,x:34357,y:32235,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33490,y:32109,cmnt:plus plus|A-120-OUT,B-124-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34543,y:32899,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:128;n:type:ShaderForge.SFN_Relay,id:84,x:34082,y:33360|IN-195-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:34964,y:33046|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34378,y:32700,cmnt:Calculate Offset by Mip|A-194-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33490,y:32310,cmnt:minus|A-120-OUT,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33484,y:31980,cmnt:plus|A-120-OUT,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33490,y:32449,cmnt:minus minus|A-120-OUT,B-124-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33519,y:32235|IN-120-OUT;n:type:ShaderForge.SFN_Lerp,id:107,x:32536,y:32553|A-59-RGB,B-65-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:32862,y:33515|IN-195-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:33786,y:33264,min:0,max:2|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:33639,y:33061,cmnt:Mip Level|IN-126-OUT;n:type:ShaderForge.SFN_Slider,id:112,x:35855,y:32624,ptlb:Angle,ptin:_Angle,min:0,cur:0,max:360;n:type:ShaderForge.SFN_Relay,id:118,x:34222,y:32197,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:119,x:34222,y:32328,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Append,id:120,x:34068,y:32245|A-118-OUT,B-119-OUT;n:type:ShaderForge.SFN_Vector1,id:123,x:34338,y:32869,v1:2;n:type:ShaderForge.SFN_Multiply,id:124,x:34180,y:32813,cmnt:Double Offset|A-86-OUT,B-123-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:125,x:33664,y:33382,ptlb:Use Mips,ptin:_UseMips,on:False;n:type:ShaderForge.SFN_Lerp,id:126,x:33566,y:33151|A-127-OUT,B-109-OUT,T-125-OUT;n:type:ShaderForge.SFN_Vector1,id:127,x:33815,y:33151,v1:0;n:type:ShaderForge.SFN_Vector1,id:154,x:35866,y:32707,v1:0;n:type:ShaderForge.SFN_Vector1,id:156,x:35866,y:32778,v1:360;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:173,x:35587,y:32849|IN-112-OUT,IMIN-154-OUT,IMAX-156-OUT,OMIN-175-OUT,OMAX-174-OUT;n:type:ShaderForge.SFN_Pi,id:174,x:35882,y:32993;n:type:ShaderForge.SFN_Negate,id:175,x:35866,y:32839|IN-174-OUT;n:type:ShaderForge.SFN_Sin,id:176,x:35369,y:32759|IN-173-OUT;n:type:ShaderForge.SFN_Cos,id:177,x:35369,y:32901|IN-173-OUT;n:type:ShaderForge.SFN_Append,id:178,x:35193,y:32821|A-176-OUT,B-177-OUT;n:type:ShaderForge.SFN_Relay,id:179,x:35011,y:32821,cmnt:Offset Direction|IN-178-OUT;n:type:ShaderForge.SFN_Multiply,id:194,x:34772,y:32855|A-179-OUT,B-85-OUT;n:type:ShaderForge.SFN_RemapRange,id:195,x:34186,y:33535,frmn:0,frmx:1,tomn:0,tomx:16|IN-3-OUT;proporder:13-3-75-125-112;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Directional Blur Low" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        _TextureSize ("Texture Size", Float ) = 128
        [MaterialToggle] _UseMips ("Use Mips", Float ) = 0
        _Angle ("Angle", Range(0, 360)) = 0
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
                float node_195 = (_Blur*16.0+0.0);
                float node_84 = node_195;
                float node_110 = lerp(0.0,clamp(node_84,0,2),_UseMips); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_154 = 0.0;
                float node_174 = 3.141592654;
                float node_175 = (-1*node_174);
                float node_173 = (node_175 + ( (_Angle - node_154) * (node_174 - node_175) ) / (360.0 - node_154));
                float2 node_86 = ((float2(sin(node_173),cos(node_173))*node_84)/_TextureSize); // Calculate Offset by Mip
                float2 node_90 = (node_120+node_86); // plus
                float2 node_124 = (node_86*2.0); // Double Offset
                float2 node_72 = (node_120+node_124); // plus plus
                float2 node_87 = (node_120-node_86); // minus
                float2 node_92 = (node_120-node_124); // minus minus
                float3 emissive = lerp(node_59.rgb,((tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_90, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_72, _Texture),0.0,node_110)).rgb+node_59.rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_87, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_92, _Texture),0.0,node_110)).rgb)/5.0),saturate(node_195));
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
