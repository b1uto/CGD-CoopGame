//Shady
using TMPro;
using UnityEngine;
namespace CGD
{
    [ExecuteInEditMode]
    public class Reveal : MonoBehaviour
    {
        [SerializeField] Material Mat;
        [SerializeField] Light SpotLight; //DEBUG

        private void Start()
        {
            //TODO
            //materials won't need to be instantiated if we have one of each clue. 

        }

        void Update()
        {
            if (Mat == null)
                return;


            if (Torch.PointLight)
            {
                Mat.SetVector("_SpotLightPosition", Torch.PointLight.transform.position);
                Mat.SetVector("_SpotLightDirection", -Torch.PointLight.transform.forward);
                Mat.SetFloat("_SpotLightAngle", Torch.PointLight.spotAngle);
                Mat.SetFloat("_SpotLightIntensity", Torch.PointLight.intensity);
                Mat.SetFloat("_SpotLightRange", Torch.PointLight.range);
            }
#if DEBUGGING
            else if (SpotLight)
            {
                Mat.SetVector("_SpotLightPosition", SpotLight.transform.position);
                Mat.SetVector("_SpotLightDirection", -SpotLight.transform.forward);
                Mat.SetFloat("_SpotLightAngle", SpotLight.spotAngle);
                Mat.SetFloat("_SpotLightIntensity", SpotLight.intensity);
                Mat.SetFloat("_SpotLightRange", SpotLight.range);
            }
        }
#endif
        }
    }
