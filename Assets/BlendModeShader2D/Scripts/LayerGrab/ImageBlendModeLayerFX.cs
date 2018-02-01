using UnityEngine;
using UnityEngine.UI;


namespace BlendModeShader2D
{ 
    [RequireComponent(typeof(Image))]
    public class ImageBlendModeLayerFX : BlendModeLayerFX
    {
        public Image image
        {
            get
            {
                if (_image != null)
                {
                    return _image;
                }
                else
                {
                    Image i = GetComponent<Image>();
                    if (i != null)
                    {
                        _image = i;
                    }
                    else
                    {
                        _image = gameObject.AddComponent<Image>();
                    }
                    _image.color = mainColor;
                    return _image;
                }
            }
        }

        public Color mainColor
        {
            get
            {
                return image.color;
            }
            set
            {
                if (image.color != value)
                {
                    image.color = value;
                }
            }
        }


        protected override string sharedGrabShader
        {
            get
            {
                return "Custom/BlendModeShader2D/UIBlendModeSharedGrab";
            }
        }
        protected override string simpleGrabShader
        {
            get
            {
                return "Custom/BlendModeShader2D/UIBlendModeSimpleGrab";
            }
        }


        Image _image;


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
            image.material = blendModeMaterial;
        }
    }
}