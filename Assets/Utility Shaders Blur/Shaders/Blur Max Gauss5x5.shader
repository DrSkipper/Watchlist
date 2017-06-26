// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.35 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.35;sub:START;pass:START;ps:flbk:Unlit/Texture,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.945098,fgcg:0.9137255,fgcb:0.8470588,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31001,y:32479|emission-107-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33111,y:32488,ntxv:0,isnm:False|UVIN-100-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Slider,id:3,x:34585,y:34038,ptlb:Blur,ptin:_Blur,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:13,x:33448,y:34229,ptlb:Texture,ptin:_Texture,glob:False;n:type:ShaderForge.SFN_Tex2d,id:57,x:33111,y:32363,ntxv:0,isnm:False|UVIN-98-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:59,x:33111,y:32243,ntxv:0,isnm:False|UVIN-94-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:61,x:33111,y:32121,ntxv:0,isnm:False|UVIN-96-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:63,x:33111,y:31999,ntxv:0,isnm:False|UVIN-93-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_TexCoord,id:71,x:34297,y:32240,uv:0;n:type:ShaderForge.SFN_Add,id:72,x:33886,y:32417,cmnt:V plus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Vector1,id:73,x:35006,y:32638,v1:2;n:type:ShaderForge.SFN_Power,id:74,x:34848,y:32722,cmnt:Pixel Offset|VAL-73-OUT,EXP-85-OUT;n:type:ShaderForge.SFN_ValueProperty,id:75,x:34848,y:32916,ptlb:Texture Size,ptin:_TextureSize,glob:False,v1:128;n:type:ShaderForge.SFN_Relay,id:84,x:34446,y:33838|IN-289-OUT;n:type:ShaderForge.SFN_Relay,id:85,x:35047,y:33068|IN-84-OUT;n:type:ShaderForge.SFN_Divide,id:86,x:34558,y:32726,cmnt:Calculate Offset by Mip|A-74-OUT,B-75-OUT;n:type:ShaderForge.SFN_Subtract,id:87,x:33875,y:32028,cmnt:U minus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Add,id:90,x:33875,y:31894,cmnt:U plus Offset|A-71-U,B-86-OUT;n:type:ShaderForge.SFN_Subtract,id:92,x:33886,y:32563,cmnt:V minus Offset|A-71-V,B-86-OUT;n:type:ShaderForge.SFN_Append,id:93,x:33446,y:31998|A-105-OUT,B-72-OUT;n:type:ShaderForge.SFN_Relay,id:94,x:33916,y:32238|IN-71-UVOUT;n:type:ShaderForge.SFN_Append,id:96,x:33446,y:32120|A-105-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:98,x:33446,y:32365|A-90-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:100,x:33446,y:32491|A-87-OUT,B-106-OUT;n:type:ShaderForge.SFN_Relay,id:105,x:33916,y:32174,cmnt:U|IN-71-U;n:type:ShaderForge.SFN_Relay,id:106,x:33916,y:32332,cmnt:V|IN-71-V;n:type:ShaderForge.SFN_Lerp,id:107,x:31343,y:32613|A-59-RGB,B-176-OUT,T-290-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:109,x:34306,y:33693,min:0,max:1|IN-84-OUT;n:type:ShaderForge.SFN_Relay,id:110,x:33946,y:33512,cmnt:Mip Level|IN-286-OUT;n:type:ShaderForge.SFN_Tex2d,id:112,x:33111,y:32633,ntxv:0,isnm:False|UVIN-124-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:114,x:33111,y:32787,ntxv:0,isnm:False|UVIN-126-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:116,x:33111,y:31876,ntxv:0,isnm:False|UVIN-120-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:118,x:33111,y:31745,ntxv:0,isnm:False|UVIN-122-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Append,id:120,x:33446,y:31867|A-87-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:122,x:33446,y:31734|A-90-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:124,x:33446,y:32624|A-90-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:126,x:33446,y:32748|A-87-OUT,B-92-OUT;n:type:ShaderForge.SFN_Add,id:128,x:32730,y:32062,cmnt:Close|A-63-RGB,B-61-RGB,C-57-RGB,D-2-RGB;n:type:ShaderForge.SFN_Add,id:130,x:32730,y:31816,cmnt:Near|A-118-RGB,B-116-RGB,C-112-RGB,D-114-RGB;n:type:ShaderForge.SFN_Relay,id:131,x:32759,y:32290,cmnt:Center Pixels|IN-59-RGB;n:type:ShaderForge.SFN_Multiply,id:132,x:32040,y:31819|A-140-OUT,B-130-OUT;n:type:ShaderForge.SFN_Multiply,id:133,x:32040,y:32020|A-138-OUT,B-128-OUT;n:type:ShaderForge.SFN_Multiply,id:134,x:32040,y:32224|A-136-OUT,B-131-OUT;n:type:ShaderForge.SFN_Add,id:135,x:31816,y:31780|A-280-OUT,B-132-OUT,C-133-OUT,D-134-OUT;n:type:ShaderForge.SFN_Vector1,id:136,x:32040,y:32166,v1:41;n:type:ShaderForge.SFN_Vector1,id:138,x:32040,y:31965,v1:26;n:type:ShaderForge.SFN_Vector1,id:140,x:32040,y:31761,v1:16;n:type:ShaderForge.SFN_Add,id:142,x:32730,y:31605,cmnt:Med|A-150-RGB,B-148-RGB,C-152-RGB,D-154-RGB;n:type:ShaderForge.SFN_Multiply,id:143,x:34337,y:32798,cmnt:Double Offset|A-86-OUT,B-144-OUT;n:type:ShaderForge.SFN_Vector1,id:144,x:34558,y:32878,v1:1.5;n:type:ShaderForge.SFN_Tex2d,id:148,x:33111,y:31622,ntxv:0,isnm:False|UVIN-164-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:150,x:33111,y:31496,ntxv:0,isnm:False|UVIN-166-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:152,x:33111,y:32922,ntxv:0,isnm:False|UVIN-168-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:154,x:33111,y:33057,ntxv:0,isnm:False|UVIN-170-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Subtract,id:156,x:33874,y:31750,cmnt:U minus minus Offset|A-71-U,B-143-OUT;n:type:ShaderForge.SFN_Add,id:158,x:33874,y:31608,cmnt:U plus plus Offset|A-71-U,B-143-OUT;n:type:ShaderForge.SFN_Add,id:160,x:33886,y:32700,cmnt:V plus plus Offset|A-71-V,B-143-OUT;n:type:ShaderForge.SFN_Subtract,id:162,x:33886,y:32846,cmnt:V minus minus Offset|A-71-V,B-143-OUT;n:type:ShaderForge.SFN_Append,id:164,x:33446,y:31595|A-156-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:166,x:33446,y:31458|A-158-OUT,B-106-OUT;n:type:ShaderForge.SFN_Append,id:168,x:33446,y:32883|A-105-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:170,x:33446,y:33003|A-105-OUT,B-162-OUT;n:type:ShaderForge.SFN_Vector1,id:172,x:32040,y:31570,v1:7;n:type:ShaderForge.SFN_Multiply,id:174,x:32040,y:31621|A-172-OUT,B-142-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:175,x:31479,y:32324,ptlb:Bias,ptin:_Bias,on:False;n:type:ShaderForge.SFN_Lerp,id:176,x:31454,y:32144|A-182-OUT,B-282-OUT,T-175-OUT;n:type:ShaderForge.SFN_Add,id:177,x:32482,y:32578|A-274-OUT,B-130-OUT,C-128-OUT,D-131-OUT;n:type:ShaderForge.SFN_Divide,id:182,x:32405,y:32749|A-177-OUT,B-183-OUT;n:type:ShaderForge.SFN_Vector1,id:183,x:32628,y:32837,v1:25;n:type:ShaderForge.SFN_Add,id:185,x:32730,y:31400,cmnt:far|A-189-RGB,B-187-RGB,C-195-RGB,D-197-RGB;n:type:ShaderForge.SFN_Tex2d,id:187,x:33111,y:31394,ntxv:0,isnm:False|UVIN-191-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:189,x:33111,y:31287,ntxv:0,isnm:False|UVIN-193-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Append,id:191,x:33446,y:31323|A-158-OUT,B-162-OUT;n:type:ShaderForge.SFN_Append,id:193,x:33446,y:31196|A-158-OUT,B-160-OUT;n:type:ShaderForge.SFN_Tex2d,id:195,x:33111,y:33185,ntxv:0,isnm:False|UVIN-234-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:197,x:33111,y:33326,ntxv:0,isnm:False|UVIN-236-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Vector1,id:217,x:32040,y:31373,v1:4;n:type:ShaderForge.SFN_Multiply,id:219,x:32040,y:31439|A-217-OUT,B-185-OUT;n:type:ShaderForge.SFN_Append,id:234,x:33446,y:33142|A-156-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:236,x:33446,y:33276|A-156-OUT,B-162-OUT;n:type:ShaderForge.SFN_Tex2d,id:239,x:33111,y:31146,ntxv:0,isnm:False|UVIN-257-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:241,x:33111,y:30990,ntxv:0,isnm:False|UVIN-259-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:243,x:33111,y:30832,ntxv:0,isnm:False|UVIN-261-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:245,x:33111,y:30682,ntxv:0,isnm:False|UVIN-263-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:247,x:33111,y:33451,ntxv:0,isnm:False|UVIN-265-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:249,x:33111,y:33593,ntxv:0,isnm:False|UVIN-267-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:251,x:33111,y:33736,ntxv:0,isnm:False|UVIN-269-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Tex2d,id:253,x:33111,y:33880,ntxv:0,isnm:False|UVIN-271-OUT,MIP-110-OUT,TEX-13-TEX;n:type:ShaderForge.SFN_Add,id:255,x:32730,y:31019,cmnt:very far 2|A-247-RGB,B-249-RGB,C-251-RGB,D-253-RGB;n:type:ShaderForge.SFN_Append,id:257,x:33446,y:31068|A-158-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:259,x:33446,y:30937|A-158-OUT,B-72-OUT;n:type:ShaderForge.SFN_Append,id:261,x:33446,y:30792|A-90-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:263,x:33446,y:30652|A-87-OUT,B-160-OUT;n:type:ShaderForge.SFN_Append,id:265,x:33446,y:33414|A-90-OUT,B-162-OUT;n:type:ShaderForge.SFN_Append,id:267,x:33446,y:33549|A-87-OUT,B-162-OUT;n:type:ShaderForge.SFN_Append,id:269,x:33446,y:33687|A-156-OUT,B-92-OUT;n:type:ShaderForge.SFN_Append,id:271,x:33446,y:33851|A-156-OUT,B-72-OUT;n:type:ShaderForge.SFN_Add,id:273,x:32730,y:31211,cmnt:very far 1|A-245-RGB,B-243-RGB,C-241-RGB,D-239-RGB;n:type:ShaderForge.SFN_Add,id:274,x:32482,y:32416|A-255-OUT,B-273-OUT,C-185-OUT,D-142-OUT;n:type:ShaderForge.SFN_Add,id:280,x:31816,y:31608|A-281-OUT,B-219-OUT,C-174-OUT;n:type:ShaderForge.SFN_Add,id:281,x:32040,y:31222|A-255-OUT,B-273-OUT;n:type:ShaderForge.SFN_Divide,id:282,x:31627,y:31828|A-135-OUT,B-283-OUT;n:type:ShaderForge.SFN_Vector1,id:283,x:31816,y:31936,v1:273;n:type:ShaderForge.SFN_ToggleProperty,id:285,x:34045,y:33826,ptlb:Use Mips,ptin:_UseMips,on:True;n:type:ShaderForge.SFN_Lerp,id:286,x:33994,y:33610|A-287-OUT,B-109-OUT,T-285-OUT;n:type:ShaderForge.SFN_Vector1,id:287,x:34131,y:33426,v1:0;n:type:ShaderForge.SFN_Vector1,id:288,x:34747,y:33808,v1:2.5;n:type:ShaderForge.SFN_Multiply,id:289,x:34585,y:33838|A-288-OUT,B-3-OUT;n:type:ShaderForge.SFN_Relay,id:290,x:33475,y:34779|IN-3-OUT;proporder:13-3-75-175-285;pass:END;sub:END;*/

