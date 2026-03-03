Shader "Custom/BlackHole"
{
    Properties
    {
        // Skybox 6-sided textures
        [Header(Skybox 6 Sided)]
        _FrontTex ("Front (+Z)", 2D) = "white" {}
        _BackTex ("Back (-Z)", 2D) = "white" {}
        _LeftTex ("Left (+X)", 2D) = "white" {}
        _RightTex ("Right (-X)", 2D) = "white" {}
        _UpTex ("Up (+Y)", 2D) = "white" {}
        _DownTex ("Down (-Y)", 2D) = "white" {}
        _SkyboxExposure ("Skybox Exposure", Range(0, 2)) = 1.0
        
        // Alternative: Use Cubemap if preferred
        [Header(Alternative Cubemap)]
        _MainTex ("Skybox Cubemap (Optional)", Cube) = "white" {}
        [Toggle] _UseCubemap ("Use Cubemap Instead of 6-Sided", Float) = 0
        
        // Accretion Disk
        [Header(Accretion Disk)]
        _AccretionTex ("Accretion Disk Texture", 2D) = "white" {}
        _AccretionColor ("Accretion Disk Color", Color) = (1, 0.5, 0.1, 1)
        _AccretionIntensity ("Accretion Intensity", Range(0, 10)) = 3.0
        _AccretionSpeed ("Accretion Speed", Range(0, 5)) = 1.0
        _DiskThickness ("Disk Thickness", Range(0.01, 0.5)) = 0.15
        _InnerDiskRadius ("Inner Disk Radius", Range(0.5, 2.0)) = 1.2
        _OuterDiskRadius ("Outer Disk Radius", Range(2.0, 5.0)) = 3.0
        
        // Black Hole Properties
        [Header(Black Hole)]
        _EventHorizonRadius ("Event Horizon Radius", Range(0.1, 2.0)) = 0.5
        _GravitationalStrength ("Gravitational Strength", Range(0.0, 5.0)) = 2.0
        _LensingIterations ("Lensing Quality", Range(1, 10)) = 5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 objectPos : TEXCOORD2;
            };
            
            // 6-sided skybox textures
            sampler2D _FrontTex;
            sampler2D _BackTex;
            sampler2D _LeftTex;
            sampler2D _RightTex;
            sampler2D _UpTex;
            sampler2D _DownTex;
            float _SkyboxExposure;
            
            // Cubemap alternative
            samplerCUBE _MainTex;
            float _UseCubemap;
            
            // Accretion disk
            sampler2D _AccretionTex;
            float4 _AccretionColor;
            float _AccretionIntensity;
            float _AccretionSpeed;
            float _DiskThickness;
            float _InnerDiskRadius;
            float _OuterDiskRadius;
            
            // Black hole
            float _EventHorizonRadius;
            float _GravitationalStrength;
            int _LensingIterations;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.objectPos = v.vertex.xyz;
                o.viewDir = normalize(o.worldPos - _WorldSpaceCameraPos);
                return o;
            }
            
            // Sample 6-sided skybox based on direction
            float4 sampleSkybox6Sided(float3 direction)
            {
                float3 dir = normalize(direction);
                float3 absDir = abs(dir);
                float4 color;
                float2 uv;
                
                // Determine which face to sample
                if (absDir.x >= absDir.y && absDir.x >= absDir.z)
                {
                    // X-axis dominant
                    if (dir.x > 0.0)
                    {
                        // Right face (+X)
                        uv = float2(-dir.z / absDir.x, dir.y / absDir.x) * 0.5 + 0.5;
                        color = tex2D(_RightTex, uv);
                    }
                    else
                    {
                        // Left face (-X)
                        uv = float2(dir.z / absDir.x, dir.y / absDir.x) * 0.5 + 0.5;
                        color = tex2D(_LeftTex, uv);
                    }
                }
                else if (absDir.y >= absDir.x && absDir.y >= absDir.z)
                {
                    // Y-axis dominant
                    if (dir.y > 0.0)
                    {
                        // Up face (+Y)
                        uv = float2(dir.x / absDir.y, -dir.z / absDir.y) * 0.5 + 0.5;
                        color = tex2D(_UpTex, uv);
                    }
                    else
                    {
                        // Down face (-Y)
                        uv = float2(dir.x / absDir.y, dir.z / absDir.y) * 0.5 + 0.5;
                        color = tex2D(_DownTex, uv);
                    }
                }
                else
                {
                    // Z-axis dominant
                    if (dir.z > 0.0)
                    {
                        // Front face (+Z)
                        uv = float2(dir.x / absDir.z, dir.y / absDir.z) * 0.5 + 0.5;
                        color = tex2D(_FrontTex, uv);
                    }
                    else
                    {
                        // Back face (-Z)
                        uv = float2(-dir.x / absDir.z, dir.y / absDir.z) * 0.5 + 0.5;
                        color = tex2D(_BackTex, uv);
                    }
                }
                
                return color * _SkyboxExposure;
            }
            
            // Sample skybox (choose between 6-sided or cubemap)
            float4 sampleSkybox(float3 direction)
            {
                if (_UseCubemap > 0.5)
                {
                    return texCUBE(_MainTex, direction) * _SkyboxExposure;
                }
                else
                {
                    return sampleSkybox6Sided(direction);
                }
            }
            
            // Ray-sphere intersection
            bool intersectSphere(float3 rayOrigin, float3 rayDir, float3 sphereCenter, float radius, out float t)
            {
                float3 oc = rayOrigin - sphereCenter;
                float b = dot(oc, rayDir);
                float c = dot(oc, oc) - radius * radius;
                float discriminant = b * b - c;
                
                if (discriminant < 0.0)
                {
                    t = -1.0;
                    return false;
                }
                
                t = -b - sqrt(discriminant);
                return t > 0.0;
            }
            
            // Gravitational lensing effect
            float3 applyGravitationalLensing(float3 rayDir, float3 blackHolePos, float3 rayOrigin)
            {
                float3 toBlackHole = blackHolePos - rayOrigin;
                float dist = length(toBlackHole);
                float3 perpDir = toBlackHole / dist;
                
                // Calculate bending based on distance
                float bendFactor = _GravitationalStrength * _EventHorizonRadius / (dist * dist + 0.1);
                
                // Bend the ray towards the black hole
                float3 bentRay = normalize(rayDir + perpDir * bendFactor);
                return bentRay;
            }
            
            // Accretion disk calculation
            float4 calculateAccretionDisk(float3 rayOrigin, float3 rayDir, float3 blackHolePos)
            {
                // Disk is in XZ plane
                float3 diskNormal = float3(0, 1, 0);
                float denom = dot(rayDir, diskNormal);
                
                if (abs(denom) < 0.0001)
                    return float4(0, 0, 0, 0);
                
                float t = dot(blackHolePos - rayOrigin, diskNormal) / denom;
                
                if (t < 0.0)
                    return float4(0, 0, 0, 0);
                
                float3 hitPoint = rayOrigin + rayDir * t;
                float3 toCenter = hitPoint - blackHolePos;
                float distToCenter = length(toCenter);
                
                // Check if within disk bounds
                if (distToCenter < _InnerDiskRadius || distToCenter > _OuterDiskRadius)
                    return float4(0, 0, 0, 0);
                
                // Height check for disk thickness
                float heightFromDisk = abs(dot(hitPoint - blackHolePos, diskNormal));
                if (heightFromDisk > _DiskThickness)
                    return float4(0, 0, 0, 0);
                
                // Calculate rotation and texture coordinates
                float angle = atan2(toCenter.z, toCenter.x);
                float normalizedDist = (distToCenter - _InnerDiskRadius) / (_OuterDiskRadius - _InnerDiskRadius);
                
                // Rotating accretion disk
                float rotation = _Time.y * _AccretionSpeed;
                float2 diskUV = float2(angle / (3.14159 * 2.0) + rotation, normalizedDist);
                
                float4 diskTex = tex2D(_AccretionTex, diskUV);
                
                // Brightness based on distance (hotter near center)
                float brightness = 1.0 - normalizedDist;
                brightness = pow(brightness, 2.0);
                
                // Doppler shift effect (one side brighter)
                float dopplerEffect = sin(angle + rotation * 3.0) * 0.3 + 0.7;
                
                float4 diskColor = _AccretionColor * diskTex * brightness * _AccretionIntensity * dopplerEffect;
                diskColor.a = brightness * (1.0 - heightFromDisk / _DiskThickness);
                
                return diskColor;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                float3 blackHolePos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDir = normalize(i.worldPos - rayOrigin);
                
                // Apply gravitational lensing
                float3 bentRay = rayDir;
                for (int step = 0; step < _LensingIterations; step++)
                {
                    bentRay = applyGravitationalLensing(bentRay, blackHolePos, rayOrigin);
                }
                
                // Check if ray hits event horizon
                float t;
                bool hitEventHorizon = intersectSphere(rayOrigin, bentRay, blackHolePos, _EventHorizonRadius, t);
                
                if (hitEventHorizon)
                {
                    return float4(0, 0, 0, 1); // Pure black for event horizon
                }
                
                // Calculate accretion disk
                float4 diskColor = calculateAccretionDisk(rayOrigin, bentRay, blackHolePos);
                
                // Sample background with lensed ray using 6-sided skybox or cubemap
                float4 bgColor = sampleSkybox(bentRay);
                
                // Combine background and accretion disk
                float4 finalColor = lerp(bgColor, diskColor, diskColor.a);
                
                // Add photon sphere glow
                float distToCenter = length(i.objectPos);
                float photonSphere = 1.5 * _EventHorizonRadius;
                float glowDist = abs(distToCenter - photonSphere);
                float glow = exp(-glowDist * 10.0) * 0.3;
                finalColor.rgb += _AccretionColor.rgb * glow;
                
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}