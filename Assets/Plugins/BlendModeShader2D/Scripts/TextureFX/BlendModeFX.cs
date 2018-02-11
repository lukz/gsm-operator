using System;
using System.Collections.Generic;
using PropertyBackingFieldDrawer;
using UnityEngine;


namespace BlendModeShader2D
{
    [ExecuteInEditMode]
    public abstract class BlendModeFX : MonoBehaviour
    {
        //public enum BlendMode
        //{
        //    Normal,
        //    //Darken
        //    Darken,
        //    Multiply,
        //    ColorBurn,
        //    LinearBurn,
        //    DarkerColor,
        //    //Lighten
        //    Lighten,
        //    Screen,
        //    ColorDodge,
        //    LinearDodge,
        //    LighterColor,
        //    //Contrast
        //    Overlay,
        //    SoftLight,
        //    HardLight,
        //    VividLight,
        //    LinearLight,
        //    PinLight,
        //    HardMix,
        //    //Inversion
        //    Difference,
        //    Exclusion,
        //    //Cancelation
        //    Subtract,
        //    Divide,
        //    //Component
        //    Hue,
        //    Saturation,
        //    Color,
        //    Luminosity
        //}
        //public enum ColorSoloMode
        //{
        //    None,
        //    Red,
        //    Green,
        //    Blue
        //}


        public Shader blendModeShader
        {
            get
            {
                return _blendModeShader;
            }
            set
            {
                if (_blendModeShader != value)
                {
                    if (value == null)
                    {
                        Debug.Log("Missing shader in " + ToString());
                        enabled = false;
                    }
                    else
                    {
                        if (value.isSupported)
                        {
                            _blendModeShader = value;
                            _blendModeMaterial = new Material(_blendModeShader);
                            InitBlendModeMaterial(_blendModeMaterial);
                            _blendModeMaterial.hideFlags = HideFlags.DontSave;
                            _createdMaterials.Add(_blendModeMaterial);
                            if (onMaterialChange != null)
                            {
                                onMaterialChange();
                            }
                        }
                        else
                        {
                            enabled = false;
                            Debug.Log("The shader " + value.ToString() + " is not supported on this platform!");
                        }
                    }
                }
            }
        }

        public Material blendModeMaterial
        {
            get
            {
                return _blendModeMaterial;
            }
            private set
            {
                _blendModeMaterial = value;
                InitBlendModeMaterial(_blendModeMaterial);
                if (onMaterialChange != null)
                {
                    onMaterialChange();
                }
            }
        }

        public bool enableGrayBase
        {
            get
            {
                return _enableGrayBase;
            }
            set
            {
                if (_enableGrayBase != value)
                {
                    _enableGrayBase = value;
                    if (_blendModeMaterial != null)
                    {
                        if (_enableGrayBase)
                        {
                            _blendModeMaterial.EnableKeyword("GRAY_BASE_ON");
                        }
                        else
                        {
                            _blendModeMaterial.DisableKeyword("GRAY_BASE_ON");
                        }
                    }
                }
            }
        }

        public BlendMode blendMode
        {
            get
            {
                return _blendMode;
            }
            set
            {
                if (_blendMode != value)
                {
                    string sDisable = "_BM_" + _blendMode.ToString().ToUpper();
                    _blendMode = value;
                    string sEnable = "_BM_" + _blendMode.ToString().ToUpper();
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.DisableKeyword(sDisable);
                        _blendModeMaterial.EnableKeyword(sEnable);
                    }
                }
            }
        }

