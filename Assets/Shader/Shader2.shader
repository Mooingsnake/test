/*
[����texture���]
��ã�������һ��shader�����̵̳ıʼǱ�������Ŀ�г��ô��ڵ���ʵ�Ǹ������ıʼǣ�û�й�������
1._MainTex_ST ��
    ����ı�ʾ��MainTex��Scale��transform����Ȼû����д�����ǵ�ȷ��Ӧ�˱༭���ﱩ¶������ֵ��Tiling Offset
2.tex2D��sampler��i.uv����texture�е�һ������ɫ��sampler��ʾ������texture��i.uv��ʾtexture������
3.��ˮ��ͼ����������
    o.uv.x += _Time.y * 0.1;
4.�������꣺
    Լ��float4(x,y,z,w)��ǰ�����ǿռ����꣬w = 0 �������Ǹ��������߷���w = 1�������Ǹ�pos��˵����offset
5.unity��ʹ���ɰ漼��
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
            float4 _MainTex_ST; // ��ʾ��MainTex��Scale��transform


            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.worldPos = mul( UNITY_MATRIX_M, v.vertex); // unity_ObjectToWorldҲ����д��UNITY_MATRIX_M
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
