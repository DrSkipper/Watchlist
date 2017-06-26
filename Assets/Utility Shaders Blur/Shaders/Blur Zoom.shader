// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31724,y:32625|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-390-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:35271,y:34436,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33343,y:33292,ptlb:Texture,ptin:_Texture,glob:False,tex:6c7d696cb9852cc439dbfb8afbf3789f;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-364-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:31973,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-384-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:32106,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-308-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:64,x:32750,y:32780|A-207-OUT,B-206-RGB,C-232-RGB,D-256-RGB,E-262-RGB;n:type:ShaderForge.SFN_Divide,id:65,x:32578,y:32797|A-64-OUT,B-66-OUT;n:type:ShaderForge.SFN_Vector1,id:66,x:32750,y:32923,v1:13;n:type:ShaderForge.SFN_TexCoord,id:71,x:35509,y:32122,uv:0;n:type:ShaderForge.SFN_Relay,id:84,x:35183,y:34270|IN-487-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33519,y:32235|IN-120-OUT;n:type:ShaderForge.SFN_Lerp,id:107,x:32128,y:32695|A-59-RGB,B-209-OUT,T-108-OUT;n:type:ShaderForge.SFN_Clamp01,id:108,x:32862,y:33515|IN-84-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:34649,y:34288,min:0,max:1|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:34502,y:34085,cmnt:Mip Level|IN-126-OUT;n:type:ShaderForge.SFN_Relay,id:118,x:34901,y:32166,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:119,x:34901,y:32297,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Append,id:120,x:34321,y:32259|A-118-OUT,B-119-OUT;n:type:ShaderForge.SFN_Vector1,id:123,x:34912,y:33031,v1:0.01;n:type:ShaderForge.SFN_Multiply,id:124,x:34704,y:32902,cmnt:0.01 offset|A-480-OUT,B-123-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:125,x:34527,y:34406,ptlb:Use Mips,ptin:_UseMips,on:True;n:type:ShaderForge.SFN_Lerp,id:126,x:34429,y:34175|A-127-OUT,B-109-OUT,T-125-OUT;n:type:ShaderForge.SFN_Vector1,id:127,x:34678,y:34175,v1:0;n:type:ShaderForge.SFN_Multiply,id:200,x:34599,y:33035,cmnt:0.02 Offset|A-480-OUT,B-202-OUT;n:type:ShaderForge.SFN_Vector1,id:202,x:34893,y:33123,v1:0.02;n:type:ShaderForge.SFN_Tex2d,id:204,x:33111,y:31829,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-398-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:206,x:33111,y:32622,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-404-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:207,x:32750,y:32647|A-284-OUT,B-63-RGB,C-59-RGB,D-57-RGB,E-2-RGB;n:type:ShaderForge.SFN_Lerp,id:209,x:32144,y:32402|A-65-OUT,B-223-OUT,T-293-OUT;n:type:ShaderForge.SFN_Relay,id:210,x:32836,y:31917|IN-59-RGB;n:type:ShaderForge.SFN_Add,id:211,x:32806,y:31638|A-63-RGB,B-57-RGB;n:type:ShaderForge.SFN_Add,id:212,x:32806,y:31267,cmnt:ppp|A-204-RGB,B-204-RGB;n:type:ShaderForge.SFN_Add,id:213,x:32806,y:31093,cmnt:pppp|A-230-RGB,B-232-RGB;n:type:ShaderForge.SFN_Add,id:218,x:32321,y:31358|A-213-OUT,B-248-OUT,C-241-OUT,D-244-OUT,E-220-OUT;n:type:ShaderForge.SFN_Multiply,id:220,x:32643,y:31905|A-210-OUT,B-222-OUT;n:type:ShaderForge.SFN_Vector1,id:222,x:32643,y:32040,v1:32;n:type:ShaderForge.SFN_Divide,id:223,x:31947,y:31852|A-218-OUT,B-225-OUT;n:type:ShaderForge.SFN_Vector1,id:225,x:32155,y:32001,v1:64;n:type:ShaderForge.SFN_Add,id:228,x:32806,y:31457|A-61-RGB,B-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:230,x:33111,y:31727,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-435-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:232,x:33111,y:32729,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-441-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Multiply,id:238,x:34446,y:33140,cmnt:0.03 Offset|A-480-OUT,B-240-OUT;n:type:ShaderForge.SFN_Vector1,id:240,x:34880,y:33240,v1:0.03;n:type:ShaderForge.SFN_Multiply,id:241,x:32634,y:31510|A-228-OUT,B-242-OUT;n:type:ShaderForge.SFN_Vector1,id:242,x:32643,y:31638,v1:4;n:type:ShaderForge.SFN_Multiply,id:244,x:32643,y:31700|A-211-OUT,B-246-OUT;n:type:ShaderForge.SFN_Vector1,id:246,x:32643,y:31832,v1:8;n:type:ShaderForge.SFN_Multiply,id:248,x:32634,y:31334|A-212-OUT,B-250-OUT;n:type:ShaderForge.SFN_Vector1,id:250,x:32634,y:31457,v1:2;n:type:ShaderForge.SFN_Tex2d,id:256,x:33111,y:32851,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-423-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:258,x:33111,y:31600,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-417-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:260,x:33111,y:31497,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-466-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:262,x:33111,y:32959,tex:6c7d696cb9852cc439dbfb8afbf3789f,ntxv:0,isnm:False|UVIN-460-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Multiply,id:272,x:34342,y:33278,cmnt:0.05 Offset|A-480-OUT,B-276-OUT;n:type:ShaderForge.SFN_Multiply,id:274,x:34289,y:33428,cmnt:0.08 Offset|A-480-OUT,B-278-OUT;n:type:ShaderForge.SFN_Vector1,id:276,x:34868,y:33350,v1:0.04;n:type:ShaderForge.SFN_Vector1,id:278,x:34848,y:33476,v1:0.05;n:type:ShaderForge.SFN_Add,id:280,x:32806,y:30930,cmnt:ppppp|A-258-RGB,B-256-RGB;n:type:ShaderForge.SFN_Add,id:282,x:32806,y:30776,cmnt:pppppp|A-260-RGB,B-262-RGB;n:type:ShaderForge.SFN_Add,id:284,x:32750,y:32511|A-260-RGB,B-258-RGB,C-230-RGB,D-204-RGB,E-61-RGB;n:type:ShaderForge.SFN_Slider,id:293,x:32128,y:32530,ptlb:Bias,ptin:_Bias,min:0,cur:1,max:1;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:308,x:33487,y:32110|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-315-OUT,OMAX-317-OUT;n:type:ShaderForge.SFN_Subtract,id:315,x:33681,y:32080|A-344-OUT,B-373-OUT;n:type:ShaderForge.SFN_Add,id:317,x:33641,y:32110|A-342-OUT,B-373-OUT;n:type:ShaderForge.SFN_Vector1,id:342,x:34297,y:32120,v1:1;n:type:ShaderForge.SFN_Vector1,id:344,x:34297,y:32064,v1:0;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:364,x:33487,y:32296|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-372-OUT,OMAX-368-OUT;n:type:ShaderForge.SFN_Subtract,id:368,x:33655,y:32330|A-342-OUT,B-373-OUT;n:type:ShaderForge.SFN_Add,id:372,x:33671,y:32296|A-344-OUT,B-373-OUT;n:type:ShaderForge.SFN_Relay,id:373,x:34639,y:32825,cmnt:0.01 offset|IN-124-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:384,x:33476,y:31950|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-386-OUT,OMAX-388-OUT;n:type:ShaderForge.SFN_Subtract,id:386,x:33681,y:31937|A-344-OUT,B-409-OUT;n:type:ShaderForge.SFN_Add,id:388,x:33649,y:31950|A-342-OUT,B-409-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:390,x:33487,y:32451|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-394-OUT,OMAX-392-OUT;n:type:ShaderForge.SFN_Subtract,id:392,x:33671,y:32485|A-342-OUT,B-409-OUT;n:type:ShaderForge.SFN_Add,id:394,x:33671,y:32451|A-344-OUT,B-409-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:398,x:33442,y:31798|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-400-OUT,OMAX-402-OUT;n:type:ShaderForge.SFN_Subtract,id:400,x:33649,y:31771|A-344-OUT,B-429-OUT;n:type:ShaderForge.SFN_Add,id:402,x:33632,y:31798|A-342-OUT,B-429-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:404,x:33474,y:32619|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-408-OUT,OMAX-406-OUT;n:type:ShaderForge.SFN_Subtract,id:406,x:33653,y:32663|A-342-OUT,B-429-OUT;n:type:ShaderForge.SFN_Add,id:408,x:33671,y:32619|A-344-OUT,B-429-OUT;n:type:ShaderForge.SFN_Relay,id:409,x:34475,y:32963|IN-200-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:417,x:33442,y:31467|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-419-OUT,OMAX-421-OUT;n:type:ShaderForge.SFN_Subtract,id:419,x:33648,y:31441|A-344-OUT,B-433-OUT;n:type:ShaderForge.SFN_Add,id:421,x:33648,y:31480|A-342-OUT,B-433-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:423,x:33474,y:32966|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-427-OUT,OMAX-425-OUT;n:type:ShaderForge.SFN_Subtract,id:425,x:33648,y:33003|A-342-OUT,B-433-OUT;n:type:ShaderForge.SFN_Add,id:427,x:33671,y:32966|A-344-OUT,B-433-OUT;n:type:ShaderForge.SFN_Relay,id:429,x:34356,y:33054|IN-238-OUT;n:type:ShaderForge.SFN_Relay,id:431,x:34249,y:33213|IN-272-OUT;n:type:ShaderForge.SFN_Relay,id:433,x:34170,y:33385|IN-274-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:435,x:33442,y:31636|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-437-OUT,OMAX-439-OUT;n:type:ShaderForge.SFN_Subtract,id:437,x:33659,y:31620|A-344-OUT,B-431-OUT;n:type:ShaderForge.SFN_Add,id:439,x:33632,y:31654|A-342-OUT,B-431-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:441,x:33474,y:32788|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-445-OUT,OMAX-443-OUT;n:type:ShaderForge.SFN_Subtract,id:443,x:33671,y:32821|A-342-OUT,B-431-OUT;n:type:ShaderForge.SFN_Add,id:445,x:33671,y:32788|A-344-OUT,B-431-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:460,x:33436,y:33141|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-464-OUT,OMAX-462-OUT;n:type:ShaderForge.SFN_Subtract,id:462,x:33628,y:33175|A-342-OUT,B-474-OUT;n:type:ShaderForge.SFN_Add,id:464,x:33644,y:33141|A-344-OUT,B-474-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:466,x:33442,y:31259|IN-120-OUT,IMIN-344-OUT,IMAX-342-OUT,OMIN-468-OUT,OMAX-470-OUT;n:type:ShaderForge.SFN_Subtract,id:468,x:33681,y:31275|A-344-OUT,B-474-OUT;n:type:ShaderForge.SFN_Add,id:470,x:33648,y:31297|A-342-OUT,B-474-OUT;n:type:ShaderForge.SFN_Multiply,id:472,x:34210,y:33568,cmnt:0.08 Offset|A-480-OUT,B-476-OUT;n:type:ShaderForge.SFN_Relay,id:474,x:34054,y:33488|IN-472-OUT;n:type:ShaderForge.SFN_Vector1,id:476,x:34789,y:33603,v1:0.06;n:type:ShaderForge.SFN_Relay,id:480,x:35102,y:32794|IN-481-OUT;n:type:ShaderForge.SFN_RemapRange,id:481,x:35194,y:33999,frmn:0,frmx:1,tomn:0,tomx:0.5|IN-84-OUT;n:type:ShaderForge.SFN_RemapRange,id:487,x:35326,y:34247,frmn:0,frmx:1,tomn:0,tomx:8|IN-3-OUT;proporder:13-3-125-293;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Zoom Blur" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        [MaterialToggle] _UseMips ("Use Mips", Float ) = 0
        _Bias ("Bias", Range(0, 1)) = 1
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
                float node_84 = (_Blur*8.0+0.0);
                float node_110 = lerp(0.0,clamp(node_84,0,1),_UseMips); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_344 = 0.0;
                float node_342 = 1.0;
                float node_480 = (node_84*0.5+0.0);
                float node_474 = (node_480*0.06);
                float node_468 = (node_344-node_474);
                float2 node_466 = (node_468 + ( (node_120 - node_344) * ((node_342+node_474) - node_468) ) / (node_342 - node_344));
                float4 node_260 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_466, _Texture),0.0,node_110));
                float node_433 = (node_480*0.05);
                float node_419 = (node_344-node_433);
                float2 node_417 = (node_419 + ( (node_120 - node_344) * ((node_342+node_433) - node_419) ) / (node_342 - node_344));
                float4 node_258 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_417, _Texture),0.0,node_110));
                float node_431 = (node_480*0.04);
                float node_437 = (node_344-node_431);
                float2 node_435 = (node_437 + ( (node_120 - node_344) * ((node_342+node_431) - node_437) ) / (node_342 - node_344));
                float4 node_230 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_435, _Texture),0.0,node_110));
                float node_429 = (node_480*0.03);
                float node_400 = (node_344-node_429);
                float2 node_398 = (node_400 + ( (node_120 - node_344) * ((node_342+node_429) - node_400) ) / (node_342 - node_344));
                float4 node_204 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_398, _Texture),0.0,node_110));
                float node_409 = (node_480*0.02);
                float node_386 = (node_344-node_409);
                float2 node_384 = (node_386 + ( (node_120 - node_344) * ((node_342+node_409) - node_386) ) / (node_342 - node_344));
                float4 node_61 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_384, _Texture),0.0,node_110));
                float node_373 = (node_480*0.01); // 0.01 offset
                float node_315 = (node_344-node_373);
                float2 node_308 = (node_315 + ( (node_120 - node_344) * ((node_342+node_373) - node_315) ) / (node_342 - node_344));
                float4 node_63 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_308, _Texture),0.0,node_110));
                float node_372 = (node_344+node_373);
                float2 node_364 = (node_372 + ( (node_120 - node_344) * ((node_342-node_373) - node_372) ) / (node_342 - node_344));
                float4 node_57 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_364, _Texture),0.0,node_110));
                float node_394 = (node_344+node_409);
                float2 node_390 = (node_394 + ( (node_120 - node_344) * ((node_342-node_409) - node_394) ) / (node_342 - node_344));
                float4 node_2 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_390, _Texture),0.0,node_110));
                float node_408 = (node_344+node_429);
                float2 node_404 = (node_408 + ( (node_120 - node_344) * ((node_342-node_429) - node_408) ) / (node_342 - node_344));
                float node_445 = (node_344+node_431);
                float2 node_441 = (node_445 + ( (node_120 - node_344) * ((node_342-node_431) - node_445) ) / (node_342 - node_344));
                float4 node_232 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_441, _Texture),0.0,node_110));
                float node_427 = (node_344+node_433);
                float2 node_423 = (node_427 + ( (node_120 - node_344) * ((node_342-node_433) - node_427) ) / (node_342 - node_344));
                float4 node_256 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_423, _Texture),0.0,node_110));
                float node_464 = (node_344+node_474);
                float2 node_460 = (node_464 + ( (node_120 - node_344) * ((node_342-node_474) - node_464) ) / (node_342 - node_344));
                float4 node_262 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_460, _Texture),0.0,node_110));
                float3 emissive = lerp(node_59.rgb,lerp(((((node_260.rgb+node_258.rgb+node_230.rgb+node_204.rgb+node_61.rgb)+node_63.rgb+node_59.rgb+node_57.rgb+node_2.rgb)+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_404, _Texture),0.0,node_110)).rgb+node_232.rgb+node_256.rgb+node_262.rgb)/13.0),(((node_230.rgb+node_232.rgb)+((node_204.rgb+node_204.rgb)*2.0)+((node_61.rgb+node_2.rgb)*4.0)+((node_63.rgb+node_57.rgb)*8.0)+(node_59.rgb*32.0))/64.0),_Bias),saturate(node_84));
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
