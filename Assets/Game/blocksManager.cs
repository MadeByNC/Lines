﻿using UnityEngine;
using System.Collections;

public class blocksManager : MonoBehaviour 
{
    public enum blockColor { BLUE, GREEN, YELLOW, RED, PINK, BROWN, ORANGE };
    public GameObject blockPrefab;
    public float blockSpawnDelay = 0.5f;
    public int blocksToCreate = 0;
    public Material unselectBlue, unselectGreen, unselectYellow, unselectRed, unselectPink, unselectBrown, unselectOrange;
    public Material selectBlue, selectGreen, selectYellow, selectRed, selectPink, selectBrown, selectOrange;

    private int blockIndex = 1; //Index to naming blocks in inspector
    private float timer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) blocksToCreate = 5;

        if (blocksToCreate > 0)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                timer = blockSpawnDelay;
                createNewBlock(blocksToCreate - 1);
                --blocksToCreate;
            }
        }
        if (gameObject.transform.childCount < 6 && blocksToCreate == 0) blocksToCreate = 5;
        if (gameObject.transform.childCount >= 49 && !gameManager.endGame.active) gameManager.endGame.setActive(true);
    }

    public blockController getSelectedBlock()
    {
        foreach (Transform block in transform) if (block.gameObject.GetComponent<blockController>().selected) return block.gameObject.GetComponent<blockController>();
        return null;
    }

    //To use in arenaBlockController
    public bool blockSelected()
    {
        foreach (Transform block in transform) if (block.gameObject.GetComponent<blockController>().selected) return true;
        return false;
    }

    //To use in blockController
    public bool blockIsSelected() 
    {
        foreach(Transform block in transform)
        {
            if (block.gameObject.GetComponent<blockController>().selected)
            {
                block.gameObject.GetComponent<blockController>().selected = false;
                block.gameObject.GetComponent<blockController>().updateMaterial();
                Debug.LogWarning("Multiple selection");
                return true;
            }
        }
        return false;
    }

    public bool blockIsMove()
    {
        foreach(Transform block in transform)
        {
            if (!block.gameObject.GetComponent<blockController>().onPosition) return true;
        }
        return false;
    }

    public void playBadPathSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
    }

    public void createNewBlock(int nextBlockNum)
    {
        arenaBlockController arenaBlock = null;

        if (gameObject.transform.childCount < 42) while (arenaBlock == null || arenaBlock.block != null) arenaBlock = gameManager.arena.arenaBlock[Random.Range(0, 48)];
        else
        {
            foreach (arenaBlockController it in gameManager.arena.arenaBlock)
            {
                if (it.block == null)
                {
                    arenaBlock = it;
                    break;
                }
            }
        }

        GameObject newBlock = Instantiate(blockPrefab) as GameObject;
        newBlock.name = blockIndex.ToString();

        newBlock.transform.parent = gameObject.transform;
        newBlock.GetComponent<blockController>().arenaTarget = arenaBlock;
        newBlock.GetComponent<blockController>().color = gameManager.nextBlock.color[nextBlockNum];

        newBlock.transform.localScale = blockPrefab.transform.localScale;
        newBlock.transform.localRotation = blockPrefab.transform.localRotation;

        Vector3 pos = arenaBlock.transform.localPosition;
        pos.z = -0.639f;
        newBlock.transform.localPosition = pos;

        ++blockIndex;
        if (nextBlockNum == 0) gameManager.nextBlock.randNewColors();
    }

    public void destroyBlock(blockController block)
    {
        foreach (Transform it in transform)
        {
            if(block == it.gameObject.GetComponent<blockController>())
            {
                blocksToCreate = 0;
                gameManager.nextBlock.randNewColors();
                it.gameObject.GetComponent<blockController>().destroyBlock();
                break;
            }
        }
    }

    public void destroyBlock(int size)
    {
        foreach (Transform it in transform)
        {
            if (size == 0 || transform.childCount < size) break;
            it.gameObject.GetComponent<blockController>().destroyBlock();
            --size;
        }
    }

    public void destroyAllBlocks()
    {
        for (int i = gameObject.transform.childCount; i > 0; --i) gameObject.transform.GetChild(i - 1).gameObject.GetComponent<blockController>().destroyBlock();
    }
}