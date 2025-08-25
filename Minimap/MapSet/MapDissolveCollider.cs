using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KHJ.Minimap
{
    public class MapDissolveCollider : MonoBehaviour
    {
        [SerializeField] private Transform _dissovleParant;
        [SerializeField] private float _dissolveAlphaValue;
        [SerializeField] private float _dissolveDuration;

        private Dictionary<Material, Material> _materialCache = new Dictionary<Material, Material>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            foreach (Transform trm in _dissovleParant)
            {
                if (!trm.gameObject.activeSelf) continue;
                MeshRenderer targetRender = trm.GetComponent<MeshRenderer>();
                if (targetRender == null) continue;

                Material originalMat = targetRender.sharedMaterial;

                if (!_materialCache.TryGetValue(originalMat, out Material copiedMat))
                {
                    copiedMat = Instantiate(originalMat);
                    _materialCache[originalMat] = copiedMat;

                    copiedMat.SetFloat("_Surface", 1f);
                    copiedMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    copiedMat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    copiedMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                }

                targetRender.material = copiedMat;
                copiedMat.DOFade(_dissolveAlphaValue, _dissolveDuration);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            foreach (Transform trm in _dissovleParant)
            {
                if (!trm.gameObject.activeSelf) continue;
                MeshRenderer targetRender = trm.GetComponent<MeshRenderer>();
                if (targetRender == null) continue;

                Material currentMat = targetRender.material;

                currentMat.DOFade(1f, _dissolveDuration)
                    .OnComplete(() =>
                    {
                        currentMat.SetFloat("_Surface", 0f);
                        currentMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                        currentMat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        currentMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    });
            }
        }
    }
}