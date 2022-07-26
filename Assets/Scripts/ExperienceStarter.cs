using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class used to detect the player presence on the portal
// so that the experience could be started
public class ExperienceStarter : MonoBehaviour
{
    // The text to enable/disable on player collision
    public GameObject UiText;
    
    // The new scene to load
    public string SceneToLoad;

    // True -> the player is ready to load the new scene,
    // False -> scene transition disabled
    [SerializeField]
    private bool _enableTransition; 

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (_enableTransition)
        {
            if (Input.GetKeyUp(KeyCode.E) && !string.IsNullOrEmpty(SceneToLoad))
            {
                // TODO FadeIn/FadeOut screen effet
                // ...

                // Load the new scene
                //SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
                StartCoroutine(FadeOutEffect(() => {
                    SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
                }));
            }
        }
    }

    public IEnumerator FadeOutEffect(Action loadSceneCallback)
    {
        var fadeEffect = GameObject.Find("FadeEffect");

        if (fadeEffect != null)
        {
            var image = fadeEffect.GetComponent<UnityEngine.UI.Image>();
            var animator = fadeEffect.GetComponent<Animator>();

            animator.SetBool("Fade", true);

            yield return new WaitUntil(() => image.color.a == 1);
        }

        // Invoke callback.
        loadSceneCallback();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UiText.SetActive(true);
            _enableTransition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UiText.SetActive(false);
            _enableTransition = false;
        }
    }
}
