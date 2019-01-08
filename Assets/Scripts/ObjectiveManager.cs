using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Objective {

    GameObject interactable;

    public int ControlPanelId { get; set; }
}

public class ObjectiveManager : MonoBehaviour {

    public List<Objective> objectiveList;

	/// <summary>
    /// Creates random objective list from all possible objectives
    /// Also the number of objectives is random
    /// </summary>
    /// <param name="AllObjectivesList"></param>
   public void CreateObjectives(List<Objective> AllObjectivesList) {
        System.Random rnd = new System.Random();

        int maxIterations = 1;
        int numberOfPanelSwitch = rnd.Next(5, 8);
        print("number of Panel switch: " + numberOfPanelSwitch);

        for (int index = 0; index < numberOfPanelSwitch; index++) {

            int panelId = rnd.Next(4);
            print("Panel id: " + panelId);

            int iterations = rnd.Next(1,5);
            print("iterations: " + iterations);

            maxIterations =+ iterations;

            int id = 0;
            while (id < iterations) {
                int numberOfInteractions = 0;

                List<Objective> controlPanel = new List<Objective>();
                foreach (var objective in AllObjectivesList) {
                    if (objective.ControlPanelId == panelId) {
                        controlPanel.Add(objective);
                    }
                }

                controlPanel = Shuffle(controlPanel);

                foreach (var objective in controlPanel) {
                    objectiveList.Add(objective);
                }
                
            }

        }
        


    }

    List<Objective> Shuffle(List<Objective> list) {
        System.Random rnd = new System.Random();
        for (int i = 0; i < list.Count; i++) {

            Objective temp = list[i];
            int rndIndex = rnd.Next(list.Count);
            list[i] = list[rndIndex];
            list[rndIndex] = temp;

        }
        return list;
    }

    void PopulateList(Objective objective) {

    }
}
