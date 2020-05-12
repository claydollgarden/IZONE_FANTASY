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

    public IEnumerator LoadMultiEffectAndPlay(string imagePath, Vector3[] targetPosition)
    {
        var particleEffectPrefab = VResourceLoad.Load<ParticleSystem>("Effects/prefab/" + imagePath);

        List<ParticleSystem> particles = new List<ParticleSystem>();

        for (int i = 0; i < targetPosition.Length; i++)
        {
            ParticleSystem particleEffect = Instantiate(particleEffectPrefab);

            particleEffect.transform.position = new Vector3(targetPosition[i].x, targetPosition[i].y, targetPosition[i].z - 0.5f);

            particles.Add(particleEffect);
        }

        yield return new WaitForSeconds(particles[0].duration);

        for (int i = particles.Count - 1; i >= 0; i--)
        {
            Destroy(particles[i].gameObject);
        }
    }
}
