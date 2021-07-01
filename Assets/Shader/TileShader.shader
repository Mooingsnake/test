/*
����tile��ʾ��ǵ�shader����Ⱦ�����Ϸ�����ʾ������ߵĸ��ӣ�selectable tile������ɫ��
�α�ģʽ��ս��ģʽ����ѡ�еĸ����ǻ�ɫ��
�����߼����£�
moveMode ����ʾ���п�ѡ���ӣ���ɫ͸������ʱ���������(AlphaWave)
            ѡ�еĸ��ӣ�����ǰ�ٿص�����վ���ĸ��ӣ�����ɫƫ��ɫ��������
CursorMode �����п�ѡ���ӣ���ɫ͸������ʱ���������
            ѡ�еĸ��ӣ���ɫ͸��ƫ��������ѡ��texture
CombatMode ����ʾ���п�ѡ���ӣ���ɫ͸������ʱ���������
            ѡ�еĸ��ӣ���ɫ͸��ƫ��������ѡ��texture


*/

Shader "Unlit/TileShader"
{
    Properties
    {
        _AimTex ("Aim Texture", 2D) = "white" {} // ֻ����battle mode��ѡ�й���Ŀ���ʱ�������
       // _TileColor ("Tile Color", Color ) = (0, 0, 1, 1) //��û������ɫ���ȱ�¶���� 
        _ModeType ("Mode Type", Float) = 0 // move 1; cursor 2 ; combat 3
        _IsCurrent ("Is Current", Float) = 1
        _IsSelectable ("Ia Selectable", Float) = 0
    }
    SubShader
    {
        Tags {
             "RenderType" = "Transparent"
             "Queue" = "Transparent"
        }


        Pass
        {
            Blend  SrcAlpha OneMinusSrcAlpha // [SRC * A + DST * b]
            ZWrite Off
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            #define HIDE float4(1,1,1,0)
            #define GREEN float4(0.027, 0.623, 0.453, 0.7)
            #define YELLOW float4(0.849,0.751,0.220, 0.7)
            #define RED float4(0.433, 0.045, 0.034, 0.7)

            #define MOVEMODE 1
            #define CURSORMODE 2
            #define COMBATMODE 3
            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Interpolator
            {
                float2 uv1 : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _AimTex;
            float4 _MainTex_ST;
            float4 _TileColor;
            float4 finColor;
            float _ModeType;
            float _IsCurrent;
            float _IsSelectable;


            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = v.uv0;
                o.normal = v.normal;

                return o;
            }

            float AlphaWave() {
                return cos( _Time.y * 2 ) * 0.25 + 1;
            }



            fixed4 frag (Interpolator i) : SV_Target
            {
                // sample the texture
                 // float4 finColor = float4(0,0,0,0);

                if (_ModeType == CURSORMODE) {
                    finColor = YELLOW;
                }
               if (_ModeType == MOVEMODE) {
                    finColor = GREEN;    
                }
                 if (_ModeType == COMBATMODE) {
                    finColor = RED;
                }
                 // float4 finColor = _TileColor;
                if (_IsSelectable && !_IsCurrent) {
                    return finColor * AlphaWave() ;
                }
                if (_IsCurrent && _ModeType < 2) {
                    return finColor * 1.2f;
                }
                if (_IsCurrent && _ModeType >= 2) {
                  //  finColor = float4(i.uv1,1,1);
                  float pattern = tex2D(_AimTex, i.uv1).x;
                  finColor =   lerp(RED , YELLOW,pattern);
                    return finColor ;
                }
                return HIDE;

            }
            ENDCG
        }
    }
}
