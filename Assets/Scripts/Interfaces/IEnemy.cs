using UnityEngine;

public interface IEnemy
{
    Vector3 boundCenter { get; set; }
    Vector3 bounds { get; set; }

    int MaxHealth { get; set; }

    int health { get; set; }

    ParticleSystem hurtPars { get; set; }

    void damage(int amt);
    
}