        public Texture2D effectTexture
        {
            get
            {
                return _effectTexture;
            }
            set
            {
                if (_effectTexture != value)
                {
                    _effectTexture = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetTexture("_EffectTex", _effectTexture);
                    }
                }
            }
        }
        public Vector2 effectTextureTiling
        {
            get
            {
                return _effectTextureTiling;
            }
            set
            {
                if (_effectTextureTiling != value)
                {
                    _effectTextureTiling = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetTextureScale("_EffectTex", _effectTextureTiling);
                    }
                }
            }
        }

        public Vector2 effectTextureOffset
        {
            get
            {
                return _effectTextureOffset;
            }
            set
            {
                if (_effectTextureOffset != value)
                {
                    _effectTextureOffset = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetTextureOffset("_EffectTex", _effectTextureOffset);
                    }
                }
            }
        }

        public Color effectColor
        {
            get
            {
                return _effectColor;
            }
            set
            {
                if (_effectColor != value)
                {
                    _effectColor = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetColor("_EffectColor", _effectColor);
                    }
                }
            }
        }

        public ColorSoloMode colorSoloMode
        {
            get
            {
                return _colorSoloMode;
            }
            set
            {
                if (_colorSoloMode != value)
                {
                    string sDisable = "_COLORSOLO_" + _colorSoloMode.ToString().ToUpper();
                    _colorSoloMode = value;
                    string sEnable = "_COLORSOLO_" + _colorSoloMode.ToString().ToUpper();
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.DisableKeyword(sDisable);
                        _blendModeMaterial.EnableKeyword(sEnable);
                    }
                }
            }
        }

        public bool pixelSnap
        {
            get
            {
                return _pixelSnap;
            }
            set
            {
                if (_pixelSnap != value)
                {
                    _pixelSnap = value;
                    if (_blendModeMaterial != null)
                    {
                        if (_pixelSnap)
                        {
                            _blendModeMaterial.EnableKeyword("PIXELSNAP_ON");
                        }
                        else
                        {
                            _blendModeMaterial.DisableKeyword("PIXELSNAP_ON");
                        }
                    }
                }
            }
        }


        protected delegate void OnMaterialChange();
        protected OnMaterialChange onMaterialChange;


        [SerializeField, PropertyBackingField]
        Shader _blendModeShader;
        [PropertyBackingField]
        Material _blendModeMaterial;
        [SerializeField, PropertyBackingField]
        [Space]
        bool _enableGrayBase = false;
        [SerializeField, PropertyBackingField]
        [Space]
        BlendMode _blendMode = BlendMode.Normal;
        [SerializeField, PropertyBackingField]
        Texture2D _effectTexture;
        [SerializeField, PropertyBackingField]
        Vector2 _effectTextureTiling = Vector2.one;
        [SerializeField, PropertyBackingField]
        Vector2 _effectTextureOffset = Vector2.zero;
        [SerializeField, PropertyBackingField]
        Color _effectColor = Color.white;
        [SerializeField, PropertyBackingField]
        ColorSoloMode _colorSoloMode = ColorSoloMode.None;

        [SerializeField, PropertyBackingField]
        [Space]
        bool _pixelSnap = false;

        List<Material> _createdMaterials = new List<Material>();

        protected virtual void Start()
        {
            if (_blendModeMaterial == null && _blendModeShader != null)
            {
                _blendModeMaterial = new Material(_blendModeShader);
                InitBlendModeMaterial(_blendModeMaterial);
                _blendModeMaterial.hideFlags = HideFlags.DontSave;
                _createdMaterials.Add(_blendModeMaterial);
            }
        }

        void OnDestroy()
        {
            RemoveCreatedMaterials();
        }

        void InitBlendModeMaterial(Material bmMaterial)
        {
            if (bmMaterial == null)
            {
                return;
            }
            bmMaterial.SetTexture("_EffectTex", _effectTexture);
            bmMaterial.SetColor("_EffectColor", _effectColor);
            if (_enableGrayBase)
            {
                bmMaterial.EnableKeyword("GRAY_BASE_ON");
            }
            else
            {
                bmMaterial.DisableKeyword("GRAY_BASE_ON");
            }
            if (_pixelSnap)
            {
                bmMaterial.EnableKeyword("PIXELSNAP_ON");
            }
            else
            {
                bmMaterial.DisableKeyword("PIXELSNAP_ON");
            }

            foreach (string strColorSoloMode in Enum.GetNames(typeof(ColorSoloMode)))
            {
                bmMaterial.DisableKeyword("_COLORSOLO_" + strColorSoloMode.ToUpper());
            }
            bmMaterial.EnableKeyword("_COLORSOLO_" + _colorSoloMode.ToString().ToUpper());

            foreach (string strBlendMode in Enum.GetNames(typeof(BlendMode)))
            {
                bmMaterial.DisableKeyword("_BM_" + strBlendMode.ToUpper());
            }
            bmMaterial.EnableKeyword("_BM_" + _blendMode.ToString().ToUpper());

            _blendModeMaterial.SetTextureScale("_EffectTex", _effectTextureTiling);
            _blendModeMaterial.SetTextureOffset("_EffectTex", _effectTextureOffset);
        }

        private void RemoveCreatedMaterials()
        {
            while (_createdMaterials.Count > 0)
            {
                Material mat = _createdMaterials[0];
                _createdMaterials.RemoveAt(0);
#if UNITY_EDITOR
                DestroyImmediate(mat);
#else
                Destroy(mat);
#endif
            }
        }

    } 
}
