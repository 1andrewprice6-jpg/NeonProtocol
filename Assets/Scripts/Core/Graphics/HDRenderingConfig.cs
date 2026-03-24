using UnityEngine;

namespace NeonProtocol.Graphics
{
    /// <summary>
    /// Centralized HD quality settings management for NeonProtocol.
    /// Configures post-processing, lighting, rendering quality, and HDR settings.
    /// </summary>
    public class HDRenderingConfig : MonoBehaviour
    {
        [Header("Post Processing")]
        [SerializeField] private float bloomIntensity = 1.5f;
        [SerializeField] private float bloomThreshold = 0.9f;
        [SerializeField] private float bloomScatter = 0.7f;

        [SerializeField] private float dofFocalLength = 50f;
        [SerializeField] private float dofAperture = 8f;
        [SerializeField] private float dofFocusDistance = 10f;

        [SerializeField] private float motionBlurShutterAngle = 270f;
        [SerializeField] private int motionBlurSampleCount = 8;

        [Header("Lighting")]
        [SerializeField] private float ambientIntensity = 1.2f;
        [SerializeField] private float reflectionIntensity = 1.0f;
        [SerializeField] private float shadowDistance = 150f;
        [SerializeField] private int shadowCascades = 4;

        [Header("Quality")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private AntiAliasing antiAliasing = AntiAliasing.MSAA4x;
        [SerializeField] private bool realtimeReflectionProbes = true;

        [Header("HDR")]
        [SerializeField] private bool hdrEnabled = true;
        [SerializeField] private float renderScale = 1.0f;
        [SerializeField] private bool allowHDR = true;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            ApplyQualitySettings();
            ApplyHDRSettings();
            Debug.Log("[HDRenderingConfig] HD rendering settings applied successfully.");
        }

        /// <summary>
        /// Applies quality settings to the rendering pipeline.
        /// </summary>
        private void ApplyQualitySettings()
        {
            // Frame rate
            Application.targetFrameRate = targetFrameRate;

            // Anisotropic filtering
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;

            // Ambient intensity
            RenderSettings.ambientIntensity = ambientIntensity;

            // Reflection intensity
            RenderSettings.reflectionIntensity = reflectionIntensity;

            // Shadow settings
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowCascades = shadowCascades;
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;

            // Real-time reflection probes
            QualitySettings.realtimeReflectionProbes = realtimeReflectionProbes;

            Debug.Log($"[HDRenderingConfig] Quality settings applied: {targetFrameRate}fps, Cascades: {shadowCascades}, Shadow Distance: {shadowDistance}");
        }

        /// <summary>
        /// Configures HDR and post-processing on the main camera.
        /// </summary>
        private void ApplyHDRSettings()
        {
            if (_mainCamera == null) return;

            _mainCamera.allowHDR = allowHDR;
            _mainCamera.renderingPath = RenderingPath.Universal;

            Debug.Log($"[HDRenderingConfig] HDR Settings Applied - HDR Enabled: {allowHDR}, Render Scale: {renderScale}");
        }

        /// <summary>
        /// Retrieves the post-processing profile configuration.
        /// </summary>
        /// <returns>A PostProcessProfile struct containing all post-processing settings.</returns>
        public PostProcessProfile GetPostProcessProfile()
        {
            return new PostProcessProfile
            {
                bloomIntensity = bloomIntensity,
                bloomThreshold = bloomThreshold,
                bloomScatter = bloomScatter,
                dofFocalLength = dofFocalLength,
                dofAperture = dofAperture,
                dofFocusDistance = dofFocusDistance,
                motionBlurShutterAngle = motionBlurShutterAngle,
                motionBlurSampleCount = motionBlurSampleCount
            };
        }

        /// <summary>
        /// Struct containing post-processing configuration data.
        /// </summary>
        public struct PostProcessProfile
        {
            public float bloomIntensity;
            public float bloomThreshold;
            public float bloomScatter;
            public float dofFocalLength;
            public float dofAperture;
            public float dofFocusDistance;
            public float motionBlurShutterAngle;
            public int motionBlurSampleCount;
        }

        public enum AntiAliasing
        {
            Disabled,
            MSAA2x,
            MSAA4x,
            MSAA8x
        }
    }
}
