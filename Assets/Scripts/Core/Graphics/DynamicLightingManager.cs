using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeonProtocol.Graphics
{
    /// <summary>
    /// Singleton manager for dynamic neon lighting effects across maps.
    /// Handles light flickering, pulsing, and per-map lighting profiles.
    /// </summary>
    public class DynamicLightingManager : MonoBehaviour
    {
        public static DynamicLightingManager Instance { get; private set; }

        [Header("Neon Colors")]
        [SerializeField] private Color[] neonLightColors = new Color[]
        {
            new Color(0, 1, 1),        // Cyan
            new Color(1, 0, 1),        // Magenta
            new Color(1, 1, 0),        // Yellow
            new Color(0, 1, 0),        // Green
            new Color(1, 0, 0)         // Red
        };

        [Header("Flicker Settings")]
        [SerializeField] private float flickerSpeed = 10f;
        [SerializeField] private float flickerIntensityMin = 0.5f;
        [SerializeField] private float flickerIntensityMax = 1.5f;

        [Header("Pulse Settings")]
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private float pulseIntensityMin = 0.8f;
        [SerializeField] private float pulseIntensityMax = 1.5f;

        private Dictionary<Light, Coroutine> _flickeringLights = new Dictionary<Light, Coroutine>();
        private List<Light> _allNeonLights = new List<Light>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Sets the ambient color to a neon shade for the current environment.
        /// </summary>
        /// <param name="color">The neon color to apply as ambient lighting.</param>
        public void SetAmbientNeon(Color color)
        {
            RenderSettings.ambientLight = color;
            Debug.Log($"[DynamicLightingManager] Ambient color set to: {color}");
        }

        /// <summary>
        /// Adds a Light to the neon lighting system with a specified color.
        /// </summary>
        /// <param name="light">The Light component to add.</param>
        /// <param name="color">The neon color for this light.</param>
        public void AddNeonLight(Light light, Color color)
        {
            if (light == null) return;

            light.color = color;
            light.intensity = 1.5f;
            _allNeonLights.Add(light);

            Debug.Log($"[DynamicLightingManager] Added neon light: {light.gameObject.name} with color {color}");
        }

        /// <summary>
        /// Starts a flickering effect on a specific Light.
        /// </summary>
        /// <param name="light">The Light to flicker.</param>
        public void StartFlicker(Light light)
        {
            if (light == null) return;

            if (_flickeringLights.ContainsKey(light))
            {
                StopCoroutine(_flickeringLights[light]);
            }

            Coroutine flicker = StartCoroutine(FlickerCoroutine(light));
            _flickeringLights[light] = flicker;

            Debug.Log($"[DynamicLightingManager] Started flickering on: {light.gameObject.name}");
        }

        /// <summary>
        /// Stops the flickering effect on a specific Light.
        /// </summary>
        /// <param name="light">The Light to stop flickering.</param>
        public void StopFlicker(Light light)
        {
            if (_flickeringLights.ContainsKey(light))
            {
                StopCoroutine(_flickeringLights[light]);
                _flickeringLights.Remove(light);
                Debug.Log($"[DynamicLightingManager] Stopped flickering on: {light.gameObject.name}");
            }
        }

        /// <summary>
        /// Pulses all neon lights in the scene with a smooth intensity wave.
        /// </summary>
        public void PulseAllLights()
        {
            StartCoroutine(PulseCoroutine());
        }

        /// <summary>
        /// Sets the lighting profile for a specific map.
        /// </summary>
        /// <param name="mapName">The name of the map to apply lighting for.</param>
        public void SetMapLightingProfile(string mapName)
        {
            switch (mapName.ToLower())
            {
                case "rave":
                    SetAmbientNeon(new Color(0.3f, 0, 0.5f)); // Deep purple
                    foreach (var light in _allNeonLights)
                    {
                        light.color = new Color(0, 1, 1); // Cyan
                        light.intensity = 1.8f;
                    }
                    break;

                case "radioactive":
                    SetAmbientNeon(new Color(0.2f, 0.4f, 0)); // Dark green
                    foreach (var light in _allNeonLights)
                    {
                        light.color = new Color(0, 1, 0); // Bright green
                        light.intensity = 1.6f;
                    }
                    break;

                case "spaceland":
                    SetAmbientNeon(new Color(0, 0.1f, 0.3f)); // Dark blue
                    foreach (var light in _allNeonLights)
                    {
                        light.color = new Color(0.5f, 1, 1); // Cyan-white
                        light.intensity = 1.4f;
                    }
                    break;

                case "beast":
                    SetAmbientNeon(new Color(0.3f, 0.1f, 0)); // Dark red-orange
                    foreach (var light in _allNeonLights)
                    {
                        light.color = new Color(1, 0.3f, 0); // Deep orange
                        light.intensity = 1.7f;
                    }
                    break;

                case "shaolin":
                    SetAmbientNeon(new Color(0.1f, 0.2f, 0.1f)); // Dark muted green
                    foreach (var light in _allNeonLights)
                    {
                        light.color = new Color(1, 1, 0); // Yellow
                        light.intensity = 1.5f;
                    }
                    break;

                default:
                    Debug.LogWarning($"[DynamicLightingManager] Unknown map: {mapName}");
                    break;
            }

            Debug.Log($"[DynamicLightingManager] Applied lighting profile for map: {mapName}");
        }

        /// <summary>
        /// Coroutine that creates a flickering effect on a light.
        /// </summary>
        private IEnumerator FlickerCoroutine(Light light)
        {
            float originalIntensity = light.intensity;

            while (light != null)
            {
                float randomIntensity = Random.Range(flickerIntensityMin, flickerIntensityMax);
                float elapsed = 0f;
                float duration = 1f / flickerSpeed;

                while (elapsed < duration && light != null)
                {
                    elapsed += Time.deltaTime;
                    light.intensity = Mathf.Lerp(originalIntensity, randomIntensity, elapsed / duration);
                    yield return null;
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Coroutine that pulses all lights with a smooth wave effect.
        /// </summary>
        private IEnumerator PulseCoroutine()
        {
            float elapsed = 0f;

            while (true)
            {
                elapsed += Time.deltaTime * pulseSpeed;
                float pulseFactor = Mathf.Lerp(pulseIntensityMin, pulseIntensityMax, (Mathf.Sin(elapsed) + 1f) / 2f);

                foreach (var light in _allNeonLights)
                {
                    if (light != null)
                    {
                        light.intensity = pulseFactor;
                    }
                }

                yield return null;
            }
        }
    }
}
