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
    }
}
