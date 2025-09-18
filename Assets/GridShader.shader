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
                // 월드 좌표를 기준으로 소수점 부분만 가져옵니다.
                // 바닥이 XY 평면에 있다면 worldPos.xy를, XZ 평면에 있다면 worldPos.xz를 사용하세요.
                float2 grid = frac(i.worldPos.xy);

                // 왼쪽/오른쪽 라인 계산
                // grid.x가 0에 가깝거나(왼쪽), 1에 가까우면(오른쪽) 1을 반환
                float lineX = step(grid.x, _HorizontalThickness) + step(1.0 - _HorizontalThickness, grid.x);

                // 아래/위 라인 계산
                // grid.y가 0에 가깝거나(아래), 1에 가까우면(위) 1을 반환
                float lineY = step(grid.y, _VerticalThickness) + step(1.0 - _VerticalThickness, grid.y);
                
                // 가로선이나 세로선 중 하나라도 해당되면 1을 반환합니다.
                float lineFactor = saturate(lineX + lineY);

                // lineFactor 값에 따라 기본색과 그리드색을 보간합니다.
                fixed4 color = lerp(_BaseColor, _GridColor, lineFactor);
                return color;
            }
            ENDCG
        }
    }
}