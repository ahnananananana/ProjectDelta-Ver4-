using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HDV
{
    public class SafeAreaSetter : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private RectTransform panelSafeArea;
        private ScreenOrientation currentOrientation;
        private Rect currentSafeArea;

        void Awake()
        {
            panelSafeArea = GetComponent<RectTransform>();

            currentOrientation = Screen.orientation;
            currentSafeArea = Screen.safeArea;

            ApplySafeArea();
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;

            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            panelSafeArea.anchorMin = anchorMin;
            panelSafeArea.anchorMax = anchorMax;

            currentOrientation = Screen.orientation;
            currentSafeArea = Screen.safeArea;
        }

        void Update()
        {
            if (currentOrientation != Screen.orientation || currentSafeArea != Screen.safeArea)
                ApplySafeArea();
        }
    }

}