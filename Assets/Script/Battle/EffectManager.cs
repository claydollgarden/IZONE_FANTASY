using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public IEnumerator LoadEffectAndPlay(string imagePath, Vector3 targetPosition)
    {
        var particleEffectPrefab = VResourceLoad.Load<ParticleSystem>("Effects/prefab/" + imagePath);

        ParticleSystem particleEffect = Instantiate(particleEffectPrefab);

        particleEffect.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z - 0.5f);

        yield return new WaitForSeconds(particleEffect.duration);

        Destroy(particleEffect.gameObject);
    }
}
