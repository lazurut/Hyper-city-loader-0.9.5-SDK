Shader "Custom/HashTiling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 2)) = 1
        _Tiling ("Tiling", Float) = 1.0
        _Rotation ("Enable Rotation", Range(0, 1)) = 0
        _Color ("Base Color", Color) = (1,1,1,1)
        _ColorVariation ("Color Variation", Range(0, 1)) = 0
        _Brightness ("Brightness", Range(0, 2)) = 1
        _Contrast ("Contrast", Range(0, 2)) = 1
        _Saturation ("Saturation", Range(0, 2)) = 1
        _Hue ("Hue Shift", Range(-180, 180)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _NormalMap;
        float _NormalStrength;
        float _Tiling;
        float _Rotation;
        fixed4 _Color;
        float _ColorVariation;
        float _Brightness;
        float _Contrast;
        float _Saturation;
        float _Hue;

        struct Input
        {
            float2 uv_MainTex;
        };

        // Улучшенная hash функция
        float2 hash22(float2 p)
        {
            float3 p3 = frac(float3(p.xyx) * float3(0.1031, 0.1030, 0.0973));
            p3 += dot(p3, p3.yzx + 19.19);
            return frac((p3.xx + p3.yz) * p3.zy);
        }

        // Поворот точки на заданный угол
        float2 rotate(float2 p, float angle)
        {
            float c = cos(angle);
            float s = sin(angle);
            return float2(
                p.x * c - p.y * s,
                p.x * s + p.y * c
            );
        }

        // Поворот нормали
        float3 rotateNormal(float3 normal, float angle)
        {
            float c = cos(angle);
            float s = sin(angle);
            return float3(
                normal.x * c - normal.y * s,
                normal.x * s + normal.y * c,
                normal.z
            );
        }

        // Функция для конвертации RGB в HSV
        float3 rgb2hsv(float3 c)
        {
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
            float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

            float d = q.x - min(q.w, q.y);
            float e = 1.0e-10;
            return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
        }

        // Функция для конвертации HSV в RGB
        float3 hsv2rgb(float3 c)
        {
            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
            return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
        }

        // Функция для настройки цвета
        fixed3 AdjustColor(fixed3 color, float2 hash)
        {
            // Применяем базовый цвет
            color *= _Color.rgb;
            
            // Яркость
            color *= _Brightness;
            
            // Контраст
            color = (color - 0.5) * _Contrast + 0.5;
            
            // Конвертируем в HSV для работы с насыщенностью и оттенком
            float3 hsv = rgb2hsv(color);
            
            // Насыщенность
            hsv.y *= _Saturation;
            
            // Сдвиг оттенка
            hsv.x += _Hue / 360.0;
            hsv.x = frac(hsv.x); // Оборачиваем значение
            
            // Цветовые вариации для каждого тайла
            if (_ColorVariation > 0)
            {
                // Случайный сдвиг оттенка для каждого тайла
                float hueVariation = (hash.x - 0.5) * _ColorVariation * 0.3;
                hsv.x += hueVariation;
                hsv.x = frac(hsv.x);
                
                // Случайная вариация яркости
                float brightnessVariation = (hash.y - 0.5) * _ColorVariation * 0.2;
                hsv.z += brightnessVariation;
                hsv.z = saturate(hsv.z);
            }
            
            // Конвертируем обратно в RGB
            return hsv2rgb(hsv);
        }

        // Функция для сэмплирования текстуры с hash tiling
        fixed4 SampleHashTiling(sampler2D tex, float2 uv, float tiling, out float angle)
        {
            float2 scaledUV = uv * tiling;
            float2 tileID = floor(scaledUV);
            float2 localUV = frac(scaledUV);
            
            // Генерируем случайные параметры для тайла
            float2 hash = hash22(tileID);
            
            // Случайное смещение
            float2 offset = hash;
            
            // Случайный поворот (если включен)
            angle = 0;
            if (_Rotation > 0.5)
            {
                angle = hash.x * 6.28318; // 2π
                localUV = rotate(localUV - 0.5, angle) + 0.5;
            }
            
            // Применяем смещение
            float2 finalUV = frac(localUV + offset);
            
            // Семплируем текстуру
            fixed4 texColor = tex2D(tex, finalUV);
            
            // Применяем цветовые настройки
            texColor.rgb = AdjustColor(texColor.rgb, hash);
            
            return texColor;
        }

        // Функция для сэмплирования normal map с hash tiling
        float3 SampleNormalHashTiling(sampler2D normalTex, float2 uv, float tiling, float angle)
        {
            float2 scaledUV = uv * tiling;
            float2 tileID = floor(scaledUV);
            float2 localUV = frac(scaledUV);
            
            // Генерируем случайные параметры для тайла
            float2 hash = hash22(tileID);
            
            // Случайное смещение
            float2 offset = hash;
            
            // Случайный поворот (если включен)
            if (_Rotation > 0.5)
            {
                localUV = rotate(localUV - 0.5, angle) + 0.5;
            }
            
            // Применяем смещение
            float2 finalUV = frac(localUV + offset);
            
            // Семплируем normal map
            float3 normal = UnpackNormal(tex2D(normalTex, finalUV));
            
            // Применяем силу нормалей
            normal.xy *= _NormalStrength;
            
            // Поворачиваем нормаль, если был поворот UV
            if (_Rotation > 0.5)
            {
                normal = rotateNormal(normal, angle);
            }
            
            return normalize(normal);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float angle;
            fixed4 c = SampleHashTiling(_MainTex, IN.uv_MainTex, _Tiling, angle);
            float3 normal = SampleNormalHashTiling(_NormalMap, IN.uv_MainTex, _Tiling, angle);
            
            o.Albedo = c.rgb;
            o.Normal = normal;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}