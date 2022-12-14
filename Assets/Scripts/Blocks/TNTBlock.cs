using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockTypeExtension;
using Death;

public class TNTBlock: Block {

    private bool isCountingDown = false;
    private static float CountDownTime = 3f;
    private GridManager _gridManger;
    
    public TNTBlock(Particle particle, GridManager gridManager): base(BlockType.TNT, particle) {
        _gridManger = gridManager;
    }

    public override void Tick() {
        // Do nothing.
    }

    public void StartCountdown() { 
        if (isCountingDown) {
            return;
        }

        isCountingDown = true;
        particle.StartCoroutine(Countdown());
    }

    private float pulsePeriod = 1f;
    IEnumerator Countdown() {
        float timeLeft = CountDownTime;
        while (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            float val = Mathf.Abs((timeLeft % pulsePeriod) - pulsePeriod / 2f) / (pulsePeriod / 2f);
            particle._renderer.color = Color.Lerp(Color.white, Color.red, val);
            yield return null;
        }
        Explode();
        yield return null;
    }

    Vector2[] explosionTargets = new Vector2[] {
        new Vector2(+0, +1),
        new Vector2(+0, -1),
        new Vector2(+1, +0),
        new Vector2(-1, +0),

        new Vector2(+1, +1),
        new Vector2(-1, -1),
        new Vector2(+1, -1),
        new Vector2(-1, +1),
        
        new Vector2(+0, +2),
        new Vector2(+0, -2),
        new Vector2(+2, +0),
        new Vector2(-2, +0),
    };
    private void Explode() {
        //blast.ShowBlast_static(particle.tile.transform.localPosition);
        _gridManger.CreateExplosion(particle.tile.location);
        Debug.Log(RectTransformUtility.WorldToScreenPoint(null, particle.tile.transform.localPosition));
        foreach (Vector2 target in explosionTargets) {
            Tile tile = particle.tile.getRelativeTile(target);
            if (tile == null) {
                continue;
            }
            if (tile.particle == null) {
                continue;
            }
            Particle p = tile.particle;
            switch (p.getBlockType()) {
                /// Chain explosions.
                case BlockType.TNT:
                    TNTBlock tntBlock = (TNTBlock)p.block;
                    tntBlock.StartCountdown();
                    break;

                /// Check whether water is ice.
                case BlockType.Water:
                    WaterBlock waterBlock = (WaterBlock)p.block;
                    if (waterBlock.IsFrozen()) {
                        p.DeleteParticle(Cause.Explosion, p.getBlockType());
                    }
                    break;

                default:
                    if (p.getBlockType().isExplodable()) {
                        p.DeleteParticle(Cause.Explosion, p.getBlockType());
                    }
                    break;
            }
        }
        particle.DeleteParticle(Cause.Explosion, blockType);
    }
}