using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public string startSceneName = string.Empty;
        public Vector3 startPosition;

/*        private void Awake()
        {
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive); //��ʼʱ����UI
        }*/

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private void Start()
        {
            StartCoroutine(LoadSceneSetActive(startSceneName, startPosition));
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            StartCoroutine(TransitScene(sceneToGo, positionToGo));
        }

        /// <summary>
        /// ���س���������Ϊ����
        /// </summary>
        /// <param name="sceneName">��������</param>
        /// <param name="targetPosition">����λ��</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName, Vector3 targetPosition)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);

            //�ƶ���������
            EventHandler.CallMoveToPosition(targetPosition);

            EventHandler.CallAfterSceneLoadEvent();
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="sceneName">Ŀ�곡��</param>
        /// <param name="targetPosition">Ŀ��λ��</param>
        /// <returns></returns>
        private IEnumerator TransitScene(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName, targetPosition);
        }
    }
}
