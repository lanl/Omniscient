// This software is open source software available under the BSD-3 license.
// 
// Copyright (c) 2018, Triad National Security, LLC
// All rights reserved.
// 
// Copyright 2018. Triad National Security, LLC. This software was produced under U.S. Government contract 89233218CNA000001 for Los Alamos National Laboratory (LANL), which is operated by Triad National Security, LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR TRIAD NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse it with the version available from LANL.
// 
// Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 1.       Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// 2.       Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// 3.       Neither the name of Triad National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
//  
// THIS SOFTWARE IS PROVIDED BY TRIAD NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL TRIAD NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Omniscient
{
    /// <summary>
    /// Runs CacheTasks in order of urgency.
    /// </summary>
    public class CacheManager
    {
        public enum CacheManagerState { Stopped, Running, Stopping }
        private CacheManagerState _state;
        public CacheManagerState State
        {
            get { return _state; }
            private set
            {
                _state = value;
                StateChanged?.Invoke(this, new EventArgs());
            }
        }

        private List<InstrumentCache> InstrumentCaches;

        private object TasksLock = new object();
        private LinkedList<CacheTask> Tasks;
        private CacheTask TaskAtHand;

        /// <summary>
        /// Called when State changes
        /// </summary>
        public event EventHandler StateChanged;

        public CacheManager()
        {
            // Make sure a Cache directory exists
            if (!Directory.Exists("Cache")) Directory.CreateDirectory("Cache");

            InstrumentCaches = new List<InstrumentCache>();

            Tasks = new LinkedList<CacheTask>();
            TaskAtHand = null;
        }

        public void AddInstrumentCache(InstrumentCache cache)
        {
            InstrumentCaches.Add(cache);
            AddUrgentTask(new InstrumentCacheScanTask(this, cache));
        }

        public void RemoveInstrumentCache(InstrumentCache cache)
        {
            InstrumentCaches.Remove(cache);
        }

        /// <summary>
        /// Adds a task that is needed urgently.
        /// </summary>
        /// <param name="cacheTask"></param>
        public void AddUrgentTask(CacheTask cacheTask)
        {
            Tasks.AddFirst(cacheTask);
        }

        public void AddNonurgentTask(CacheTask cacheTask)
        {
            Tasks.AddLast(cacheTask);
        }

        /// <summary>
        /// Starts the main loop thread.
        /// </summary>
        public void Start()
        {
            if (State == CacheManagerState.Stopped)
            {
                Thread mainThread = new Thread(MainLoop);
                mainThread.Start();
            }
        }

        /// <summary>
        /// Signals the main loop to stop.
        /// </summary>
        public void Stop()
        {
            if (State == CacheManagerState.Running)
                State = CacheManagerState.Stopping;
        }

        /// <summary>
        /// The main loop in the CacheManager. Starts tasks as others finish.
        /// </summary>
        public void MainLoop()
        {
            State = CacheManagerState.Running;
            while (State != CacheManagerState.Stopped)
            {
                if (State==CacheManagerState.Stopping)
                {
                    if (TaskAtHand != null)
                    {
                        TaskAtHand.Abort();
                    }
                    break;
                }

                if(TaskAtHand == null)
                {
                    if (Tasks.Count > 0)
                        StartNextTask();
                    else
                        Thread.Sleep(100);
                }
                else
                {
                    switch (TaskAtHand.State)
                    {
                        case CacheTask.CacheTaskState.Running:
                            Thread.Sleep(100);
                            break;
                        case CacheTask.CacheTaskState.Stopping:
                            Thread.Sleep(10);
                            break;
                        case CacheTask.CacheTaskState.Aborted:
                        case CacheTask.CacheTaskState.Complete:
                            StartNextTask();
                            break;
                    }
                }
            }
            State = CacheManagerState.Stopped;
        }


        /// <summary>
        /// Starts running the next task in the list
        /// </summary>
        public void StartNextTask()
        {
            lock (TasksLock)
            {
                if (TaskAtHand != null)
                {
                    Tasks.Remove(TaskAtHand);
                }
                if (Tasks.Count > 0)
                {
                    TaskAtHand = Tasks.First.Value;
                }
                else
                {
                    TaskAtHand = null;
                }
            }
            TaskAtHand?.Start();
        }
    }

    public abstract class CacheTask
    {
        public enum CacheTaskState { NotStarted, Running, Stopping, Aborted, Complete };
        private CacheTaskState _state;
        public CacheTaskState State
        {
            get { return _state; }
            protected set
            {
                _state = value;
                if (value == CacheTaskState.Complete)
                {
                    OnComplete?.Invoke(this, new EventArgs());
                }
            }
        }
        public Thread TaskThread { get; protected set; }

        /// <summary>
        /// Invoked when task is complete.
        /// </summary>
        public event EventHandler OnComplete;

        public CacheManager Manager { get; private set; }

        public CacheTask(CacheManager manager)
        {
            Manager = manager;
            State = CacheTaskState.NotStarted;
        }

        public void Abort()
        {

        }

        public void Start()
        {
            State = CacheTaskState.Running;
            TaskThread = new Thread(RunTask);
            TaskThread.Start();
        }

        public abstract void RunTask();
    }
}
