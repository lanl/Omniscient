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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public struct WUCSRecord
    {
        public DateTime time;
        public int statusA;
        public double mainVoltageA;
        public double batteryTempA;
        public double moduleTempA;
        public double batteryVoltageA;
        public double chargerVoltageA;
        public int statusB;
        public double mainVoltageB;
        public double batteryTempB;
        public double moduleTempB;
        public double batteryVoltageB;
        public double chargerVoltageB;
    }

    public class WUCSParser
    {
        private System.Globalization.CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        const string TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public List<WUCSRecord> Records { get; private set; }

        public WUCSParser()
        {

        }

        public ReturnCode ParseOneRecord(string fileName)
        {
            // Read lines from file
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadLines(fileName, 1024);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            Records = new List<WUCSRecord>();
            WUCSRecord record = new WUCSRecord();
            record.time = DateTime.MinValue;
            DateTime timeStamp;
            string[] tokens;
            char[] splitChars = new char[] { ' ', '\t' };
            foreach (string line in lines)
            {
                tokens = line.Split(splitChars);

                // Try to fail early for auto-configuration
                if (tokens.Length < 6) return ReturnCode.CORRUPTED_FILE;
                try
                {
                    timeStamp = DateTime.ParseExact(line.Substring(9, 19), TIMESTAMP_FORMAT, CULTURE_INFO);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }

                if (tokens[5] != "Vicor") continue; // signature of data line
                if (record.time != timeStamp)
                {
                    if (record.time > DateTime.MinValue)
                    {
                        Records.Add(record);
                        return ReturnCode.SUCCESS;
                    }
                    record = new WUCSRecord();
                    record.time = timeStamp;
                }
                if (tokens[7] == "A")
                {
                    if (tokens[8] == "Status:") record.statusA = int.Parse(tokens[9]);
                    else if (tokens[8] == "Main" && tokens[9] == "Voltage:") record.mainVoltageA = double.Parse(tokens[10]);
                    else if (tokens[8] == "Battery" && tokens[9] == "Temperature:") record.batteryTempA = double.Parse(tokens[10]);
                    else if (tokens[8] == "Module" && tokens[9] == "Temperature:") record.moduleTempA = double.Parse(tokens[10]);
                    else if (tokens[8] == "Battery" && tokens[9] == "Voltage:") record.batteryVoltageA = double.Parse(tokens[10]);
                    else if (tokens[8] == "Charger" && tokens[9] == "Voltage:") record.chargerVoltageA = double.Parse(tokens[10]);
                }
                else if (tokens[7] == "B")
                {
                    if (tokens[8] == "Status:") record.statusA = int.Parse(tokens[9]);
                    else if (tokens[8] == "Main" && tokens[9] == "Voltage:") record.mainVoltageB = double.Parse(tokens[10]);
                    else if (tokens[8] == "Battery" && tokens[9] == "Temperature:") record.batteryTempB = double.Parse(tokens[10]);
                    else if (tokens[8] == "Module" && tokens[9] == "Temperature:") record.moduleTempB = double.Parse(tokens[10]);
                    else if (tokens[8] == "Battery" && tokens[9] == "Voltage:") record.batteryVoltageB = double.Parse(tokens[10]);
                    else if (tokens[8] == "Charger" && tokens[9] == "Voltage:") record.chargerVoltageB = double.Parse(tokens[10]);
                }
            }
            Records.Add(record);
            return ReturnCode.SUCCESS;
        }

        public ReturnCode ParseFile(string fileName)
        {
            // Read lines from file
            string[] lines;
            try
            {
                lines = IOUtility.PermissiveReadAllLines(fileName);
            }
            catch (Exception ex)
            {
                return ReturnCode.COULD_NOT_OPEN_FILE;
            }

            Records = new List<WUCSRecord>();
            WUCSRecord record = new WUCSRecord();
            record.time = DateTime.MinValue;
            DateTime timeStamp;
            string[] tokens;
            char[] splitChars = new char[] { ' ', '\t' };
            foreach (string line in lines)
            {
                tokens = line.Split(splitChars);

                // Try to fail early for auto-configuration
                if (tokens.Length < 6) return ReturnCode.CORRUPTED_FILE;
                try
                { 
                    timeStamp = DateTime.ParseExact(line.Substring(9, 19), TIMESTAMP_FORMAT, CULTURE_INFO);
                }
                catch
                {
                    return ReturnCode.CORRUPTED_FILE;
                }
                
                if (tokens[5] != "Vicor") continue; // signature of data line
                if (record.time != timeStamp)
                {
                    if (record.time > DateTime.MinValue) Records.Add(record);
                    record = new WUCSRecord();
                    record.time = timeStamp;
                }
                if (tokens[7] == "A")
                {
                    if (tokens[8] == "Status:")
                    {
                        try { record.statusA = int.Parse(tokens[9]); }
                        catch { record.statusA = -99; }
                    }
                    else if (tokens[8] == "Main" && tokens[9] == "Voltage:")
                    {
                        try { record.mainVoltageA = double.Parse(tokens[10]); }
                        catch { record.mainVoltageA = -99; }
                    }
                    else if (tokens[8] == "Battery" && tokens[9] == "Temperature:")
                    {
                        try { record.batteryTempA = double.Parse(tokens[10]); }
                        catch { record.batteryTempA = -99; }
                    }
                    else if (tokens[8] == "Module" && tokens[9] == "Temperature:")
                    {
                        try { record.moduleTempA = double.Parse(tokens[10]); }
                        catch { record.moduleTempA = -99; }
                    }
                    else if (tokens[8] == "Battery" && tokens[9] == "Voltage:")
                    {
                        try { record.batteryVoltageA = double.Parse(tokens[10]); }
                        catch { record.batteryVoltageA = -99; }
                    }
                    else if (tokens[8] == "Charger" && tokens[9] == "Voltage:")
                    {
                        try { record.chargerVoltageA = double.Parse(tokens[10]); }
                        catch { record.chargerVoltageA = -99; }
                    }
                }
                else if (tokens[7] == "B")
                {
                    if (tokens[8] == "Status:")
                    {
                        try { record.statusB = int.Parse(tokens[9]); }
                        catch { record.statusB = -99; }
                    }
                    else if (tokens[8] == "Main" && tokens[9] == "Voltage:")
                    {
                        try { record.mainVoltageB = double.Parse(tokens[10]); }
                        catch { record.mainVoltageB = -99; }
                    }
                    else if (tokens[8] == "Battery" && tokens[9] == "Temperature:")
                    {
                        try { record.batteryTempB = double.Parse(tokens[10]); }
                        catch { record.batteryTempB = -99; }
                    }
                    else if (tokens[8] == "Module" && tokens[9] == "Temperature:")
                    {
                        try { record.moduleTempB = double.Parse(tokens[10]); }
                        catch { record.moduleTempB = -99; }
                    }
                    else if (tokens[8] == "Battery" && tokens[9] == "Voltage:")
                    {
                        try { record.batteryVoltageB = double.Parse(tokens[10]); }
                        catch { record.batteryVoltageB = -99; }
                    }
                    else if (tokens[8] == "Charger" && tokens[9] == "Voltage:")
                    {
                        try { record.chargerVoltageB = double.Parse(tokens[10]); }
                        catch { record.chargerVoltageB = -99; }
                    }
                }
            }
            Records.Add(record);
            return ReturnCode.SUCCESS;
        }
    }
}
