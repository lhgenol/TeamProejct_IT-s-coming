// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SceneBendReplacementWithColor"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }  // Default Material�� ���� Opaque��
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            // ����: z�� ���� x�� �̵����� 2�� �Լ��� ���
            // f(z) = 0.2 * z - 0.0175 * z^2
            float2 BendFunction(float z)
            {
                float shiftX = 0.2 * z - 0.005 * (z * z);// float shiftX = 0.2 * z - 0.0175 * (z * z);
                return float2(shiftX, 0);
            }

            v2f vert(appdata v)
            {
                v2f o;
                // ���� ��ǥ�� ���� ��ǥ�� ��ȯ
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // z���� ���� x�� ������ ����Ͽ� ����
                float2 shift = BendFunction(worldPos.z);
                worldPos.x += shift.x;

                // ���� ��ǥ�� Ŭ�� �������� ��ȯ
                o.vertex = mul(UNITY_MATRIX_VP, float4(worldPos, 1.0));
                // uv ��ǥ�� ���� �ؽ�ó ���ø��� ���
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ���� ��Ƽ������ _MainTex�� _Color�� �״�� ���
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}

