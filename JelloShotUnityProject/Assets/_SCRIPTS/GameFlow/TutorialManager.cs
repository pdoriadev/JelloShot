using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField]
    private GameObject _TutorialEnemy;
    private Animator _TutorialTriAnim;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;

        GameManager.onEnterTutorialEvent += TutorialStartListener;
        _TutorialEnemy.GetComponent<DamageableBase>().onTakeDamageEvent += OnDamagedListener;
        _TutorialTriAnim = _TutorialEnemy.GetComponent<Animator>();
    }
    private void OnDisable()
    {
        GameManager.onEnterTutorialEvent -= TutorialStartListener;
        _TutorialEnemy.GetComponent<DamageableBase>().onTakeDamageEvent -= OnDamagedListener;

        instance = null;
    }
    #endregion

    public List<GameObject> tutorialEnemies;

    // checks if any enemy gameObjects are active. If not, exit tutorial state into gameplay state. 
    public void CheckIfAnyActtve()
    {
        for (int i = 0; i < tutorialEnemies.Count - 1; i++)
        {
            if (tutorialEnemies[i].activeSelf == true)
                return;
        }
        //GameManager.instance.ChangeStateTo(GameState.Gameplay);
    }

    private void TutorialStartListener()
    {
        _TutorialTriAnim.SetBool("ShouldSpawn", true);
        _TutorialTriAnim.fireEvents = true;
    }

    private void Update()
    {
        if (_TutorialTriAnim.GetBool("ShouldSpawn") == true 
            && _TutorialTriAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f )
        {
            Vector3 pos = _TutorialEnemy.transform.position;
            _TutorialEnemy.GetComponent<Animator>().SetBool("ShouldRotate", true);
            _TutorialEnemy.transform.position = pos;
        }
    }

    private void OnDamagedListener(DamagedInfo damagedInfo)
    {
        if (damagedInfo.currentHealth == damagedInfo.maxHealth - 1)
        {
            _TutorialEnemy.GetComponent<Animator>().SetBool("ShouldMove", true);
        }
    }
     
}
