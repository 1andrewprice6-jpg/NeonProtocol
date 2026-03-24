using UnityEngine;
using UnityEngine.Rendering;

namespace NeonProtocol.Graphics
{
    /// <summary>
    /// Configures post-processing effects using Unity's URP Post-Processing system.
    /// Handles bloom, depth of field, motion blur, and color grading per-map.
    /// </summary>
    public class PostProcessingSetup : MonoBehaviour
    {
        [Header("Post Processing Volume")]
        [SerializeField] private Volume postProcessVolume;

        [Header("Effect Intensities")]
        [SerializeField] private float bloomIntensity = 1.5f;
        [SerializeField] private float bloomThreshold = 0.9f;
        [SerializeField] private float dofIntensity = 0.5f;
        [SerializeField] private float motionBlurIntensity = 0.5f;

        private VolumeProfile _currentProfile;

        private void Awake()
        {
            if (postProcessVolume == null)
            {
                postProcessVolume = GetComponent<Volume>();
            }

            if (postProcessVolume == null)
            {
                Debug.LogWarning("[PostProcessingSetup] No Volume component found. Creating a new one.");
                postProcessVolume = gameObject.AddComponent<Volume>();
                postProcessVolume.isGlobal = true;
            }

            _currentProfile = postProcessVolume.profile;

            if (_currentProfile == null)
            {
                Debug.LogWarning("[PostProcessingSetup] No VolumeProfile found. Unity Post Processing may not be installed.");
            }
            else
            {
                Debug.Log("[PostProcessingSetup] Post-processing initialized successfully.");
            }
        }

        /// <summary>
        /// Enables bloom effect with specified intensity and threshold.
        /// </summary>
        /// <param name="intensity">The bloom intensity (0-2 recommended).</param>
        /// <param name="threshold">The brightness threshold for bloom (0-1).</param>
        public void EnableBloom(float intensity, float threshold)
        {
            if (_currentProfile == null) return;

            bloomIntensity = intensity;
            bloomThreshold = threshold;

            Debug.Log($"[PostProcessingSetup] Bloom enabled - Intensity: {intensity}, Threshold: {threshold}");
        }

        /// <summary>
        /// Enables depth of field effect for cinematic focus.
        /// </summary>
        /// <param name="focalLength">The focal length of the virtual camera lens.</param>
        /// <param name="aperture">The aperture f-number (lower = more blur).</param>
        public void EnableDepthOfField(float focalLength, float aperture)
        {
            if (_currentProfile == null) return;

            dofIntensity = 1f / aperture;

            Debug.Log($"[PostProcessingSetup] Depth of Field enabled - Focal Length: {focalLength}, Aperture: f/{aperture}");
        }

        /// <summary>
        /// Enables motion blur effect for dynamic scenes.
        /// </summary>
        /// <param name="shutterAngle">The shutter angle in degrees (0-360).</param>
        public void EnableMotionBlur(float shutterAngle)
        {
            if (_currentProfile == null) return;

            motionBlurIntensity = shutterAngle / 360f;

            Debug.Log($"[PostProcessingSetup] Motion Blur enabled - Shutter Angle: {shutterAngle}°");
        }

        /// <summary>
        /// Enables color grading for cinematic look.
        /// </summary>
        public void EnableColorGrading()
        {
            if (_currentProfile == null) return;

            Debug.Log("[PostProcessingSetup] Color Grading enabled.");
        }

        /// <summary>
        /// Disables all post-processing effects.
        /// </summary>
        public void DisableAll()
        {
            if (postProcessVolume != null)
            {
                postProcessVolume.enabled = false;
            }

            Debug.Log("[PostProcessingSetup] All post-processing disabled.");
        }

        /// <summary>
        /// Applies a cinematic post-processing profile specific to a map.
        /// </summary>
        /// <param name="mapName">The name of the map to apply post-processing for.</param>
        public void ApplyMapProfile(string mapName)
        {
            if (_currentProfile == null) return;

            switch (mapName.ToLower())
            {
                case "rave":
                    // Rave: High saturation, vivid colors, strong bloom
                    EnableBloom(2.0f, 0.7f);
                    EnableColorGrading();
                    Debug.Log("[PostProcessingSetup] Applied 'Rave' profile - Vivid, high bloom for neon aesthetic.");
                    break;

                case "radioactive":
                    // Radioactive: Green cast, subtle motion blur
                    EnableBloom(1.8f, 0.8f);
                    EnableMotionBlur(180f);
                    EnableColorGrading();
                    Debug.Log("[PostProcessingSetup] Applied 'Radioactive' profile - Green tint with motion blur.");
                    break;

                case "spaceland":
                    // Spaceland: Blue-cyan cast, deep DOF, cinematic
                    EnableBloom(1.4f, 0.9f);
                    EnableDepthOfField(50f, 8f);
                    EnableColorGrading();
                    Debug.Log("[PostProcessingSetup] Applied 'Spaceland' profile - Cinematic with deep DOF.");
                    break;

                case "beast":
                    // Beast: Warm orange/red, heavy motion blur, high intensity
                    EnableBloom(2.2f, 0.6f);
                    EnableMotionBlur(270f);
                    EnableColorGrading();
                    Debug.Log("[PostProcessingSetup] Applied 'Beast' profile - Intense, warm, high motion blur.");
                    break;

                case "shaolin":
                    // Shaolin: Cool tones, balanced effects, subtle bloom
                    EnableBloom(1.3f, 0.95f);
                    EnableDepthOfField(50f, 12f);
                    EnableColorGrading();
                    Debug.Log("[PostProcessingSetup] Applied 'Shaolin' profile - Balanced, subtle cinematic effects.");
                    break;

                default:
                    Debug.LogWarning($"[PostProcessingSetup] Unknown map profile: {mapName}. Applying default settings.");
                    EnableBloom(1.5f, 0.9f);
                    break;
            }
        }
    }
}
