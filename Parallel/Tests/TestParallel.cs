#region Copyright (c) 2009 MEDIT S.A.
//
// Filename: TestParallel.cs
//
// This file may be used under the terms of the 2-clause BSD license:
//
// Copyright (c) 2009, Stewart A. Adcock <stewart@adcock.org.uk>
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list
//      of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this
//      list of conditions and the following disclaimer in the documentation and/or other
//      materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
// SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
// THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// Revision History
// Version  date        author      changes
// 1.0      2008-05-01  Stewart     Tests for Parallel.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using NUnit.Framework;

using Uk.Org.Adcock.Parallel;

namespace Uk.Org.Adcock.Parallel.Tests
{
  /// <summary>
  /// Test the Parallel class
  /// </summary>
  [TestFixture]
  public class TestParallel
  {
    #region ThreadsCount
    /// <summary>
    /// Tests ThreadsCount.
    /// </summary>
    [Test]
    public void TestThreadsCount()
    {
      Parallel.ThreadsCount = 5;
      Assert.AreEqual(5, Parallel.ThreadsCount);
      Parallel.ThreadsCount = 10;
      Assert.AreEqual(10, Parallel.ThreadsCount);
    }

    /// <summary>
    /// Tests ThreadsCount.
    /// </summary>
    [Test]
    public void TestThreadsCountSetToZeroResetsDefault()
    {
      Parallel.ThreadsCount = 0;
      Assert.AreEqual(System.Environment.ProcessorCount, Parallel.ThreadsCount);
    }
    #endregion

    #region Parallel.For
    /// <summary>
    /// Tests For.
    /// </summary>
    [Test]
    public void TestForIterations()
    {
      int[] iterationCount = new int[5];

      Parallel.For(0, 5, delegate(int i)
      {
        iterationCount[i]++;
      });

      foreach (int i in iterationCount)
      {
        Assert.AreEqual(1, iterationCount[i], "Iteraction " + i + " not performed exactly once.");
      }
    }

    /// <summary>
    /// Tests For and whether the number of threads used is correct.
    /// </summary>
    [Test]
    public void TestForCountThreads()
    {
      List<int> items = new List<int>();
      for (int i = 0; i < 5; i++)
      {
        items.Add(i);
      }

      Parallel.ThreadsCount = 3;
      List<int> threadIds = new List<int>();

      Parallel.For(0, 5, delegate(int i)
      {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        lock (threadIds)
        {
          if (threadIds.Contains(threadId) == false)
          {
            threadIds.Add(threadId);
          }
        }
        Thread.Sleep(20);
      });

      Assert.AreEqual(3, threadIds.Count, "Incorrect number of threads used.");
    }

    /// <summary>
    /// Tests For and whether the number of threads used is correct.
    /// </summary>
    [Test]
    public void TestForCountThreads1()
    {
      List<int> items = new List<int>();
      for (int i = 0; i < 5; i++)
      {
        items.Add(i);
      }

      Parallel.ThreadsCount = 1;
      List<int> threadIds = new List<int>();
      threadIds.Add(Thread.CurrentThread.ManagedThreadId);

      Parallel.For(0, 5, delegate(int i)
      {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        lock (threadIds)
        {
          if (threadIds.Contains(threadId) == false)
          {
            threadIds.Add(threadId);
          }
        }
        Thread.Sleep(20);
      });

      Assert.AreEqual(1, threadIds.Count, "More than one thread used.");
    }
    #endregion

    #region Parallel.ForEach
    /// <summary>
    /// Tests ForEach.
    /// </summary>
    [Test]
    public void TestForEachIterations()
    {
      int[] iterationCount = new int[5];
      List<int> items = new List<int>();
      for (int i = 0; i < 5; i++)
      {
        items.Add(i);
      }

      Parallel.ForEach<int>(items, delegate(int i)
      {
        iterationCount[i]++;
      });

      foreach (int i in iterationCount)
      {
        Assert.AreEqual(1, iterationCount[i], "Iteraction " + i + " not performed exactly once.");
      }
    }

    /// <summary>
    /// Tests ForEach and whether the number of threads used is correct.
    /// </summary>
    [Test]
    public void TestForEachCountThreads()
    {
      List<int> items = new List<int>();
      for (int i = 0; i < 5; i++)
      {
        items.Add(i);
      }

      Parallel.ThreadsCount = 3;
      List<int> threadIds = new List<int>();

      Parallel.ForEach<int>(items, delegate(int i)
      {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        lock (threadIds)
        {
          if (threadIds.Contains(threadId) == false)
          {
            threadIds.Add(threadId);
          }
        }
        Thread.Sleep(100);
      });

      Assert.AreEqual(3, threadIds.Count, "Incorrect number of threads used.");
    }

    /// <summary>
    /// Tests ForEach and whether the number of threads used is correct.
    /// </summary>
    [Test]
    public void TestForEachCountThreads1()
    {
      List<int> items = new List<int>();
      for (int i = 0; i < 5; i++)
      {
        items.Add(i);
      }

      Parallel.ThreadsCount = 1;
      List<int> threadIds = new List<int>();
      threadIds.Add(Thread.CurrentThread.ManagedThreadId);

      Parallel.ForEach<int>(items, delegate(int i)
      {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        lock (threadIds)
        {
          if (threadIds.Contains(threadId) == false)
          {
            threadIds.Add(threadId);
          }
        }
        Thread.Sleep(20);
      });

      Assert.AreEqual(1, threadIds.Count, "More than one thread used.");
    }
    #endregion
  }
}
