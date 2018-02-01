using UnityEngine;
using UnityEngine.UI;


namespace BlendModeShader2D
{
    [RequireComponent(typeof(Image))]
    public class ImageBlendModeFX : BlendModeFX
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
