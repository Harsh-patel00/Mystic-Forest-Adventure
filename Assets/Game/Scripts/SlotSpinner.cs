using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpinner : MonoBehaviour
{
    [SerializeField] Transform slotSymbolsParent;
    [SerializeField] List<GameObject> slotSymbols = new List<GameObject>();
    [SerializeField] private float spinTimer = 15f;
    [SerializeField] private float spinTime;
    private float spinSpeed = 0.01f;
    private Coroutine spinRoutine;
    
    public Action SlotStopped;
    
    private void Start()
    {
        foreach (Transform child in slotSymbolsParent)
        {
            // This is a safety check that all the slot items would contain 'symbol'
            if (child.GetComponent<Symbol>())
            {
                slotSymbols.Add(child.gameObject);
            }
        }
    }

    public void StartSpin()
    {
        spinRoutine = StartCoroutine(StartSpinRoutine());
    }

    private IEnumerator StartSpinRoutine()
    {
        // Initialize the list from the children of slotSymbolsParent
        slotSymbols.Clear();
        foreach (Transform child in slotSymbolsParent)
        {
            slotSymbols.Add(child.gameObject);
        }

        spinTime = spinTimer;

        while (spinTime > 0)
        {
            spinTime -= Time.deltaTime;

            // Rotate the list by moving the first element to the end
            GameObject firstSymbol = slotSymbols[0];
            slotSymbols.RemoveAt(0);
            // slotSymbols.Add(firstSymbol);
            slotSymbols.Insert(slotSymbols.Count - 3, firstSymbol);

            // Reposition symbols based on their new order in the list
            // Ignoring last 3 elements as they are predefined values
            for (int i = 0; i < slotSymbols.Count - 3; i++)
            {
                slotSymbols[i].transform.SetSiblingIndex(i);
            }

            yield return new WaitForSeconds(spinSpeed);
        }

        // Move last 3 (Predefined) elements to top
        int ind = 0;
        for (int i = slotSymbols.Count - 3; i < slotSymbols.Count; i++)
        {
            slotSymbols[i].transform.SetSiblingIndex(ind++);
        }
        
        SlotStopped?.Invoke();
        
        // Done to get the updated list for evaluation
        slotSymbols.Clear();
        foreach (Transform child in slotSymbolsParent)
        {
            slotSymbols.Add(child.gameObject);
        }
        
    }

    public void StopSpin()
    {
        if (spinRoutine != null)
        {
            StopCoroutine(spinRoutine);
            spinRoutine = null;
        }
        
        spinTime = 0;
        
        slotSymbols.Clear();
        foreach (Transform child in slotSymbolsParent)
        {
            slotSymbols.Add(child.gameObject);
        }
        
        SlotStopped?.Invoke();
    }

    public List<GameObject> GetTop3Slots()
    {
        var top3Slots = new List<GameObject>();
        top3Slots.Add(slotSymbols[2]);
        top3Slots.Add(slotSymbols[1]);
        top3Slots.Add(slotSymbols[0]);
        return top3Slots;
    }

    // Used to replace last 3 slots items with the generated one
    public void SetLast3Slots(List<Symbol> newSlots)
    {
        slotSymbols[^3].GetComponent<Symbol>().UpdateWithNewData(newSlots[0]);
        slotSymbols[^2].GetComponent<Symbol>().UpdateWithNewData(newSlots[1]);
        slotSymbols[^1].GetComponent<Symbol>().UpdateWithNewData(newSlots[2]);
    }

    public void SetSlotSpinSpeed(float speed)
    {
        spinSpeed = speed;
    }
}
