using System.Threading.Tasks;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public int health = 100;

    void Update()
    {
        if (health <= 0)
        {
            PlayDeathAnimationAndDestroy();
        }
    }

    private async void PlayDeathAnimationAndDestroy()
    {
        await PlayDeathAnimation(); // vent p� at animationen er f�rdig
        Destroy(gameObject); // fjern denne npc efter
    }

    private async Task PlayDeathAnimation()
    {
        // tilf�j animation nedenunder
        Debug.Log("Playing death animation...");

    
        await Task.Delay(2000); // g�r ud fra at animationen er 2 sekunder lang.
    }
}
