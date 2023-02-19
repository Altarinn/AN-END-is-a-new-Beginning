using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Phantom : DamageTaker
{
    [Header("CUTSCENE")]
    public Transform CSPivot;
    public float CSflySpeed;

    public GameObject TilesToDestroy, SoulOrb;

    protected override void Awake()
    {
        if (GameController.Instance.IsPhantom)
        {
            TilesToDestroy.SetActive(false);
            Destroy(gameObject);
            return;
        }
        else
        {
            var pc = GetComponent<TarodevController.PlayerController>();
            pc.InputModule = GetComponent<BaseAI>();

            GetComponent<PlayerFire>().InputModule = GetComponent<BaseAI>();

            transform.GetChild(0).GetComponent<TarodevController.PlayerAnimator>().enabled = false;
        }

        base.Awake();
        destroyOnDeath = false;
    }

    protected override void CheckDeath()
    {
        base.CheckDeath();

        if(dead)
        {
            StartCoroutine(PhantomCutScene());
        }
    }

    IEnumerator PhantomCutScene()
    {
        // Initialization
        var pc = GetComponent<TarodevController.PlayerController>();
        var player = GameController.Instance.player;

        GetComponent<BaseAI>().enabled = false;

        pc.InputModule = GetComponent<ReplayableInput>();
        pc.IsPhantom = true;
        pc.Gravity = true;
        GetComponent<PlayerFire>().InputModule = GetComponent<ReplayableInput>();

        GameController.Instance.EnterCutScene();

        // Jump
        pc.ZeroVelocity();
        pc.FakeJump();

        yield return new WaitForSeconds(1.0f);

        float overtime = 5.0f;

        // Wait until grounded or 5 sec
        while (!pc.Grounded && overtime > 0) { overtime -= Time.deltaTime; yield return null; }

        // Disable gravity, collider etc.
        pc.enabled = false;
        pc.GetComponent<Collider2D>().enabled = false;

        // Move to position
        while ((transform.position - CSPivot.position).sqrMagnitude > 1e-4)
        {
            transform.position = Vector3.MoveTowards(transform.position, CSPivot.position, CSflySpeed * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(1.0f);

        // Drain soul
        for (int i = 0; i < 60; i++)
        {
            var so = Instantiate(SoulOrb, player.transform.position, Quaternion.identity).GetComponent<SoulOrb>();
            so.Go(Random.insideUnitCircle.normalized, transform.position);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(5.0f);

        // Explosion
        var expl = Instantiate(GameController.Instance.DeathExplosion, transform.position, Quaternion.identity);
        expl.transform.localScale = Vector3.one * 6.0f;

        transform.DOScale(Vector3.one, 0.8f);

        yield return new WaitForSeconds(0.1f);

        // Change level layout
        TilesToDestroy.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        // Enable gravity, collider etc.
        pc.enabled = true;
        pc.GetComponent<Collider2D>().enabled = true;

        GameController.Instance.ChangeToPhantom(this);

        GameController.Instance.ExitCutScene();
    }
}
