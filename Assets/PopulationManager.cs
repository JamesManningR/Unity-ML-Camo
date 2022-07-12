using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour {
    public GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    int trialTime = 10;
    int generation = 0;

    // GUI Text
    GUIStyle guiStyle = new GUIStyle();
    void OnGUI() {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 100), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 100), "Time: " + (int)elapsed, guiStyle);
    }

    // Start is called before the first frame update
    void Start() {
        // Create the population
        // 10 times
        for (int i = 0; i < populationSize; i++) {
            // Generate a random position
            Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
            // Create a new person
            GameObject newPerson = Instantiate(personPrefab, pos, Quaternion.identity);
            // Generate random rgb values for the new person
            newPerson.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            newPerson.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            newPerson.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
            // Generate random size for the new person
            newPerson.GetComponent<DNA>().size = Random.Range(0.1f, 0.3f);
            // Add the new person to the population
            population.Add(newPerson);
        }
    }

    // Breed single person
    GameObject Breed(GameObject parent1, GameObject parent2) {
        // get a new position
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        // Create a new person
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
        // Randomly grab a gene from either parent
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        // 50% chance of getting either rgb gene from each parent
        offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
        offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
        offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        offspring.GetComponent<DNA>().size = Random.Range(0, 10) < 5 ? dna1.size : dna2.size;
        
        // Get the MutationChance from the DNA
        float mutationChance = offspring.GetComponent<DNA>().mutationChance;
        // Check if the offspring should mutate
        if (Random.Range(0.0f, 1.0f) < mutationChance) {
            // Mutate the offspring
            offspring.GetComponent<DNA>().Mutate();
        }

        // 9 Months passes in an instant

        // Birth the offspring
        return offspring;
    }

    // Breed new population
    void BreedNewPopulation() {
        // Create new list for the new population
        List<GameObject> newPopulation = new List<GameObject>();
        // Remove the least fit individuals
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeAlive).ToList();
        // Clear the old population
        population.Clear();

        // Breed upper half of the population
        for (int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++) {
            // Breed each person with the next person in the list
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
        
        // destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++) {
            Destroy(sortedList[i]);
        }

        // Increment the generation
        generation++;
    }

    // Update is called once per frame
    void Update() {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime) {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
