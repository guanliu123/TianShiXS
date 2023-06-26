Shader "Nan/NanoEffectsShader"
{
	Properties
	{
		[Enum(AlphaBlend, 10,Additive,1)]_AlphaBlend("AlphaBlend", Float) = 10
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 2
		[Enum(Off,0,On,1)]_ZWrite("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest", Float) = 4

        //主帖图
		_MainTex("主贴图", 2D) = "white" {}
		[HDR]_MainColorBack("亮部颜色", Color) = (1,1,1,1)
        //[HDR]_MainColorFront("背面亮部颜色", Color) = (0,0,0,1)
        [HDR]_MainColorBack02("暗部颜色", Color) = (0,0,0,1)
        _MainColorFront("反面颜色明暗", range(0.0,5.0)) = 1
        _MainSpeedInt("XY：主贴图速度 Z：颜色亮度 W：速度强度", Vector) = (0,0,1,1)
        [MaterialToggle] _MainUVCustom("主贴图流动速度Custom1.ZW曲线开关", Float) = 0
        [Space(5)]
        [MaterialToggle] _MainTexAlpha("主贴图R和A通道选择", Float) = 0

		 _Clip("Clip裁切 自身溶解", Range(0.0, 1.05)) = 0
        [Toggle] _ClipCustom("主贴图Clip裁切Custom1.X曲线开关", Float) = 0

        //主贴图闪烁
        [Space(5)]
        [Toggle] _Shan("主贴图闪烁", Int) = 0
        [Space(2)]
		_ShanPinLv("闪烁频率", Float) = 4
		_ShanZhenFu("闪烁幅度", Float) = 1

        //主帖图UV缩放，旋转
        [Space(5)]

        _MainUVScale("主贴图UV缩放", Float) = 1
        [MaterialToggle] _MainUVScaleCustom("主贴图UV缩放Custom2.X曲线开关", Float) = 0
		[KeywordEnum(Off,UVRotator,UVTwirl)] _UVxuanze("主贴图UV类型选择", Float) = 0
		
		_RotatorInt("主贴图旋转值", Float) = 1
		_TwirlInt("主贴图漩涡强度", Float) = 0
		_CeterScale("漩涡中心缩放", Float) = 1
		_UVScaleOffset("XY：漩涡UV平铺值 ZW：漩涡UV偏移", Vector) = (1,1,0,0)

        //Mask贴图
        [Header(MaskTex)]
        [Toggle] _UseMaskTex("启用遮罩功能", Int) = 0
        [Space(2)]
		_MaskTex("遮罩图，使用R通道", 2D) = "white" {}
		_MaskSpeedInt("XY：遮罩图速度 Z：遮罩扰动强度(受扰动图影响) W：速度强度", Vector) = (0,0,0,1)
        [MaterialToggle] _MaskUVCustom("遮罩图速度流动Custom2.ZW曲线开关", Float) = 0

        //溶解贴图
        [Header(DissolveTex)]
        [Toggle] _UseDissolveTex("启用溶解功能", Int) = 0
        [Space(2)]
		_DissolveTex("溶解图，使用R通道", 2D) = "white" {}
		_DissolveSpeedInt("XY：溶解图速度 Z：溶解扰动强度(受扰动图影响) W：速度强度", Vector) = (0,0,0,1) 
		_dissolveInt("主贴图溶解值", Range( 0 , 1)) = 0.0
        [MaterialToggle] _dissolveCustom("主贴图溶解Custom1.Y曲线开关", Float) = 0
		_hardness("溶解软硬度", Range( 0 , 1)) = 0
		_width("溶解亮边宽度", Range( 0 , 1)) = 0
		[HDR]_widthColor("溶解亮边颜色", Color) = (1,1,1,1)

        //扰动图
        [Header(NoiseTex)]
        [Toggle] _UseNoiseTex("启用扰动贴图", Int) = 0
        [Space(2)]
		_NoiseTex("扰动图，使用R和G通道", 2D) = "white" {}
		_Noise01SpeedInt("XY：扰动图R速度 Z：主贴图被R扰动强度 W：R速度强度", Vector) = (0,0,0.01,1)
        [MaterialToggle] _MainNoiseIntCustom("主贴图扰动Custom2.Y曲线开关", Float) = 0
		[MaterialToggle] _DoubleNoise("打开扰动图R和G通道，为双扰动", Float) = 0
		_Noise02Speed("XY：扰动图G速度 Z：无 W：G速度强度", Vector) = (0,0,0,1)
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord3( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent" "ForceNoShadowCasting" = "Ture" "IgnoreProjector" = "Ture"}
		

        Pass
        {
            Name "FORWARD"
            Tags{
                "LightMode"="ForwardBase"
            }

                Cull [_CullMode]
                ZWrite [_ZWrite]
                ZTest [_ZTest]
                Blend SrcAlpha [_AlphaBlend]
     
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0   
            #pragma multi_compile_local __ _UVXUANZE_UVROTATOR _UVXUANZE_UVTWIRL
            #pragma multi_compile_local __ _USEDISSOLVETEX_ON
            #pragma multi_compile_local __ _USENOISETEX_ON
            #pragma multi_compile_local __ _USEMASKTEX_ON

            #include "UnityCG.cginc"
            struct a2v
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                half4 vertexColor : COLOR;
                half4 customData1:TEXCOORD1;
                half4 customData2:TEXCOORD2;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv1 : TEXCOORD0;
                half4 customData1:TEXCOORD1;
                half4 customData2:TEXCOORD2;
                float4 uv2 : TEXCOORD3;
                half4 vertexColor:COLOR;
            };
            uniform int _AlphaBlend;
            uniform int _ZWrite;
            uniform int _CullMode;
            uniform int _ZTest;

            //主帖图
            uniform half _MainColorFront;
            uniform half4 _MainColorBack;
            uniform half4 _MainColorBack02;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half4 _MainSpeedInt;
            uniform half _MainUVCustom;
            uniform half _MainTexAlpha;

             uniform half _Clip;
             uniform half _ClipCustom;

            //主帖图UV缩放 旋转
            uniform half _MainUVScale;
            uniform half _MainUVScaleCustom;
            uniform half _RotatorInt;
            uniform half _TwirlInt;
            uniform half _CeterScale;
            uniform half4 _UVScaleOffset;

            //扰动图
            #ifdef _USENOISETEX_ON
                uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
                uniform half4 _Noise01SpeedInt;
                uniform half _MainNoiseIntCustom;
                uniform half _DoubleNoise;
                uniform half4 _Noise02Speed;
            #endif

            //主帖图闪烁
            uniform half _Shan;
            uniform half _ShanPinLv;
            uniform half _ShanZhenFu;

            //溶解图
            #ifdef _USEDISSOLVETEX_ON
                uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
                uniform half4 _DissolveSpeedInt;
                uniform half _dissolveInt;
                uniform half _dissolveCustom;
                uniform half _hardness;
                uniform half _width;
                uniform half4 _widthColor;
            #endif

            //遮罩图
            #ifdef _USEMASKTEX_ON
                uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
                uniform half4 _MaskSpeedInt;
                uniform half _MaskUVCustom;
            #endif

            v2f vert (a2v v)
            {
                v2f o = (v2f)0;
                o.customData1 = v.customData1;
                o.customData2 = v.customData2;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexColor = v.vertexColor;
                o.uv1.xy = TRANSFORM_TEX(v.uv, _MainTex);

                #ifdef _USEMASKTEX_ON
                o.uv1.zw = TRANSFORM_TEX(v.uv, _MaskTex);
                #endif

                #ifdef _USEDISSOLVETEX_ON
                o.uv2.xy = TRANSFORM_TEX(v.uv, _DissolveTex);
                #endif

                #ifdef _USENOISETEX_ON
                    o.uv2.zw = TRANSFORM_TEX(v.uv, _NoiseTex);
                #endif

                return o;
            }

            float4 frag(v2f i, float facing : VFACE) : SV_Target
            {
                half2 uvScale =  i.uv1.xy - 0.5 ;
                half MainTexUVScale = lerp(_MainUVScale, i.customData2.x, _MainUVScaleCustom);
                uvScale *= MainTexUVScale;
                
                //02旋转
                #if defined(_UVXUANZE_UVROTATOR)
                    half2 MainTexUV_01 = uvScale;
                    half cos02 = cos(_RotatorInt * _Time.g);
                    half sin02 = sin(_RotatorInt * _Time.g);
                    half2 rotator02 = float2((cos02 * MainTexUV_01.x - sin02 * MainTexUV_01.y), (sin02 * MainTexUV_01.x + cos02 * MainTexUV_01.y));
                    uvScale = rotator02;

                //03旋涡
                #elif defined(_UVXUANZE_UVTWIRL)
                    half cos01 = cos(1.0 - length(uvScale) * 2.0 * _TwirlInt);
                    half sin01 = sin(1.0 - length(uvScale) * 2.0 * _TwirlInt);
                    half2 rotator01 = mul(uvScale , float2x2(cos01, -sin01, sin01, cos01))  + float2( 0.5,0.5 );
                    half2 rotator01_uv = (rotator01 * 2.0 - 1.0);
                    half2 twirl = float2((atan2(rotator01_uv.y, rotator01_uv.x) / (2.0 * UNITY_PI) + 0.5), pow(length(rotator01_uv), _CeterScale));

                    uvScale = (twirl * float2(_UVScaleOffset.x, _UVScaleOffset.y) + float2(_UVScaleOffset.z , _UVScaleOffset.w));
                    uvScale -= 0.5 ;
                #endif

                uvScale += 0.5;

                //主贴图
                float UsualTime = _MainSpeedInt.w * _Time.g;

                half2 MainTexUV = lerp((uvScale + float2((_MainSpeedInt.x * UsualTime), (_MainSpeedInt.y*UsualTime))),(uvScale + i.customData1.zw), _MainUVCustom) ;
                
                //扰动图
                #ifdef _USENOISETEX_ON
                    half NoiseTime01 = UsualTime * _Noise01SpeedInt.w;
                    half4 NoiseTex01 = tex2D(_NoiseTex, i.uv2.zw + float2((_Noise01SpeedInt.x * NoiseTime01),(_Noise01SpeedInt.y * NoiseTime01)));
                    half NoiseTime02 = UsualTime * _Noise02Speed.w;
                    half4 NoiseTex02 = tex2D(_NoiseTex, i.uv2.zw + float2((_Noise02Speed.x * NoiseTime02),(_Noise02Speed.y * NoiseTime02)));
                    half NoiseTex_var = lerp(NoiseTex01.r,NoiseTex02.g + NoiseTex01.r,_DoubleNoise);

                    MainTexUV += NoiseTex_var * lerp(_Noise01SpeedInt.z, i.customData2.y, _MainNoiseIntCustom);
                #endif
            
                //主贴图采样
                half4 MainTex_var = tex2D(_MainTex,MainTexUV);

                half MianTexAlpha = lerp(MainTex_var.a,MainTex_var.r,_MainTexAlpha);
                MianTexAlpha *= step(lerp(_Clip,i.customData1.x,_ClipCustom),MianTexAlpha);

                half FullAlpha = MianTexAlpha * _MainColorBack.a;

                half3 FinalColorbacklerp = lerp(_MainColorBack02.rgb, _MainColorBack.rgb,MainTex_var.rgb);
                //half3 FinalColorfrontlerp = lerp(_MainColorBack02.rgb, _MainColorFront.rgb,MainTex_var.rgb);
                half3 FinalColor = lerp(_MainColorFront * FinalColorbacklerp,FinalColorbacklerp,facing) * _MainSpeedInt.z;   
                FinalColor *= MainTex_var.rgb;

                //遮罩图
                #ifdef _USEMASKTEX_ON
                    UsualTime *= _MaskSpeedInt.w;

                    half2 MaskTexUV = lerp((i.uv1.zw + float2((_MaskSpeedInt.x * UsualTime),(_MaskSpeedInt.y*UsualTime))), (i.uv1.zw + i.customData2.zw), _MaskUVCustom);
                    #ifdef _USENOISETEX_ON
                        MaskTexUV += NoiseTex_var * _MaskSpeedInt.z;
                    #endif
                    half4 MaskTex_var = tex2D(_MaskTex,MaskTexUV);
                    FullAlpha *= MaskTex_var;
                #endif

                //溶解图
                #ifdef _USEDISSOLVETEX_ON
                    UsualTime *= _DissolveSpeedInt.w;
                    half2 DissolveTexUV = (i.uv2.xy + float2((_DissolveSpeedInt.x * UsualTime),(_DissolveSpeedInt.y*UsualTime))) ;

                    #ifdef _USENOISETEX_ON
                        DissolveTexUV += NoiseTex_var * _DissolveSpeedInt.z;
                    #endif

                    half4 DissolveTex_var = tex2D(_DissolveTex,DissolveTexUV);

                    //软溶解
                    half ClampHardness = clamp(_hardness, 0.001, 0.999);
                    half dissolveInt = lerp((_dissolveInt * (_width + 1.0)), i.customData1.y, _dissolveCustom);
                    half smoothstepDissolve01 = smoothstep(ClampHardness, 1.0, (2.0 - (1.0 - DissolveTex_var.r + (dissolveInt * (2.0 - ClampHardness)))));
                    half smoothstepDissolve02 = smoothstep(ClampHardness, 1.0, (2.0 - (1.0 - DissolveTex_var.r + ((2.0 - ClampHardness) * (dissolveInt - _width )))));
                    //half d = smoothstepDissolve02 - smoothstepDissolve01;
                    FinalColor = lerp(_widthColor.rgb, FinalColor , smoothstepDissolve01);

                    FullAlpha *= smoothstepDissolve02;

                #endif
                
                //闪烁
                half ShanTime = _Time.y * _ShanPinLv;
                half MianTexShan = (abs(sin(ShanTime))) * (abs(cos(ShanTime))) * _ShanZhenFu;

                half MianTexShanLerp = FullAlpha * lerp(1, MianTexShan, _Shan);

                //汇总
                half3 Emission = (i.vertexColor.rgb * FinalColor ).rgb;
                
                half Alpha = MianTexShanLerp * i.vertexColor.a;

                return half4(Emission,Alpha);
            }
            ENDCG
        }
	}
	CustomEditor "NanoEffectShaderGUI"
}