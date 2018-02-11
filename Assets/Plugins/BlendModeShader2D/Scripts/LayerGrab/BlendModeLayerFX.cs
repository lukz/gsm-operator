using System;
using System.Collections.Generic;
using PropertyBackingFieldDrawer;
using UnityEngine;


namespace BlendModeShader2D
{
    [ExecuteInEditMode]
    public abstract class BlendModeLayerFX : MonoBehaviour
    {
        public enum GrabMode
        {
            Shared,
            Simple
        }

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

        public GrabMode grabMode
        {
            get
            {
                return _grabMode;
            }
            set
            {
                if (_grabMode != value)
                {
                    _grabMode = value;
                    if (value == GrabMode.Shared)
                    {
                        blendModeShader = Shader.Find(sharedGrabShader);
                    }
                    else if (value == GrabMode.Simple)
                    {
                        blendModeShader = Shader.Find(simpleGrabShader);
                    }
                }
            }
        }

        public Color baseLayerColor
        {
            get
            {
                return _baseLayerColor;
            }
            set
            {
                if (_baseLayerColor != value)
                {
                    _baseLayerColor = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetColor("_BaseLayerColor", _baseLayerColor);
                    }
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

        public Color extraColor
        {
            get
            {
                return _extraColor;
            }
            set
            {
                if (_extraColor != value)
                {
                    _extraColor = value;
                    if (_blendModeMaterial != null)
                    {
                        _blendModeMaterial.SetColor("_ExtraColor", _extraColor);
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

        protected abstract string sharedGrabShader { get; }
        protected abstract string simpleGrabShader { get; }
        

        [SerializeField, PropertyBackingField]
        Shader _blendModeShader;
        [PropertyBackingField]
        Material _blendModeMaterial;
        
        [SerializeField, PropertyBackingField]
        [Space]
        BlendMode _blendMode = BlendMode.Normal;
        [SerializeField, PropertyBackingField]
        GrabMode _grabMode = GrabMode.Shared;
        [SerializeField, PropertyBackingField]
        Color _baseLayerColor = Color.white;
        [SerializeField, PropertyBackingField]
        bool _enableGrayBase = false;

        [SerializeField, PropertyBackingField]
        [Space]
        Color _extraColor = Color.clear;

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
            bmMaterial.SetColor("_BaseLayerColor", _baseLayerColor);
            bmMaterial.SetColor("_ExtraColor", _extraColor);
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

            foreach (string strBlendMode in Enum.GetNames(typeof(BlendMode)))
            {
                bmMaterial.DisableKeyword("_BM_" + strBlendMode.ToUpper());
            }
            bmMaterial.EnableKeyword("_BM_" + _blendMode.ToString().ToUpper());
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