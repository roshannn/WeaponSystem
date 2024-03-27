using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class AnimationsImporter : AssetPostprocessor
{
    private const string TargetDirectory = "Assets/Animations/Clips";
    private const string AvatarPath = "Assets/Models/Characters/Remy.fbx";
    private const string AvatarMaskPath = "Assets/Animations/Masks/CharacterAnimationMask.mask";
    private void OnPreprocessModel() {
        if (assetPath.StartsWith(TargetDirectory)) {
            Debug.Log($"Preprocessing Model : {assetPath}");
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if(modelImporter.animationType != ModelImporterAnimationType.Human) {
                modelImporter.animationType = ModelImporterAnimationType.Human;
            } 
        }
    }

    private void OnPostprocessModel(GameObject gameObject) {
        Debug.Log($"PostProcessing Model: {assetPath}");
        if (assetPath.StartsWith(TargetDirectory)){
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if(modelImporter.animationType == ModelImporterAnimationType.Human) {
                modelImporter.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
                modelImporter.sourceAvatar = AssetDatabase.LoadAssetAtPath<Avatar>(AvatarPath);
            }
        } 
    }

    private void OnPreprocessAnimation() {
        // Ensure this code runs only for assets in the specified directory and for ModelImporter types
        if (assetPath.StartsWith(TargetDirectory) && assetImporter is ModelImporter modelImporter) {
            modelImporter.motionNodeName = "<Root Transform>";
            // Debug message to indicate preprocessing has started
            Debug.Log("OnPreprocessAnimation");

            // Directly cast the array to a List<ModelImporterClipAnimation> for easier manipulation
            var clipAnimations = modelImporter.defaultClipAnimations.ToList();

            if (clipAnimations.Any()) { // Check if there are any clip animations
                Debug.Log("Setting clips");

                // Load the AvatarMask once outside the loop to improve performance
                AvatarMask animationAvatarMask = AssetDatabase.LoadAssetAtPath<AvatarMask>(AvatarMaskPath);

                // Assuming TakeInfo is relevant and available; you might need to handle cases where this is not true
                var takeInfo = modelImporter.importedTakeInfos.FirstOrDefault(); // Safely attempt to get the first TakeInfo
                foreach (var clip in clipAnimations) {
                    clip.maskType = ClipAnimationMaskType.CopyFromOther;
                    clip.maskSource = animationAvatarMask;
                    clip.firstFrame = takeInfo.bakeStartTime * takeInfo.sampleRate;
                    clip.lastFrame = takeInfo.bakeStopTime * takeInfo.sampleRate;
                }

                // Apply the modified clip animations back to the modelImporter
                modelImporter.clipAnimations = clipAnimations.ToArray();
            }
        }
    }
    private void OnPostprocessAnimation(GameObject root, AnimationClip clip) {
        
        clip.name = root.name;


    }
}
