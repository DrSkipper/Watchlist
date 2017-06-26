// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:30468,y:32772|emission-191-OUT,alpha-1067-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-100-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34534,y:34004,ptlb:Blur,ptin:_Blur,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33366,y:33285,ptlb:Texture,ptin:_Texture,glob:False,tex:e757048ded3d3d840a28c139df222c67;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-98-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:32121,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-96-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:31999,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-93-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_TexCoord,id:71,x:34971,y:32317,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33886,y:32417,cmnt:V plus Offset|A-187-OUT,B-86-OUT;n:type:ShaderForge.SFN_Vector1,id:73,x:35006,y:32638,v1:2;n:type:ShaderForge.SFN_Power,id:74,x:34848,y:32722,cmnt:Pixel Offset|VAL-73-OUT,EXP-85-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34858,y:32859,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:128;n:type:ShaderForge.SFN_Relay,id:84,x:34446,y:33838|IN-653-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:35047,y:33068|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34471,y:32675,cmnt:Calculate Offset by Mip|A-74-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33875,y:32028,cmnt:U minus Offset|A-186-OUT,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33875,y:31894,cmnt:U plus Offset|A-186-OUT,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33886,y:32563,cmnt:V minus Offset|A-187-OUT,B-86-OUT;n:type:ShaderForge.SFN_Append,id:93,x:33446,y:31998|A-105-OUT,B-72-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33916,y:32238|IN-188-OUT;n:type:ShaderForge.SFN_Append,id:96,x:33446,y:32120|A-105-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:98,x:33446,y:32365|A-90-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:100,x:33446,y:32491|A-87-OUT,B-106-OUT;n:type:ShaderForge.SFN_Relay,id:105,x:33916,y:32174,cmnt:U|IN-186-OUT;n:type:ShaderForge.SFN_Relay,id:106,x:33916,y:32332,cmnt:V|IN-187-OUT;n:type:ShaderForge.SFN_Lerp,id:107,x:31941,y:32964|A-59-A,B-176-OUT,T-654-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:34306,y:33693,min:0,max:3|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:34159,y:33490,cmnt:Mip Level|IN-109-OUT;n:type:ShaderForge.SFN_Tex2d,id:112,x:33111,y:32633,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-124-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:114,x:33111,y:32787,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-126-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:116,x:33111,y:31876,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-120-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:118,x:33111,y:31745,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-122-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Append,id:120,x:33446,y:31867|A-87-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:122,x:33446,y:31734|A-90-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:124,x:33446,y:32624|A-90-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:126,x:33446,y:32748|A-87-OUT,B-92-OUT;n:type:ShaderForge.SFN_Add,id:128,x:32730,y:32062,cmnt:Near|A-63-A,B-61-A,C-57-A,D-2-A;n:type:ShaderForge.SFN_Add,id:130,x:32730,y:31816,cmnt:Mid Pixels|A-118-A,B-116-A,C-112-A,D-114-A;n:type:ShaderForge.SFN_Relay,id:131,x:32759,y:32290,cmnt:Center Pixels|IN-59-A;n:type:ShaderForge.SFN_Multiply,id:132,x:32399,y:31738|A-140-OUT,B-130-OUT;n:type:ShaderForge.SFN_Multiply,id:133,x:32399,y:31939|A-138-OUT,B-128-OUT;n:type:ShaderForge.SFN_Multiply,id:134,x:32399,y:32143|A-136-OUT,B-131-OUT;n:type:ShaderForge.SFN_Add,id:135,x:32162,y:31717|A-174-OUT,B-132-OUT,C-133-OUT,D-134-OUT;n:type:ShaderForge.SFN_Vector1,id:136,x:32399,y:32085,v1:0.4;n:type:ShaderForge.SFN_Vector1,id:138,x:32399,y:31884,v1:0.075;n:type:ShaderForge.SFN_Vector1,id:140,x:32399,y:31680,v1:0.05;n:type:ShaderForge.SFN_Add,id:142,x:32730,y:31605,cmnt:Far Pixels|A-150-A,B-148-A,C-152-A,D-154-A;n:type:ShaderForge.SFN_Multiply,id:143,x:34337,y:32798,cmnt:Double Offset|A-86-OUT,B-184-OUT;n:type:ShaderForge.SFN_Tex2d,id:148,x:33111,y:31622,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-164-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:150,x:33111,y:31496,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-166-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:152,x:33111,y:32922,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-168-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:154,x:33111,y:33057,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|UVIN-170-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Subtract,id:156,x:33874,y:31750,cmnt:U minus minus Offset|A-186-OUT,B-143-OUT;n:type:ShaderForge.SFN_Add,id:158,x:33874,y:31608,cmnt:U plus plus Offset|A-186-OUT,B-143-OUT;n:type:ShaderForge.SFN_Add,id:160,x:33886,y:32700,cmnt:V plus plus Offset|A-187-OUT,B-143-OUT;n:type:ShaderForge.SFN_Subtract,id:162,x:33886,y:32846,cmnt:V minus minus Offset|A-187-OUT,B-143-OUT;n:type:ShaderForge.SFN_Append,id:164,x:33446,y:31595|A-156-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:166,x:33446,y:31458|A-158-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:168,x:33446,y:32883|A-105-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:170,x:33446,y:33003|A-105-OUT,B-162-OUT;n:type:ShaderForge.SFN_Vector1,id:172,x:32399,y:31489,v1:0.025;n:type:ShaderForge.SFN_Multiply,id:174,x:32399,y:31540|A-172-OUT,B-142-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:175,x:32198,y:32624,ptlb:Bias,ptin:_Bias,on:False;n:type:ShaderForge.SFN_Lerp,id:176,x:32052,y:32495|A-182-OUT,B-135-OUT,T-175-OUT;n:type:ShaderForge.SFN_Add,id:177,x:32509,y:32351|A-142-OUT,B-130-OUT,C-128-OUT,D-131-OUT;n:type:ShaderForge.SFN_Divide,id:182,x:32348,y:32400|A-177-OUT,B-183-OUT;n:type:ShaderForge.SFN_Vector1,id:183,x:32509,y:32505,v1:13;n:type:ShaderForge.SFN_Vector1,id:184,x:34541,y:32967,v1:1.5;n:type:ShaderForge.SFN_OneMinus,id:185,x:31801,y:32737|IN-107-OUT;n:type:ShaderForge.SFN_Relay,id:186,x:34444,y:32154|IN-274-OUT;n:type:ShaderForge.SFN_Relay,id:187,x:34444,y:32361|IN-302-OUT;n:type:ShaderForge.SFN_Relay,id:188,x:34444,y:32254|IN-327-OUT;n:type:ShaderForge.SFN_Tex2d,id:190,x:33111,y:33285,tex:e757048ded3d3d840a28c139df222c67,ntxv:0,isnm:False|TEX-13-TEX;n:type:ShaderForge.SFN_Lerp,id:191,x:31306,y:33203|A-209-OUT,B-190-RGB,T-190-A;n:type:ShaderForge.SFN_Color,id:208,x:31797,y:32438,ptlb:Background Color,ptin:_BackgroundColor,glob:False,c1:0.9338235,c2:0.9338235,c3:0.9338235,c4:1;n:type:ShaderForge.SFN_Multiply,id:209,x:31489,y:33071|A-208-RGB,B-1086-OUT;n:type:ShaderForge.SFN_Slider,id:255,x:35977,y:31984,ptlb:Angle,ptin:_Angle,min:0,cur:0,max:360;n:type:ShaderForge.SFN_Vector1,id:257,x:35988,y:32067,v1:0;n:type:ShaderForge.SFN_Vector1,id:259,x:35988,y:32138,v1:360;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:261,x:35709,y:32209|IN-255-OUT,IMIN-257-OUT,IMAX-259-OUT,OMIN-265-OUT,OMAX-263-OUT;n:type:ShaderForge.SFN_Pi,id:263,x:36004,y:32353;n:type:ShaderForge.SFN_Negate,id:265,x:35988,y:32199|IN-263-OUT;n:type:ShaderForge.SFN_Sin,id:267,x:35491,y:32106,cmnt:U|IN-261-OUT;n:type:ShaderForge.SFN_Cos,id:269,x:35491,y:32281,cmnt:V|IN-261-OUT;n:type:ShaderForge.SFN_Append,id:271,x:35315,y:32181,cmnt:UV|A-267-OUT,B-269-OUT;n:type:ShaderForge.SFN_Relay,id:274,x:34619,y:32154|IN-437-OUT;n:type:ShaderForge.SFN_Relay,id:302,x:34604,y:32343|IN-438-OUT;n:type:ShaderForge.SFN_Relay,id:327,x:34605,y:32276|IN-439-OUT;n:type:ShaderForge.SFN_Multiply,id:333,x:35114,y:32160|A-335-OUT,B-271-OUT;n:type:ShaderForge.SFN_Slider,id:334,x:35139,y:31852,ptlb:Offset,ptin:_Offset,min:-1,cur:0.6857609,max:1;n:type:ShaderForge.SFN_Multiply,id:335,x:35114,y:31974|A-334-OUT,B-337-OUT;n:type:ShaderForge.SFN_Vector1,id:337,x:35450,y:31955,v1:0.125;n:type:ShaderForge.SFN_Divide,id:339,x:35274,y:31994|A-337-OUT,B-75-OUT;n:type:ShaderForge.SFN_ComponentMask,id:340,x:34986,y:32171,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-333-OUT;n:type:ShaderForge.SFN_Add,id:437,x:34787,y:32223|A-340-R,B-71-U;n:type:ShaderForge.SFN_Add,id:438,x:34787,y:32343|A-71-V,B-340-G;n:type:ShaderForge.SFN_Add,id:439,x:34787,y:32103|A-340-OUT,B-71-UVOUT;n:type:ShaderForge.SFN_RemapRange,id:653,x:34563,y:33786,frmn:0,frmx:1,tomn:0,tomx:3|IN-3-OUT;n:type:ShaderForge.SFN_Relay,id:654,x:32890,y:33869|IN-3-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:1062,x:31797,y:32343,ptlb:Alpha,ptin:_Alpha,on:False;n:type:ShaderForge.SFN_Multiply,id:1063,x:31322,y:32600|A-208-A,B-1074-OUT;n:type:ShaderForge.SFN_Add,id:1065,x:31020,y:32662|A-1063-OUT,B-190-A;n:type:ShaderForge.SFN_Clamp01,id:1066,x:31020,y:32533|IN-1065-OUT;n:type:ShaderForge.SFN_Lerp,id:1067,x:30798,y:32643|A-1068-OUT,B-1066-OUT,T-1062-OUT;n:type:ShaderForge.SFN_Vector1,id:1068,x:30807,y:32569,v1:1;n:type:ShaderForge.SFN_OneMinus,id:1074,x:31491,y:32624|IN-185-OUT;n:type:ShaderForge.SFN_Lerp,id:1086,x:31635,y:32856|A-185-OUT,B-1088-OUT,T-1062-OUT;n:type:ShaderForge.SFN_Vector1,id:1088,x:31635,y:32985,v1:0;proporder:13-3-75-175-208-255-334-1062;pass:END;sub:END;*/

