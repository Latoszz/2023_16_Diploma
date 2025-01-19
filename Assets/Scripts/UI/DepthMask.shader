Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags { "Queue" = "Overlay-1" }
        Pass
        {
            ZWrite On
            ColorMask 0
        }
    }
}