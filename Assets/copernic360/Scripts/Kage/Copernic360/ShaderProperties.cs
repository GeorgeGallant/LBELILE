using UnityEngine;

namespace Kage.Copernic360
{
    internal class ShaderProperties
    {
        public static int Height = Shader.PropertyToID("_Height");
        public static int Radius = Shader.PropertyToID("_Radius");
        public static int FloorRadius = Shader.PropertyToID("_FloorRadius");
        public static int MovementRange = Shader.PropertyToID("_MovementRange");
        public static int RotationY = Shader.PropertyToID("_RotationY");
    }
}