Shader "Utility Shaders/Blur/Blur Gauss 5x5" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 1
        _TextureSize ("Texture Size", Float ) = 128
        [MaterialToggle] _Bias ("Bias", Float ) = 0
        [MaterialToggle] _UseMips ("Use Mips", Float ) = 0
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
                float2 node_94 = node_71.rg;
                float node_84 = (2.5*_Blur);
                float node_110 = lerp(0.0,clamp(node_84,0,1),_UseMips); // Mip Level
                float4 node_59 = tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_94, _Texture),0.0,node_110));
                float node_86 = (pow(2.0,node_84)/_TextureSize); // Calculate Offset by Mip
                float node_90 = (node_71.r+node_86); // U plus Offset
                float node_143 = (node_86*1.5); // Double Offset
                float node_162 = (node_71.g-node_143); // V minus minus Offset
                float2 node_265 = float2(node_90,node_162);
                float node_87 = (node_71.r-node_86); // U minus Offset
                float2 node_267 = float2(node_87,node_162);
                float node_156 = (node_71.r-node_143); // U minus minus Offset
                float node_92 = (node_71.g-node_86); // V minus Offset
                float2 node_269 = float2(node_156,node_92);
                float node_72 = (node_71.g+node_86); // V plus Offset
                float2 node_271 = float2(node_156,node_72);
                float3 node_255 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_265, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_267, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_269, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_271, _Texture),0.0,node_110)).rgb); // very far 2
                float node_160 = (node_71.g+node_143); // V plus plus Offset
                float2 node_263 = float2(node_87,node_160);
                float2 node_261 = float2(node_90,node_160);
                float node_158 = (node_71.r+node_143); // U plus plus Offset
                float2 node_259 = float2(node_158,node_72);
                float2 node_257 = float2(node_158,node_92);
                float3 node_273 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_263, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_261, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_259, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_257, _Texture),0.0,node_110)).rgb); // very far 1
                float2 node_193 = float2(node_158,node_160);
                float2 node_191 = float2(node_158,node_162);
                float2 node_234 = float2(node_156,node_160);
                float2 node_236 = float2(node_156,node_162);
                float3 node_185 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_193, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_191, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_234, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_236, _Texture),0.0,node_110)).rgb); // far
                float node_106 = node_71.g; // V
                float2 node_166 = float2(node_158,node_106);
                float2 node_164 = float2(node_156,node_106);
                float node_105 = node_71.r; // U
                float2 node_168 = float2(node_105,node_160);
                float2 node_170 = float2(node_105,node_162);
                float3 node_142 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_166, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_164, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_168, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_170, _Texture),0.0,node_110)).rgb); // Med
                float2 node_122 = float2(node_90,node_72);
                float2 node_120 = float2(node_87,node_72);
                float2 node_124 = float2(node_90,node_92);
                float2 node_126 = float2(node_87,node_92);
                float3 node_130 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_122, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_120, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_124, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_126, _Texture),0.0,node_110)).rgb); // Near
                float2 node_93 = float2(node_105,node_72);
                float2 node_96 = float2(node_105,node_92);
                float2 node_98 = float2(node_90,node_106);
                float2 node_100 = float2(node_87,node_106);
                float3 node_128 = (tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_93, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_96, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_98, _Texture),0.0,node_110)).rgb+tex2Dlod(_Texture,float4(TRANSFORM_TEX(node_100, _Texture),0.0,node_110)).rgb); // Close
                float3 node_131 = node_59.rgb; // Center Pixels
                float3 emissive = lerp(node_59.rgb,lerp((((node_255+node_273+node_185+node_142)+node_130+node_128+node_131)/25.0),((((node_255+node_273)+(4.0*node_185)+(7.0*node_142))+(16.0*node_130)+(26.0*node_128)+(41.0*node_131))/273.0),_Bias),_Blur);
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
