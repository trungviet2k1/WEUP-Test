using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bulletSound;
    public AudioSource enemySound;
    public AudioClip bullets;
    public AudioClip enemyDead;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBulletSound()
    {
        if (bulletSound != null)
        {
            bulletSound.PlayOneShot(bullets);
        }
    }
    
    public void DestroyEnemySound()
    {
        if (enemySound != null)
        {
            enemySound.PlayOneShot(enemyDead);
        }
    }
}