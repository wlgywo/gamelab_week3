Shader "Unlit/GridShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _GridColor ("Grid Color", Color) = (0,0,0,1)
        _HorizontalThickness ("Horizontal Thickness", Range(0, 0.5)) = 0.01
        _VerticalThickness ("Vertical Thickness", Range(0, 0.5)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _GridColor;
            float _HorizontalThickness;
            float _VerticalThickness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // ���� ��ǥ�� �������� �Ҽ��� �κи� �����ɴϴ�.
                // �ٴ��� XY ��鿡 �ִٸ� worldPos.xy��, XZ ��鿡 �ִٸ� worldPos.xz�� ����ϼ���.
                float2 grid = frac(i.worldPos.xy);

                // ����/������ ���� ���
                // grid.x�� 0�� �����ų�(����), 1�� ������(������) 1�� ��ȯ
                float lineX = step(grid.x, _HorizontalThickness) + step(1.0 - _HorizontalThickness, grid.x);

                // �Ʒ�/�� ���� ���
                // grid.y�� 0�� �����ų�(�Ʒ�), 1�� ������(��) 1�� ��ȯ
                float lineY = step(grid.y, _VerticalThickness) + step(1.0 - _VerticalThickness, grid.y);
                
                // ���μ��̳� ���μ� �� �ϳ��� �ش�Ǹ� 1�� ��ȯ�մϴ�.
                float lineFactor = saturate(lineX + lineY);

                // lineFactor ���� ���� �⺻���� �׸������ �����մϴ�.
                fixed4 color = lerp(_BaseColor, _GridColor, lineFactor);
                return color;
            }
            ENDCG
        }
    }
}