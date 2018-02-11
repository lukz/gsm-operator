using UnityEngine;


namespace BlendModeShader2D
{
    [ExecuteInEditMode]
    public class BlendModeFlow : MonoBehaviour
    {
        public BlendModeFX blendModeFX
        {
            get
            {
                if (_blendModeFx == null)
                {
                    _blendModeFx = GetComponent<BlendModeFX>();
                }
                return _blendModeFx;
            }
        }


        public Vector2 flowSpeed = Vector2.one;


        BlendModeFX _blendModeFx;


        void Update()
        {
            blendModeFX.effectTextureOffset = blendModeFX.effectTextureOffset - Time.deltaTime * flowSpeed;
        }
    } 
}
