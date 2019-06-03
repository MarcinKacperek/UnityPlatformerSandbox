using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour {

    [SerializeField] private int numberOfGhosts = 3;
    [SerializeField] private float ghostFadeTime = 0.2f;
    [SerializeField] private float ghostInitialAlpha = 0.5f;

    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowTrail(float time) {
        StartCoroutine(CreateGhosts(time));
    }

    private IEnumerator CreateGhosts(float time) {
        float timeInterval = time / numberOfGhosts;

        for (int i = 0; i < numberOfGhosts; i++) {
            yield return new WaitForSeconds(timeInterval);
            CreateGhost();
        }
    }

    private void CreateGhost() {
        GameObject ghost = new GameObject("Ghost");
        // Copy scale
        ghost.transform.position = transform.position;
        ghost.transform.localScale = transform.localScale;
        // Copy image
        SpriteRenderer ghostSpriteRenderer = ghost.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        ghostSpriteRenderer.sprite = spriteRenderer.sprite;
        ghostSpriteRenderer.color = spriteRenderer.color;
        
        StartCoroutine(FadeGhost(ghostSpriteRenderer));
    }

    private IEnumerator FadeGhost(SpriteRenderer ghostSpriteRenderer) {
        Color color = ghostSpriteRenderer.color;

        float timeElapsed = 0;
        while (timeElapsed < ghostFadeTime) {
            color.a = Mathf.Lerp(ghostInitialAlpha, 0.0f, timeElapsed / ghostFadeTime);
            ghostSpriteRenderer.color = color;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        Destroy(ghostSpriteRenderer.gameObject);
    }

}
