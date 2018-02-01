using System;
using UnityEngine;


namespace BlendModeShader2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBlendModeLayerFX : BlendModeLayerFX
    {
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer != null)
                {
                    return _spriteRenderer;
                }
                else
                {
                    SpriteRenderer sr = GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        _spriteRenderer = sr;
                    }
                    else
                    {
                        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                    _spriteRenderer.color = mainColor;
                    return _spriteRenderer;
                }
            }
        }

        public Color mainColor
        {
            get
            {
                return spriteRenderer.color;
            }
            set
            {
                if (spriteRenderer.color != value)
                {
                    spriteRenderer.color = value;
                }
            }
        }


        protected override string sharedGrabShader
        {
            get
            {
                return "Custom/BlendModeShader2D/SpriteBlendModeSharedGrab";
            }
        }
        protected override string simpleGrabShader
        {
            get
            {
                return "Custom/BlendModeShader2D/SpriteBlendModeSimpleGrab";
            }
        }


        SpriteRenderer _spriteRenderer;


        void OnEnable()
        {
            onMaterialChange += UpdateMaterial;
        }

        void OnDisable()
        {
            onMaterialChange -= UpdateMaterial;
        }

        protected override void Start()
        {
            base.Start();
            UpdateMaterial();
        }


        public void UpdateMaterial()
        {
            spriteRenderer.material = blendModeMaterial;
        }
    }
}