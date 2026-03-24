using UnityEngine;

namespace NeonProtocol.Graphics
{
    /// <summary>
    /// Controls global shader properties and keywords for HD rendering mode.
    /// Manages cinematic post-processing shader uniforms for neon aesthetics.
    /// </summary>
    public class ShaderQualityController : MonoBehaviour
    {
        [Header("HD Shader Properties")]
        [SerializeField] private float hdBloomIntensity = 1.5f;
        [SerializeField] private float neonGlowStrength = 2.0f;
        [SerializeField] private float chromaticAberrationAmount = 0.02f;
        [SerializeField] private float vignetteIntensity = 0.3f;

        [Header("Shader Keywords")]
        [SerializeField] private bool enableHDMode = true;
        [SerializeField] private bool enableChromaticAberration = true;
        [SerializeField] private bool enableVignette = true;

        private bool _hdKeywordsEnabled = false;

        private const string HD_KEYWORD = "HD_MODE_ENABLED";
        private const string CHROMA_KEYWORD = "CHROMATIC_ABERRATION_ON";
        private const string VIGNETTE_KEYWORD = "VIGNETTE_ON";

        private void Awake()
        {
            if (enableHDMode)
            {
                ApplyHDProfile();
            }
        }

        /// <summary>
        /// Enables all HD rendering shader keywords globally.
        /// </summary>
        public void EnableHDKeywords()
        {
            if (_hdKeywordsEnabled) return;

            Shader.EnableKeyword(HD_KEYWORD);

            if (enableChromaticAberration)
                Shader.EnableKeyword(CHROMA_KEYWORD);

            if (enableVignette)
                Shader.EnableKeyword(VIGNETTE_KEYWORD);

            _hdKeywordsEnabled = true;

            Debug.Log("[ShaderQualityController] HD shader keywords enabled.");
        }

        /// <summary>
        /// Disables all HD rendering shader keywords globally.
        /// </summary>
        public void DisableHDKeywords()
        {
            if (!_hdKeywordsEnabled) return;

            Shader.DisableKeyword(HD_KEYWORD);
            Shader.DisableKeyword(CHROMA_KEYWORD);
            Shader.DisableKeyword(VIGNETTE_KEYWORD);

            _hdKeywordsEnabled = false;

            Debug.Log("[ShaderQualityController] HD shader keywords disabled.");
        }

        /// <summary>
        /// Sets all global shader properties to cinematic HD values.
        /// </summary>
        public void ApplyHDProfile()
        {
            EnableHDKeywords();

            // Set global shader floats
            Shader.SetGlobalFloat("_HDBloomIntensity", hdBloomIntensity);
            Shader.SetGlobalFloat("_NeonGlowStrength", neonGlowStrength);
            Shader.SetGlobalFloat("_ChromaticAberrationAmount", chromaticAberrationAmount);
            Shader.SetGlobalFloat("_VignetteIntensity", vignetteIntensity);

            // Set global shader colors for neon effects
            Shader.SetGlobalColor("_NeonPrimaryColor", new Color(0, 1, 1)); // Cyan
            Shader.SetGlobalColor("_NeonSecondaryColor", new Color(1, 0, 1)); // Magenta
            Shader.SetGlobalColor("_NeonAccentColor", new Color(1, 1, 0)); // Yellow

            // Set global vector for advanced effects
            Shader.SetGlobalVector("_HDPostProcessParams",
                new Vector4(hdBloomIntensity, neonGlowStrength, chromaticAberrationAmount, vignetteIntensity));

            Debug.Log("[ShaderQualityController] HD shader profile applied with cinematic settings.");
            Debug.Log($"  - Bloom Intensity: {hdBloomIntensity}");
            Debug.Log($"  - Neon Glow Strength: {neonGlowStrength}");
            Debug.Log($"  - Chromatic Aberration: {chromaticAberrationAmount}");
            Debug.Log($"  - Vignette Intensity: {vignetteIntensity}");
        }

        /// <summary>
        /// Updates a specific HD shader property at runtime.
        /// </summary>
        /// <param name="propertyName">The name of the shader property (e.g., "_HDBloomIntensity").</param>
        /// <param name="value">The float value to set.</param>
        public void SetShaderProperty(string propertyName, float value)
        {
            Shader.SetGlobalFloat(propertyName, value);
            Debug.Log($"[ShaderQualityController] Set shader property '{propertyName}' to {value}");
        }

        /// <summary>
        /// Updates a specific HD shader color property at runtime.
        /// </summary>
        /// <param name="propertyName">The name of the shader property (e.g., "_NeonPrimaryColor").</param>
        /// <param name="color">The color value to set.</param>
        public void SetShaderColor(string propertyName, Color color)
        {
            Shader.SetGlobalColor(propertyName, color);
            Debug.Log($"[ShaderQualityController] Set shader color '{propertyName}' to {color}");
        }

        /// <summary>
        /// Gets the current HD bloom intensity setting.
        /// </summary>
        /// <returns>The bloom intensity value.</returns>
        public float GetBloomIntensity()
        {
            return hdBloomIntensity;
        }

        /// <summary>
        /// Gets the current neon glow strength setting.
        /// </summary>
        /// <returns>The glow strength value.</returns>
        public float GetGlowStrength()
        {
            return neonGlowStrength;
        }
    }
}
