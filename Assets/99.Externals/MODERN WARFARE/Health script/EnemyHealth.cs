using UnityEngine;
using UnityEngine.UI; // Required to access the UI Slider

namespace TargetHealth
{
    public class EnemyHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        private int currentHealth;
        public GameObject deathEffect;
        public ParticleSystem smokeEffect; // Assign in Inspector
        public AudioClip explosionSound; // Explosion sound effect
        private AudioSource audioSource; // AudioSource to play sound

        [Header("Explosion Sound Settings")]
        public float explosionVolume = 1.0f; // Adjustable explosion sound volume

        // Reference to the health bar UI Slider
        public Slider healthBar;

        // Reference to the Canvas for the health bar
        public Canvas healthCanvas;

        void Start()
        {
            currentHealth = maxHealth;

            // Set up the AudioSource component on the enemy object
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if it doesn't exist
            }

            if (smokeEffect != null)
            {
                smokeEffect.Stop(); // Ensure smoke is off at the start
            }

            if (healthCanvas != null)
            {
                healthCanvas.worldCamera = Camera.main; // Assign the camera to the world space canvas
                healthCanvas.transform.SetParent(this.transform); // Make the canvas a child of the enemy
                healthCanvas.transform.localPosition = new Vector3(0, 2, 0); // Offset the canvas above the enemy
            }

            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth; // Set max health on the slider
                healthBar.value = currentHealth; // Set initial health on the slider
            }
        }

        void Update()
        {
            // Keep the health bar following the enemy's position
            if (healthCanvas != null)
            {
                healthCanvas.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z); 
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (healthBar != null)
            {
                healthBar.value = currentHealth; // Update the health bar value
            }

            // Start smoke when health is 30 or below
            if (currentHealth <= 30 && smokeEffect != null && !smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            // Stop smoke when destroyed
            if (smokeEffect != null)
            {
                smokeEffect.Stop();
                smokeEffect.transform.parent = null; // Detach smoke from enemy
                Destroy(smokeEffect.gameObject, 2f); // Destroy smoke after 2 seconds
            }

            if (deathEffect != null)
            {
                GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // Play explosion sound at enemy's position with adjustable volume
            if (explosionSound != null)
            {
                GameObject tempAudioSource = new GameObject("ExplosionSound");
                tempAudioSource.transform.position = transform.position; // Position at enemy's location
                AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();
                tempSource.clip = explosionSound;
                tempSource.volume = explosionVolume; // Use the adjustable volume
                tempSource.Play();

                Destroy(tempAudioSource, explosionSound.length); // Destroy after sound plays
            }

            // Destroy the health canvas and health bar when the enemy dies
            if (healthCanvas != null)
            {
                Destroy(healthCanvas.gameObject);
            }

            // Destroy the enemy object
            Destroy(gameObject);
        }
    }
}