Shader "Utility Shaders/Extras/Drop Shadow" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 0.2
        _TextureSize ("Texture Size", Float ) = 128
        [MaterialToggle] _Bias ("Bias", Float ) = 0
        _BackgroundColor ("Background Color", Color) = (0.9338235,0.9338235,0.9338235,1)
        _Angle ("Angle", Range(0, 360)) = 0
        _Offset ("Offset", Range(-1, 1)) = 0.6857609
        [MaterialToggle] _Alpha ("Alpha", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float _Blur;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _TextureSize;
            uniform fixed _Bias;
            uniform float4 _BackgroundColor;
            uniform float _Angle;
            uniform float _Offset;
            uniform fixed _Alpha;
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
                float node_337 = 0.125;
                float node_257 = 0.0;
                float node_263 = 3.141592654;
                float node_265 = (-1*node_263);
                float node_261 = (node_265 + ( (_Angle - node_257) * (node_263 - node_265) ) / (360.0 - node_257));
                float2 node_340 = ((_Offset*node_337)*float2(sin(node_261),cos(node_261))).rg;
                float2 node_71 = i.uv0;
                float2 node_94 = (node_340+node_71.rg);
                float node_84 = (_Blur*3.0+0.0);
                float node_110 = clamp(node_84,0,3); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_186 = (node_340.r+node_71.r);
                float node_86 = (pow(2.0,node_84)/_TextureSize); // Calculate Offset by Mip
                float node_143 = (node_86*1.5); // Double Offset
                float node_187 = (node_71.g+node_340.g);
                float node_106 = node_187; // V
                float2 node_166 = float2((node_186+node_143),node_106);
                float2 node_164 = float2((node_186-node_143),node_106);
                float node_105 = node_186; // U
                float2 node_168 = float2(node_105,(node_187+node_143));
                float2 node_170 = float2(node_105,(node_187-node_143));
                float node_142 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_166, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_164, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_168, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_170, _Texture),0.0,node_110)).a); // Far Pixels
                float node_90 = (node_186+node_86); // U plus Offset
                float node_72 = (node_187+node_86); // V plus Offset
                float2 node_122 = float2(node_90,node_72);
                float node_87 = (node_186-node_86); // U minus Offset
                float2 node_120 = float2(node_87,node_72);
                float node_92 = (node_187-node_86); // V minus Offset
                float2 node_124 = float2(node_90,node_92);
                float2 node_126 = float2(node_87,node_92);
                float node_130 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_122, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_120, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_124, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_126, _Texture),0.0,node_110)).a); // Mid Pixels
                float2 node_93 = float2(node_105,node_72);
                float2 node_96 = float2(node_105,node_92);
                float2 node_98 = float2(node_90,node_106);
                float2 node_100 = float2(node_87,node_106);
                float node_128 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_93, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_96, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_98, _Texture),0.0,node_110)).a+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_100, _Texture),0.0,node_110)).a); // Near
                float node_131 = node_59.a; // Center Pixels
                float node_185 = (1.0 - lerp(node_59.a,lerp(((node_142+node_130+node_128+node_131)/13.0),((0.025*node_142)+(0.05*node_130)+(0.075*node_128)+(0.4*node_131)),_Bias),_Blur));
                float2 node_1096 = i.uv0;
                float4 node_190 = tex2D(_Texture,TRANSFORM_TEX(node_1096.rg, _Texture));
                float3 emissive = lerp((_BackgroundColor.rgb*lerp(node_185,0.0,_Alpha)),node_190.rgb,node_190.a);
                float3 finalColor = emissive;
                float node_1063 = (_BackgroundColor.a*(1.0 - node_185));
/// Final Color:
                return fixed4(finalColor,lerp(1.0,saturate((node_1063+node_190.a)),_Alpha));
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
    CustomEditor "ShaderForgeMaterialInspector"
}
