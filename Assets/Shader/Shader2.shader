/*
[二、texture相关]
你好，这里是一个shader基础教程的笔记本，在项目中长久存在但其实是个独立的笔记，没有工程作用
1._MainTex_ST ，
    神奇的表示了MainTex的Scale和transform。虽然没有明写，但是的确对应了编辑器里暴露的两个值：Tiling Offset
2.tex2D（sampler，i.uv）把texture中的一像素颜色，sampler表示采样的texture，i.uv表示texture的坐标
3.让水贴图流动起来：
    o.uv.x += _Time.y * 0.1;
4.世界坐标：
    约定float4(x,y,z,w)中前三个是空间坐标，w = 0 代表这是个向量或者方向，w = 1代表这是个pos，说明是offset
5.unity里使用蒙版技术
*/

Shader "Unlit/Shader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pattern ("Pattern", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _Pattern;
            float4 _MainTex_ST; // 表示了MainTex的Scale和transform


            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.worldPos = mul( UNITY_MATRIX_M, v.vertex); // unity_ObjectToWorld也可以写成UNITY_MATRIX_M
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
 
                return o;
            }

            fixed4 frag (Interpolator i) : SV_Target
            {
                // sample the texture
             //   fixed4 col = tex2D(_MainTex, i.uv);
                float2 topDownProjection = i.worldPos;
                float4 baseColor = tex2D( _MainTex, topDownProjection);
                float pattern = tex2D(_Pattern, i.uv).x;

                float4 finalColor = lerp( float4(1,0,0,1), baseColor, pattern);
                return finalColor;
            }
            ENDCG
        }
    }
}
