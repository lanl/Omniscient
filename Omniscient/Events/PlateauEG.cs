/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2020, International Atomic Energy Agency (IAEA), IAEA.org

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    class PlateauEG : EventGenerator
    {
        Channel signalChannel;
        Channel sigmaChannel;
        int minPoints;
        int edgePoints;
        double nSigma;
        double threshold;

        public PlateauEG(DetectionSystem parent, string newName, 
            Channel newSignalChannel, Channel newSigmaChannel, 
            int newMinPoints, int newEdgePoints, double newNSigma, double newThreshold, uint id) : 
            base(parent, newName, id)
        {
            eventGeneratorType = "Plateau";
            signalChannel = newSignalChannel;
            sigmaChannel = newSigmaChannel;
            minPoints = newMinPoints;
            edgePoints = newEdgePoints;
            nSigma = newNSigma;
            threshold = newThreshold;
        }

        public override List<Event> GenerateEvents(DateTime start, DateTime end)
        {
            signalChannel.GetInstrument().LoadData(ChannelCompartment.Process, start, end);
            List<DateTime> times = signalChannel.GetTimeStamps(ChannelCompartment.Process);
            List<double> vals = signalChannel.GetValues(ChannelCompartment.Process);
            List<TimeSpan> durations = null;
            if (signalChannel.GetChannelType() == Channel.ChannelType.DURATION_VALUE)
                durations = signalChannel.GetDurations(ChannelCompartment.Process);
            List<double> sigmas = sigmaChannel.GetValues(ChannelCompartment.Process);
            events = new List<Event>();
            
            // Validation
            if (vals.Count != sigmas.Count) return events;
            if (minPoints < 3) return events;
            if (vals.Count <= minPoints) return events;

            Event eve = new Event(this);        // Really shouldn't need to make an event here but visual studio freaks out without it
            
            double runningAverage = 0;
            double runningSum = 0;
            int nAveragePoints = minPoints;
            
            // Boundary case
            bool inEvent = true;        // Assume true unless proven false below
            int eventStartIndex = 0;
            int eventEndIndex = 0;
            for (int j = 0; j < minPoints; j++) runningSum += vals[j];
            runningAverage = runningSum / nAveragePoints;
            for (int j = 0; j < minPoints; j++)
            {
                if (vals[j] < threshold || 
                    vals[j] < (runningAverage - nSigma * sigmas[j]) ||
                    vals[j] > (runningAverage + nSigma * sigmas[j]))
                {
                    inEvent = false;
                    break;
                }
            }

            bool eventCheck;
            int i = minPoints;
            while (i < times.Count)
            {
                if (!inEvent)
                {
                    runningSum -= vals[i - minPoints];
                    runningSum += vals[i];
                    runningAverage = runningSum / nAveragePoints;

                    if (vals[i] < threshold)
                    {
                        i++;
                        continue;
                    }

                    eventCheck = true;
                    for (int j = i; j > i - minPoints; j--)
                    {
                        if (vals[i] < threshold ||
                            vals[i] < (runningAverage - nSigma * sigmas[i]) ||
                            vals[i] > (runningAverage + nSigma * sigmas[i]))
                        {
                            eventCheck = false;
                            break;
                        }
                    }

                    // New event
                    if (eventCheck)
                    {
                        eventStartIndex = i - minPoints;
                        inEvent = true;
                    }
                }
                else
                {
                    runningSum += vals[i];
                    nAveragePoints += 1;
                    runningAverage = runningSum / nAveragePoints;
                    eventCheck = true;
                    if (vals[i] < threshold ||
                            vals[i] < (runningAverage - nSigma * sigmas[i]) ||
                            vals[i] > (runningAverage + nSigma * sigmas[i]))
                    {
                        eventCheck = false;
                    }

                    if (!eventCheck)
                    {
                        eventEndIndex = i - 1;
                        eve = new Event(this);
                        int newEnd = FinalizeEvent(eve, eventStartIndex, eventEndIndex, vals, sigmas, times);
                        if (newEnd > 0)
                        {
                            // Event complete
                            events.Add(eve);
                            i = newEnd + minPoints + 1;
                            if (i >= times.Count) break;

                            runningSum = 0;
                            nAveragePoints = minPoints;
                            int regionStart = i - minPoints + 1;
                            eventStartIndex = regionStart;
                            for (int j = regionStart; j <= i; j++) runningSum += vals[j];
                            runningAverage = runningSum / nAveragePoints;
                            for (int j = regionStart; j <= i; j++)
                            {
                                if (vals[j] < threshold ||
                                    vals[j] < (runningAverage - nSigma * sigmas[j]) ||
                                    vals[j] > (runningAverage + nSigma * sigmas[j]))
                                {
                                    inEvent = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Event failed - 
                            i = eventStartIndex + minPoints + 1;
                            if (i >= times.Count) break;

                            eventStartIndex = i - minPoints;

                            runningSum = 0;
                            nAveragePoints = minPoints;
                            int regionStart = i - minPoints + 1;
                            eventStartIndex = regionStart;
                            for (int j = regionStart; j <= i; j++) runningSum += vals[j];
                            runningAverage = runningSum / nAveragePoints;
                            for (int j = regionStart; j <= i; j++)
                            {
                                if (vals[j] < threshold ||
                                    vals[j] < (runningAverage - nSigma * sigmas[j]) ||
                                    vals[j] > (runningAverage + nSigma * sigmas[j]))
                                {
                                    inEvent = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                i++;
            }
            if (inEvent)
            {
                eventEndIndex = times.Count - 1;
                eve = new Event(this);
                int newEnd = FinalizeEvent(eve, eventStartIndex, eventEndIndex, vals, sigmas, times);
                if (newEnd > 0) events.Add(eve);
            }
            return events;
        }

        /// <summary>
        /// Returns -1 if fail or the end index if success
        /// </summary>
        private int FinalizeEvent(Event eve, int eventStartIndex, int eventEndIndex,
            List<double> vals, List<double> sigmas, List<DateTime> times)
        {
            const int FAIL = -1;
            int nPoints = eventEndIndex - eventStartIndex;
            int bonusPoints = nPoints - (minPoints + 2 * edgePoints);
            if (bonusPoints < 0) return FAIL;

            double runningSum = 0;
            double runningAverage = 0;
            int count;
            int newStart;
            int newEnd;
            bool eventCheck;

            if (bonusPoints == 0)
            {
                // Minimum event: all points must pass
                newStart = eventStartIndex + edgePoints;
                newEnd = eventEndIndex - edgePoints;
            }
            else
            {
                // More than the minimum number of points: start at the middle and go out
                int midPoint = (eventStartIndex + eventEndIndex) / 2;
                int bonusRadius = bonusPoints / 2 - bonusPoints % 2;
                int startRadius = (bonusRadius / 2 - bonusRadius % 2) + minPoints / 2;
                newStart = midPoint - startRadius;
                newEnd = midPoint + startRadius;
            }

            // Core check
            for (int i = newStart; i <= newEnd; i++) runningSum += vals[i];
            count = newEnd - newStart + 1;
            runningAverage = runningSum/count;
            eventCheck = true;
            for (int i = newStart; i <= newEnd; i++)
            {

                if (vals[i] < threshold ||
                        vals[i] < (runningAverage - nSigma * sigmas[i]) ||
                        vals[i] > (runningAverage + nSigma * sigmas[i]))
                {
                    eventCheck = false;
                    break;
                }
            }
            if (!eventCheck) return FAIL;

            // Wing check
            int lowWing = 0;
            int highWing = 0;
            bool lowWingPass = true;
            bool highWingPass = true;
            while (lowWingPass || highWingPass)
            {
                // Low wing
                lowWing = newStart - 1;
                if (lowWing >= eventStartIndex)
                {
                    if (vals[lowWing] < threshold ||
                        vals[lowWing] < (runningAverage - nSigma * sigmas[lowWing]) ||
                        vals[lowWing] > (runningAverage + nSigma * sigmas[lowWing]))
                    {
                        lowWingPass = false;
                    }
                    else
                    {
                        newStart = lowWing;
                        runningSum += vals[lowWing];
                        count++;
                        runningAverage = runningSum / count;
                        lowWingPass = true;
                    }
                }
                else lowWingPass = false;

                // High wing
                highWing = newEnd + 1;
                if (highWing < vals.Count)
                {
                    if (vals[highWing] < threshold ||
                        vals[highWing] < (runningAverage - nSigma * sigmas[highWing]) ||
                        vals[highWing] > (runningAverage + nSigma * sigmas[highWing]))
                    {
                        highWingPass = false;
                    }
                    else
                    {
                        newEnd = highWing;
                        runningSum += vals[highWing];
                        count++;
                        runningAverage = runningSum / count;
                        highWingPass = true;
                    }
                }
                else highWingPass = false;
            }

            // Start and end times established
            newStart += edgePoints;
            newEnd -= edgePoints;
            double maximum = double.MinValue;
            int maxIndex = -1;
            runningSum = 0;
            for (int i = newStart; i <= newEnd; i++)
            {
                if (vals[i] > maximum)
                {
                    maxIndex = i;
                    maximum = vals[i];
                }
                runningSum += vals[i];
            }
            count = newEnd - newStart + 1;
            runningAverage = runningSum / count;

            eve.Comment = signalChannel.Name + " has a plateau.";
            eve.StartTime = times[newStart];
            eve.EndTime = times[newEnd];
            eve.MaxTime = times[maxIndex];
            eve.MaxValue = maximum;
            eve.MeanValue = runningAverage;

            return newEnd;
        }

        public override List<Parameter> GetParameters()
        {
            // Channel newSignalChannel, Channel newSigmaChannel, 
            // int newMinPoints, int newEdgePoints, double newNSigma, double newThreshold,

            List<Parameter> parameters = new List<Parameter>()
            {
                new SystemChannelParameter("Signal Channel", (DetectionSystem)eventWatcher){ Value = signalChannel.Name },
                new SystemChannelParameter("Sigma Channel", (DetectionSystem)eventWatcher){ Value = sigmaChannel.Name },
                new IntParameter("Min Points"){ Value = minPoints.ToString() },
                new IntParameter("Edge Points"){ Value = edgePoints.ToString() },
                new DoubleParameter("N Sigma") { Value = nSigma.ToString() },
                new DoubleParameter("Threshold") { Value = threshold.ToString() }
            };
            return parameters;
        }
    }

    public class PlateauEGHookup : EventGeneratorHookup
    {
        public PlateauEGHookup()
        {
            TemplateParameters = new List<ParameterTemplate>()
            {
                new ParameterTemplate("Signal Channel", ParameterType.SystemChannel),
                new ParameterTemplate("Sigma Channel", ParameterType.SystemChannel),
                new ParameterTemplate("Min Points", ParameterType.Int),
                new ParameterTemplate("Edge Points", ParameterType.Int),
                new ParameterTemplate("N Sigma", ParameterType.Double),
                new ParameterTemplate("Threshold", ParameterType.Double)
            };
        }

        public override string Type { get { return "Plateau"; } }

        public override EventGenerator FromParameters(DetectionSystem parent, string newName, List<Parameter> parameters, uint id)
        {
            Channel signalChannel = null;
            Channel sigmaChannel = null;
            int minPoints = 3;
            int edgePoints = 0;
            double threshold = 0;
            double nSigma = 3;
            TimeSpan debounceTime = TimeSpan.FromTicks(0);
            foreach (Parameter param in parameters)
            {
                switch (param.Name)
                {
                    case "Signal Channel":
                        signalChannel = ((SystemChannelParameter)param).ToChannel();
                        break;
                    case "Sigma Channel":
                        sigmaChannel = ((SystemChannelParameter)param).ToChannel();
                        break;
                    case "Min Points":
                        minPoints = ((IntParameter)param).ToInt();
                        break;
                    case "Edge Points":
                        edgePoints = ((IntParameter)param).ToInt();
                        break;
                    case "N Sigma":
                        nSigma = ((DoubleParameter)param).ToDouble();
                        break;
                    case "Threshold":
                        threshold = ((DoubleParameter)param).ToDouble();
                        break;
                }
            }
            return new PlateauEG(parent, newName, signalChannel, sigmaChannel, minPoints, edgePoints, nSigma, threshold, id);
        }
    }
}

