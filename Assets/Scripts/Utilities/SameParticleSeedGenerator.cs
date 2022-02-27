using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SameParticleSeedGenerator : MonoBehaviour
{
    public ParticleSystem[] setSeedParticles;
    public int ParticleSeed;
    public bool forceNewSeed;


	private void FixedUpdate()
	{
        if (setSeedParticles[0].isStopped)
            Destroy(gameObject);
    }

#if UNITY_EDITOR
	void OnValidate()
    {
        if (forceNewSeed)
        {
            ParticleSeed = Random.Range(0, int.MaxValue);

            if (setSeedParticles.Length > 0)
            {
                for (int i = 0; i < setSeedParticles.Length; i++)
                {
                    setSeedParticles[i].randomSeed = (uint)ParticleSeed;
                }
            }

            forceNewSeed = false;
        }
    }
#endif
}