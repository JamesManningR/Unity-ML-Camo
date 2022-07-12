using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    // Genes
    public float r;
    public float g;
    public float b;
    public float size;

    // State for if it's alive or dead
    bool isAlive = true;
    // Time spent alive
    public float timeAlive = 0;

    // Mutation Chance
    public float mutationChance = .1f;
    // Mutation extent
    public float colourMutationAmount = .1f;
    public float sizeMutationAmount = .01f;

    // Load renderer and colider
    SpriteRenderer sRenderer;
    Collider2D sCollider;
    
    // Check for mouse click
    void OnMouseDown() {
        // If it's alive
        if (isAlive) {
            // Kill it
            isAlive = false;
            // Set time to live to
            timeAlive = PopulationManager.elapsed;
            // Log the time it died
            Debug.Log("Dead at: " + timeAlive);
            // Disable collider
            sCollider.enabled = false;
            // Disable renderer
            sRenderer.enabled = false;
        }
    }

    public void Mutate() {
        // Log that a mutation has taken place
        Debug.Log("Mutation");
        // Randomly choose either r,g,b or size
        int gene = Random.Range(0, 4);
        // Randomly choose a value to change it by
        float amount;

        // If it's a colour gene
        if (gene <= 3) {
            amount = Random.Range(-colourMutationAmount, colourMutationAmount);
        // Otherwise it's a size gene
        } else {
            amount = Random.Range(-sizeMutationAmount, sizeMutationAmount);
        }

        // Mutate R, G or B based on gene
        if (gene == 0){ 
            r += amount;
            if (r > 1) r = 1;
            if (r < 0) r = 0;
        } else if (gene == 1) {
            g += amount;
            if (g > 1) g = 1;
            if (g < 0) g = 0;
        } else if (gene == 2) {
            b += amount;
            if (b > 1) b = 1;
            if (b < 0) b = 0;
        } else if (gene == 3) {
            size += amount;
            if (size > 1) size = 1;
            if (size < 0) size = 0;
        }
    }

    // Start is called before the first frame update
    void Start() {
        sRenderer = GetComponent<SpriteRenderer>();
        sCollider = GetComponent<Collider2D>();
        // Set the random colour
        sRenderer.color = new Color(r, g, b);
        // set the size of the sprite
        this.transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
