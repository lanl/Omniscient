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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omniscient;

namespace OmniscientTests
{
    [TestClass]
    public class CacheTest
    {
        [TestMethod]
        public void DateCompactWorks()
        {
            List<DateTime> times = new List<DateTime>()
            {
                new DateTime(91),
                new DateTime(91),
                new DateTime(91),
                new DateTime(91),
                new DateTime(90),
                new DateTime(91),
                new DateTime(90),
                new DateTime(92),
                new DateTime(93),
                new DateTime(94),
                new DateTime(95),
                new DateTime(96),
                new DateTime(97),
                new DateTime(96),
                new DateTime(95),
                new DateTime(96),
            };

            List<long> cTimes = IOUtility.DateCompact(times);

            Assert.AreEqual(cTimes[0], 91);
            Assert.AreEqual(cTimes[1], 91);
            Assert.AreEqual(cTimes[2], 91);
            Assert.AreEqual(cTimes[3], 1);
            Assert.AreEqual(cTimes[4], 90);
            Assert.AreEqual(cTimes[5], 91);
            Assert.AreEqual(cTimes[6], 90);
            Assert.AreEqual(cTimes[7], 92);
            Assert.AreEqual(cTimes[8], 93);
            Assert.AreEqual(cTimes[9], 94);
            Assert.AreEqual(cTimes[10], 3);
            Assert.AreEqual(cTimes[11], 96);
            Assert.AreEqual(cTimes[12], 95);
            Assert.AreEqual(cTimes[13], 96);
        }

        [TestMethod]
        public void DateUnpackWorks()
        {
            List<long> ticks = new List<long>()
            {
                91,91,91,1,90,91,90,92,93,94,3,96,95,96
            };

            List<DateTime> times = IOUtility.DateUnpack(ticks);

            Assert.AreEqual(times[0].Ticks, 91);
            Assert.AreEqual(times[1].Ticks, 91);
            Assert.AreEqual(times[2].Ticks, 91);
            Assert.AreEqual(times[3].Ticks, 91);
            Assert.AreEqual(times[4].Ticks, 90);
            Assert.AreEqual(times[5].Ticks, 91);
            Assert.AreEqual(times[6].Ticks, 90);
            Assert.AreEqual(times[7].Ticks, 92);
            Assert.AreEqual(times[8].Ticks, 93);
            Assert.AreEqual(times[9].Ticks, 94);
            Assert.AreEqual(times[10].Ticks, 95);
            Assert.AreEqual(times[11].Ticks, 96);
            Assert.AreEqual(times[12].Ticks, 97);
            Assert.AreEqual(times[13].Ticks, 96);
            Assert.AreEqual(times[14].Ticks, 95);
            Assert.AreEqual(times[15].Ticks, 96);
        }
    }
}
