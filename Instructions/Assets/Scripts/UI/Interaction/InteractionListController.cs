using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionListController : MonoBehaviour
{

    public GameObject addInstructionPrefab;
    private List<Instruction> instructions = new List<Instruction>();

    private bool changed = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (changed)
        {
            changed = false;
            GenerateUI();
        }
    }

    private void GenerateUI()
    {
        Clear();

        InstInstruction(addInstructionPrefab);

        List<Instruction> tmp = new List<Instruction>(instructions);
        foreach (Instruction instruction in tmp)
        {
            InstInstruction(instruction.gameObject);
            InstInstruction(addInstructionPrefab);
        }
    }

    private void InstInstruction(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
    }

    private void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child);
    }

}
