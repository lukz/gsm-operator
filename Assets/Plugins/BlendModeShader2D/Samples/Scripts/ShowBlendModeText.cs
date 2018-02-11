using UnityEngine;


namespace BlendModeShader2D.Demo
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMesh))]
    public class ShowBlendModeText : MonoBehaviour
    {
        TextMesh _textMesh;
        SpriteBlendModeFX _spriteBlendModeFX;
        void Start ()
        {
            _textMesh = GetComponent<TextMesh>();
            _spriteBlendModeFX = GetComponentInParent<SpriteBlendModeFX>();
        }
	
        void Update ()
        {
            _textMesh.text = _spriteBlendModeFX.blendMode.ToString();
        }
    }
}
