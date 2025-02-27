using UnityEngine;

namespace SupermarketTogetherKacker.menu
{
    public class FPSBoostCrap
    {
        private static int originalQualityLevel;
        private static ShadowQuality originalShadows;
        private static int originalTextureMipmapLimit;
        private static int originalAntiAliasing;
        private static float originalShadowDistance;
        private static bool originalRealtimeReflections;
        private static bool originalSoftParticles;
        private static float originalRenderScaleX;
        private static float originalRenderScaleY;

        public static bool fpsBoost; 
        
        public static void FPSBoost()
        {
            if (!fpsBoost)
            {
                // Save Original
                originalQualityLevel = QualitySettings.GetQualityLevel();
                originalShadows = QualitySettings.shadows;
                originalTextureMipmapLimit = QualitySettings.globalTextureMipmapLimit;
                originalAntiAliasing = QualitySettings.antiAliasing;
                originalShadowDistance = QualitySettings.shadowDistance;
                originalRealtimeReflections = QualitySettings.realtimeReflectionProbes;
                originalSoftParticles = QualitySettings.softParticles;
                originalRenderScaleX = ScalableBufferManager.widthScaleFactor;
                originalRenderScaleY = ScalableBufferManager.heightScaleFactor;
                
                // Set Settings
                QualitySettings.SetQualityLevel(0, true);
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.globalTextureMipmapLimit = int.MaxValue;
                QualitySettings.antiAliasing = 0;
                QualitySettings.realtimeReflectionProbes = false;
                QualitySettings.softParticles = false;
                QualitySettings.shadowDistance = float.MinValue;
                ScalableBufferManager.ResizeBuffers(float.MinValue, float.MinValue);
            }
            else
            {
                QualitySettings.SetQualityLevel(originalQualityLevel, true);
                QualitySettings.shadows = originalShadows;
                QualitySettings.globalTextureMipmapLimit = originalTextureMipmapLimit;
                QualitySettings.antiAliasing = originalAntiAliasing;
                QualitySettings.shadowDistance = originalShadowDistance;
                QualitySettings.realtimeReflectionProbes = originalRealtimeReflections;
                QualitySettings.softParticles = originalSoftParticles;
                ScalableBufferManager.ResizeBuffers(originalRenderScaleX, originalRenderScaleY);
            }
        }
    }
}