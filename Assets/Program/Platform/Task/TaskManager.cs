using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Platform
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TaskManager: TMonoSingleton<TaskManager>
    {
        private Dictionary<int, Task> _taskMap;
        private List<Task> _removeingTasks;
        private List<Task> _addingTasks;

        private int _autoIncID;

        private int NewTaskID
        {
            get
            {
                return _autoIncID;
                _autoIncID++;
            }
        }

        public void Awake()
        {
            _taskMap = new Dictionary<int, Task>();
            _removeingTasks = new List<Task>();
            _addingTasks = new List<Task>();
            _autoIncID = 1;
        }

        void Update()
        {
            float dt = Time.deltaTime;

            //新加入的Task
            foreach (var task in this._addingTasks)
            {
                this._taskMap.Add(task.Id, task);
            }
            this._addingTasks.Clear();

            //触发
            foreach (var task in _taskMap.Values)
            {
                if (task.isRemoved)
                {
                    this._removeingTasks.Add(task);
                    continue;
                }

                task.passedTime += dt;
                if (task.passedTime >= (task.delay + task.duration))
                {//触发
                    task.callBack.Invoke(task.param);
                    task.repeat--;
                    task.passedTime -= (task.delay + task.duration);
                    task.delay = 0;

                    if (task.repeat == 0)
                    {
                        task.isRemoved = true;
                        this._removeingTasks.Add(task);
                    }
                }
            }
            
            //清理要删除的Task
            foreach (var task in _removeingTasks)
            {
                this._taskMap.Remove(task.Id);
            }
            this._removeingTasks.Clear();
        }

        /// <summary>
        /// 添加只触发一次的定时任务
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int AddOnceTask(Action callBack, float delay)
        {
            return AddTask(callBack, 1, 0, delay);
        }
        
        /// <summary>
        /// 添加只触发一次的带参数的定时任务
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="param"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int AddOnceTask(Action<object> callBack, object param, float delay)
        {
            return AddTask(callBack, param, 1, 0, delay);
        }

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="repeat">repeat小于等于0时无限触发</param>
        /// <param name="duration"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int AddTask(Action callBack, int repeat, float duration, float delay)
        {
            Action<object> temp = o =>
            {
                callBack.Invoke();
            };
            return this.AddTask(temp, null, repeat, duration, delay);
        }

        /// <summary>
        /// 添加带参数的定时任务
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="param"></param>
        /// <param name="repeat">repeat小于等于0时无限触发</param>
        /// <param name="duration"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int AddTask(Action<object> callBack, object param, int repeat, float duration, float delay)
        {
            var task = new Task();

            task.callBack = callBack;
            task.param = param;
            task.repeat = repeat;
            task.duration = duration;
            task.delay = delay;
            task.passedTime = task.duration;
            task.isRemoved = false;

            task.Id = this.NewTaskID;
            
            this._addingTasks.Add(task);
            return task.Id;
        }

        
        public void RemoveTask(int taskId)
        {
            if (_taskMap.ContainsKey(taskId))
            {
                _taskMap[taskId].isRemoved = true;
                return;
            }
            int idx = _addingTasks.FindIndex(p => p.Id == taskId);
            if (idx >= 0)
            {
                _addingTasks.RemoveAt(idx);
            }
        }
    }
}