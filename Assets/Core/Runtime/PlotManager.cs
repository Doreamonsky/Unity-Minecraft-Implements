using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace MC.Core
{
    public class PlotManager : MonoBehaviour
    {

        [System.Serializable]
        public class Task
        {
            public enum TaskType
            {
                InventoryCount
            }

            public TaskType taskType = TaskType.InventoryCount;

            public GameObject uiGuidance;

            public Toggle completeToggle;

            [Header("Inventory Count")]
            public string invName;
            public int invCount;

            [HideInInspector]
            public Player player;

            public void CompleteTask()
            {
                completeToggle.isOn = true;
                uiGuidance.transform.SetAsLastSibling();
            }

            public bool CheckComplete()
            {
                var isComplete = false;

                switch (taskType)
                {
                    case TaskType.InventoryCount:
                        var inv = player.inventorySystem.inventoryStorageList.Find(val => val.inventory.inventoryName == invName);

                        if (inv == null)
                        {
                            isComplete = false;
                        }
                        else
                        {
                            if (inv.count < invCount)
                            {
                                isComplete = false;
                            }
                            else
                            {
                                isComplete = true;
                            }
                        }
                        break;


                }

                return isComplete;
            }
        }

        public List<Task> tasks = new List<Task>();

        public GameObject player;

        public GameObject runtime;

        public GameObject timeline;

        public GameObject gamePlayGuide;

        public PlayableDirector director;

        public bool skipTimeline = false;

        private Queue<Task> queueTasks = new Queue<Task>();

        private void Start()
        {
            timeline.SetActive(true);
            runtime.SetActive(false);
            gamePlayGuide.SetActive(false);

            foreach (var task in tasks)
            {
                task.player = player.GetComponent<Player>();
                queueTasks.Enqueue(task);
            }

            StartCoroutine(PlotUpdate());
        }

        private IEnumerator PlotUpdate()
        {
            //仅编辑器可选择跳过Timeline
#if UNITY_EDITOR
            if (!skipTimeline)
            {
#endif
                yield return new WaitForSeconds((float)director.duration + (float)director.initialTime);
#if UNITY_EDITOR
            }
#endif

            timeline.SetActive(false);

            runtime.SetActive(true);
            gamePlayGuide.SetActive(true);

            while (true)
            {
                var task = queueTasks.Dequeue();

                if (task == null)
                {
                    yield break;
                }
                else
                {
                    while (!task.CheckComplete())
                    {
                        yield return new WaitForSeconds(1);
                    }

                    task.CompleteTask();
                }
            }
        }
    }

}
