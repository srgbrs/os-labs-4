using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;

namespace laba_taskPlanner
{
    class Program
    {
        class Task 
        {
            public string id;
            public int duration;
            public int time;

            public Task(string id)
            {
                this.id = id;
                this.duration = 0;
                this.time = 0;
            }

            public int getDuration() {
                return this.duration;
            }

            public int getTime()
            {
                return this.time;
            }

            public void setDuration(int dur)
            {
                this.duration = dur;
            }
    

        }
        class TaskManager
        {
            public List<Task> planned_tasks;
            public List<List<int>> matrix;

            public TaskManager(List<Task> tasks, List<List<int>> matrix)
            {
                this.planned_tasks = tasks;
                this.matrix = matrix;
                getDurationFromMatrix();
                timesCalculation();
            }
            public void getDurationFromMatrix()
            {
                for (var i = 0; i < matrix.Count; i++)
                {
                    planned_tasks[i].duration = matrix[i].FirstOrDefault(x => x != 0 && x != 100);
                }
            }


            public void timesCalculation()
            {
                var q = new List<Task> { planned_tasks.Last() };
                while (q.Any())
                {
                    var currProcess = q[0];
                    var previousProcessesIndexes = Enumerable.Range(0, planned_tasks.Count)
                        .Where(index => matrix[index][planned_tasks.IndexOf(currProcess)] != 0).ToList();
                    foreach (var i in previousProcessesIndexes)
                    {
                        if (planned_tasks[i].duration + currProcess.time > planned_tasks[i].time)
                        {
                            planned_tasks[i].time = planned_tasks[i].duration + currProcess.time;
                        }
                        if (!q.Contains(planned_tasks[i])) q.Add(planned_tasks[i]);
                    }
                    q.RemoveAt(0);
                }
            }

            public List<Task> PathCalculation()
            {
                var currProcess = planned_tasks[0];
                var path = new List<Task> { currProcess };
                while (currProcess != planned_tasks.Last())
                {
                    var nextProcessesIndexes = Enumerable.Range(0, planned_tasks.Count)
                        .Where(index => matrix[planned_tasks.IndexOf(currProcess)][index] != 0).ToList();
                    foreach (var i in nextProcessesIndexes)
                    {
                        if (currProcess.time - currProcess.duration == planned_tasks[i].time)
                        {
                            currProcess = planned_tasks[i];
                            path.Add(currProcess);
                            break;
                        }
                    }
                }
                return path;
            }

            public void showSequence()
            {
                var critical_path = PathCalculation();
                Console.WriteLine("Критический путь");
                int z = 0;
                while (z != critical_path.Count) {
                    Console.Write(" id:" + critical_path[z].id);
                      z++;  
                        
                 }
                Console.WriteLine();
                Console.Write("Общее критическое время:" + planned_tasks[0].time);
             
            }
        }
        static void Main(string[] args)
        {

            var planned_tasks = new List<Task>
            {
                new Task("inital"), new Task("01"),
                new Task("02"), new Task("03"),
                new Task("04"), new Task("05"),
                new Task("06"), new Task("last")
            };

            var matrixofTimes = new List<List<int>>
            {
                new List<int> {0,100,100,100,0,0,0,0}, // start
                new List<int> {0,0,0,0,8,0,0,0}, // a
                new List<int> {0,0,0,0,2,2,0,0},
                new List<int> {0,0,0,0,0,10,10,0},
                new List<int> {0,0,0,0,0,0,0,8},
                new List<int> {0,0,0,0,0,0,0,14},
                new List<int> {0,0,0,0,0,0,0,4},
                new List<int> {0,0,0,0,0,0,0,0}
            };


            var taskManag = new TaskManager(planned_tasks, matrixofTimes);
            taskManag.showSequence();
        }
    }
}