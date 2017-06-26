// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31736,y:32361|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-500-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:35431,y:34490,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33392,y:33219,ptlb:Texture,ptin:_Texture,glob:False,tex:6c7d696cb9852cc439dbfb8afbf3789f;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-510-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:31973,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-512-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:32106,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-487-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:64,x:32541,y:32340|A-207-OUT,B-206-RGB,C-232-RGB,D-256-RGB,E-262-RGB;n:type:ShaderForge.SFN_Divide,id:65,x:32235,y:32209|A-64-OUT,B-66-OUT;n:type:ShaderForge.SFN_Vector1,id:66,x:32251,y:32342,v1:13;n:type:ShaderForge.SFN_TexCoord,id:71,x:34373,y:32225,uv:0;n:type:ShaderForge.SFN_Relay,id:84,x:35206,y:34262|IN-3-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33519,y:32235|IN-71-UVOUT;n:type:ShaderForge.SFN_Lerp,id:107,x:32213,y:32677|A-59-RGB,B-65-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:34812,y:34487|IN-662-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:34502,y:34085,cmnt:Mip Level|IN-670-OUT;n:type:ShaderForge.SFN_Vector1,id:123,x:34956,y:33097,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:124,x:34704,y:32902,cmnt:0.01 offset|A-480-OUT,B-664-OUT;n:type:ShaderForge.SFN_Multiply,id:200,x:34599,y:33035,cmnt:0.02 Offset|A-480-OUT,B-123-OUT;n:type:ShaderForge.SFN_Vector1,id:202,x:34890,y:33294,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:204,x:33111,y:31829,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-514-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:206,x:33111,y:32622,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-502-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:207,x:32541,y:32207|A-284-OUT,B-63-RGB,C-59-RGB,D-57-RGB,E-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:230,x:33111,y:31727,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-516-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:232,x:33111,y:32729,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-504-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Multiply,id:238,x:34446,y:33140,cmnt:0.03 Offset|A-480-OUT,B-666-OUT;n:type:ShaderForge.SFN_Vector1,id:240,x:34830,y:33585,v1:0.3;n:type:ShaderForge.SFN_Tex2d,id:256,x:33111,y:32851,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-506-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:258,x:33111,y:31600,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-518-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:260,x:33111,y:31497,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-520-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:262,x:33111,y:32959,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-508-UVOUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Multiply,id:272,x:34342,y:33278,cmnt:0.05 Offset|A-480-OUT,B-202-OUT;n:type:ShaderForge.SFN_Multiply,id:274,x:34289,y:33428,cmnt:0.08 Offset|A-480-OUT,B-668-OUT;n:type:ShaderForge.SFN_Add,id:284,x:32541,y:32071|A-260-RGB,B-258-RGB,C-230-RGB,D-204-RGB,E-61-RGB;n:type:ShaderForge.SFN_Relay,id:373,x:34639,y:32825,cmnt:0.01 offset|IN-124-OUT;n:type:ShaderForge.SFN_Relay,id:409,x:34475,y:32963|IN-200-OUT;n:type:ShaderForge.SFN_Relay,id:429,x:34356,y:33054|IN-238-OUT;n:type:ShaderForge.SFN_Relay,id:431,x:34249,y:33213|IN-272-OUT;n:type:ShaderForge.SFN_Relay,id:433,x:34144,y:33376|IN-274-OUT;n:type:ShaderForge.SFN_Multiply,id:472,x:34210,y:33568,cmnt:0.08 Offset|A-480-OUT,B-240-OUT;n:type:ShaderForge.SFN_Relay,id:474,x:34030,y:33528|IN-472-OUT;n:type:ShaderForge.SFN_Relay,id:480,x:35102,y:32794|IN-662-OUT;n:type:ShaderForge.SFN_Rotator,id:487,x:33557,y:32098|UVIN-71-UVOUT,ANG-488-OUT;n:type:ShaderForge.SFN_Negate,id:488,x:34609,y:32684|IN-373-OUT;n:type:ShaderForge.SFN_Negate,id:490,x:34445,y:32842|IN-409-OUT;n:type:ShaderForge.SFN_Negate,id:492,x:34291,y:32942|IN-429-OUT;n:type:ShaderForge.SFN_Negate,id:494,x:34181,y:33086|IN-431-OUT;n:type:ShaderForge.SFN_Negate,id:496,x:34080,y:33253|IN-433-OUT;n:type:ShaderForge.SFN_Negate,id:498,x:33985,y:33404|IN-474-OUT;n:type:ShaderForge.SFN_Rotator,id:500,x:33545,y:32451|UVIN-71-UVOUT,ANG-409-OUT;n:type:ShaderForge.SFN_Rotator,id:502,x:33562,y:32620|UVIN-71-UVOUT,ANG-429-OUT;n:type:ShaderForge.SFN_Rotator,id:504,x:33562,y:32769|UVIN-71-UVOUT,ANG-431-OUT;n:type:ShaderForge.SFN_Rotator,id:506,x:33562,y:32912|UVIN-71-UVOUT,ANG-433-OUT;n:type:ShaderForge.SFN_Rotator,id:508,x:33562,y:33065|UVIN-71-UVOUT,ANG-474-OUT;n:type:ShaderForge.SFN_Rotator,id:510,x:33545,y:32316|UVIN-71-UVOUT,ANG-373-OUT;n:type:ShaderForge.SFN_Rotator,id:512,x:33557,y:31934|UVIN-71-UVOUT,ANG-490-OUT;n:type:ShaderForge.SFN_Rotator,id:514,x:33557,y:31787|UVIN-71-UVOUT,ANG-492-OUT;n:type:ShaderForge.SFN_Rotator,id:516,x:33568,y:31640|UVIN-71-UVOUT,ANG-494-OUT;n:type:ShaderForge.SFN_Rotator,id:518,x:33568,y:31488|UVIN-71-UVOUT,ANG-496-OUT;n:type:ShaderForge.SFN_Rotator,id:520,x:33568,y:31351|UVIN-71-UVOUT,ANG-498-OUT;n:type:ShaderForge.SFN_Vector1,id:660,x:35600,y:34006,v1:6.283185;n:type:ShaderForge.SFN_Multiply,id:662,x:35231,y:33947|A-660-OUT,B-84-OUT;n:type:ShaderForge.SFN_Vector1,id:664,x:34991,y:33000,v1:0.05;n:type:ShaderForge.SFN_Vector1,id:666,x:34933,y:33170,v1:0.15;n:type:ShaderForge.SFN_Vector1,id:668,x:34830,y:33438,v1:0.25;n:type:ShaderForge.SFN_ConstantClamp,id:670,x:34729,y:33975,min:0,max:4|IN-662-OUT;proporder:13-3;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Radial Blur" {
    Properties {
        _Texture ("Texture", 2D) = "" {}
        _Blur ("Blur", Range(0, 1)) = 1
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
                float node_662 = (6.283185*_Blur);
                float node_110 = clamp(node_662,0,4); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_480 = node_662;
                float node_474 = (node_480*0.3);
                float node_520_ang = (-1*node_474);
                float node_520_spd = 1.0;
                float node_520_cos = cos(node_520_spd*node_520_ang);
                float node_520_sin = sin(node_520_spd*node_520_ang);
                float2 node_520_piv = float2(0.5,0.5);
                float2 node_520 = (mul(node_71.rg-node_520_piv,float2x2( node_520_cos, -node_520_sin, node_520_sin, node_520_cos))+node_520_piv);
                float node_433 = (node_480*0.25);
                float node_518_ang = (-1*node_433);
                float node_518_spd = 1.0;
                float node_518_cos = cos(node_518_spd*node_518_ang);
                float node_518_sin = sin(node_518_spd*node_518_ang);
                float2 node_518_piv = float2(0.5,0.5);
                float2 node_518 = (mul(node_71.rg-node_518_piv,float2x2( node_518_cos, -node_518_sin, node_518_sin, node_518_cos))+node_518_piv);
                float node_431 = (node_480*0.2);
                float node_516_ang = (-1*node_431);
                float node_516_spd = 1.0;
                float node_516_cos = cos(node_516_spd*node_516_ang);
                float node_516_sin = sin(node_516_spd*node_516_ang);
                float2 node_516_piv = float2(0.5,0.5);
                float2 node_516 = (mul(node_71.rg-node_516_piv,float2x2( node_516_cos, -node_516_sin, node_516_sin, node_516_cos))+node_516_piv);
                float node_429 = (node_480*0.15);
                float node_514_ang = (-1*node_429);
                float node_514_spd = 1.0;
                float node_514_cos = cos(node_514_spd*node_514_ang);
                float node_514_sin = sin(node_514_spd*node_514_ang);
                float2 node_514_piv = float2(0.5,0.5);
                float2 node_514 = (mul(node_71.rg-node_514_piv,float2x2( node_514_cos, -node_514_sin, node_514_sin, node_514_cos))+node_514_piv);
                float node_409 = (node_480*0.1);
                float node_512_ang = (-1*node_409);
                float node_512_spd = 1.0;
                float node_512_cos = cos(node_512_spd*node_512_ang);
                float node_512_sin = sin(node_512_spd*node_512_ang);
                float2 node_512_piv = float2(0.5,0.5);
                float2 node_512 = (mul(node_71.rg-node_512_piv,float2x2( node_512_cos, -node_512_sin, node_512_sin, node_512_cos))+node_512_piv);
                float node_373 = (node_480*0.05); // 0.01 offset
                float node_487_ang = (-1*node_373);
                float node_487_spd = 1.0;
                float node_487_cos = cos(node_487_spd*node_487_ang);
                float node_487_sin = sin(node_487_spd*node_487_ang);
                float2 node_487_piv = float2(0.5,0.5);
                float2 node_487 = (mul(node_71.rg-node_487_piv,float2x2( node_487_cos, -node_487_sin, node_487_sin, node_487_cos))+node_487_piv);
                float node_510_ang = node_373;
                float node_510_spd = 1.0;
                float node_510_cos = cos(node_510_spd*node_510_ang);
                float node_510_sin = sin(node_510_spd*node_510_ang);
                float2 node_510_piv = float2(0.5,0.5);
                float2 node_510 = (mul(node_71.rg-node_510_piv,float2x2( node_510_cos, -node_510_sin, node_510_sin, node_510_cos))+node_510_piv);
                float node_500_ang = node_409;
                float node_500_spd = 1.0;
                float node_500_cos = cos(node_500_spd*node_500_ang);
                float node_500_sin = sin(node_500_spd*node_500_ang);
                float2 node_500_piv = float2(0.5,0.5);
                float2 node_500 = (mul(node_71.rg-node_500_piv,float2x2( node_500_cos, -node_500_sin, node_500_sin, node_500_cos))+node_500_piv);
                float node_502_ang = node_429;
                float node_502_spd = 1.0;
                float node_502_cos = cos(node_502_spd*node_502_ang);
                float node_502_sin = sin(node_502_spd*node_502_ang);
                float2 node_502_piv = float2(0.5,0.5);
                float2 node_502 = (mul(node_71.rg-node_502_piv,float2x2( node_502_cos, -node_502_sin, node_502_sin, node_502_cos))+node_502_piv);
                float node_504_ang = node_431;
                float node_504_spd = 1.0;
                float node_504_cos = cos(node_504_spd*node_504_ang);
                float node_504_sin = sin(node_504_spd*node_504_ang);
                float2 node_504_piv = float2(0.5,0.5);
                float2 node_504 = (mul(node_71.rg-node_504_piv,float2x2( node_504_cos, -node_504_sin, node_504_sin, node_504_cos))+node_504_piv);
                float node_506_ang = node_433;
                float node_506_spd = 1.0;
                float node_506_cos = cos(node_506_spd*node_506_ang);
                float node_506_sin = sin(node_506_spd*node_506_ang);
                float2 node_506_piv = float2(0.5,0.5);
                float2 node_506 = (mul(node_71.rg-node_506_piv,float2x2( node_506_cos, -node_506_sin, node_506_sin, node_506_cos))+node_506_piv);
                float node_508_ang = node_474;
                float node_508_spd = 1.0;
                float node_508_cos = cos(node_508_spd*node_508_ang);
                float node_508_sin = sin(node_508_spd*node_508_ang);
                float2 node_508_piv = float2(0.5,0.5);
                float2 node_508 = (mul(node_71.rg-node_508_piv,float2x2( node_508_cos, -node_508_sin, node_508_sin, node_508_cos))+node_508_piv);
                float3 emissive = lerp(node_59.rgb,((((tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_520, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_518, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_516, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_514, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_512, _Texture),0.0,node_110)).rgb)+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_487, _Texture),0.0,node_110)).rgb+node_59.rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_510, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_500, _Texture),0.0,node_110)).rgb)+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_502, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_504, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_506, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_508, _Texture),0.0,node_110)).rgb)/13.0),saturate(node_662));
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
