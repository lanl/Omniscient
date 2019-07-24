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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class DataMonitor
    {
        const string HEADER = "File Path,Date/Time of Access,Hash,Bytes as of Last Access,Processed Bytes,Date/Time of Modification as of Last Access,Data Start Date/Time,Data End Date/Time";

        readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        public string FileListFile { get; set; }
        public string DirectoryToMonitor { get; set; }
        public bool ScanRecursively { get; set; }

        public List<FileScan> FileList { get; private set; }
        List<FileScan> MissingFiles { get; set; }

        public string FilePrefix { get; set; }
        public string FileSuffix { get; set; }
        
        /// <summary>
        /// In LazyMode, hashes are not generated
        /// </summary>
        public bool LazyMode { get; set; }

        public DataMonitor()
        {
            FileList = new List<FileScan>();
            MissingFiles = new List<FileScan>();
            FilePrefix = "";
            FileSuffix = "";
            ScanRecursively = true;
            LazyMode = false;
        }


        /// <summary>
        /// Update or insert a new 
        /// </summary>
        /// <param name="updatedEntry"></param>
        public void UpdateListEntry(FileScan updatedEntry)
        {
            int index = FindInList(updatedEntry.FileName);
            if (index >= 0)
            {
                FileList.RemoveAt(index);
                FileList.Insert(index, updatedEntry);
            }
            else
            {
                FileList.Add(updatedEntry);
            }
        }

        /// <summary>
        /// Returns FileScans that are not consistent with the FileList
        /// </summary>
        /// <returns></returns>
        public List<FileScan> Scan()
        {
            int listCount = FileList.Count;
            List<FileScan> scanResults = ScanDirectory(DirectoryToMonitor);
            List<FileScan> outOfDateResults = new List<FileScan>(scanResults.Count);

            bool[] accountedFor = new bool[listCount];
            for (int i = 0; i < listCount; i++) accountedFor[i] = false;

            // Find all out of date and deleted files
            int index;
            for (int i=0; i<scanResults.Count; i++)
            {
                FileScan scanResult = scanResults[i];
                index = FindInList(scanResult.FileName);
                if (index < 0)
                {
                    outOfDateResults.Add(scanResult);
                }
                else
                if (LazyMode)
                {
                    if(FileList[index].ProcessedBytes < scanResult.Bytes ||
                        FileList[index].Bytes != scanResult.Bytes || 
                        FileList[index].TimeOfModification != scanResult.TimeOfModification)
                    {
                        scanResult.ProcessedBytes = FileList[index].ProcessedBytes;
                        outOfDateResults.Add(scanResult);
                        accountedFor[index] = true;
                    }
                    else
                    {
                        accountedFor[index] = true;
                    }
                }
                else
                {
                    if (FileList[index].ProcessedBytes < scanResult.Bytes || 
                        FileList[index].Hash != scanResult.Hash)
                    {
                        scanResult.ProcessedBytes = FileList[index].ProcessedBytes;
                        outOfDateResults.Add(scanResult);
                        accountedFor[index] = true;
                    }
                    else
                    {
                        accountedFor[index] = true;
                    }
                }
            }

            MissingFiles.Clear();
            for (int i = 0; i < listCount; i++)
            {
                if(!accountedFor[i])
                {
                    MissingFiles.Add(FileList[i]);
                }
            }
            return outOfDateResults;
        }
        
        /// <summary>
        /// This should really be turned into a binary search...
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private int FindInList(string fileName)
        {
            for (int i=0; i<FileList.Count;i++)
            {
                if (FileList[i].FileName == fileName) return i;
            }

            return -1;
        }

        public List<FileScan> ScanDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName)) return new List<FileScan>();
            string[] files = Directory.GetFiles(directoryName);
            List<FileScan> entries = new List<FileScan>(files.Length);
            foreach (string file in files)
            {
                if (Path.GetFileName(file).ToLower().StartsWith(FilePrefix.ToLower()) && file.ToLower().EndsWith(FileSuffix.ToLower()))
                {
                    entries.Add(ScanFile(file));
                }
            }

            if(ScanRecursively)
            {
                string[] directories = Directory.GetDirectories(directoryName);
                foreach(string directory in directories)
                {
                    entries.AddRange(ScanDirectory(directory));
                }
            }
            return entries;
        }

        public FileScan ScanFile(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            FileScan entry = new FileScan
            {
                FileName = fileName,
                TimeOfLastAccess = DateTime.Now,
                Bytes = info.Length,
                ProcessedBytes = 0,
            };
            if(LazyMode)
            {
                entry.Hash = "";
            }
            else
            {
                entry.Hash = HashFile(fileName);
            }
            long ticks = info.LastWriteTime.Ticks / OneSecond.Ticks;
            entry.TimeOfModification = new DateTime(ticks * OneSecond.Ticks);
            return entry;
        }

        public string HashFile(string fileName)
        {
            using (BufferedStream stream = new BufferedStream(File.OpenRead(fileName), 1200000))
            {
                SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
                byte[] hash = provider.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }

        public bool LoadFileListFile()
        {
            // Make the file if it doesn't exist
            if (!File.Exists(FileListFile))
            {
                File.WriteAllLines(FileListFile, new string[] { HEADER });
            }

            // Read the file from disk
            string[] lines;
            try
            {
                lines = File.ReadAllLines(FileListFile);
            }
            catch
            {
                return false;
            }
            FileList.Clear();
            if (lines.Length < 2) return true;

            FileScan entry;
            string[] tokens;
            for (int i=1; i<lines.Length; i++)
            {
                tokens = lines[i].Split(',');
                entry = new FileScan();
                entry.FileName = tokens[0];
                entry.TimeOfLastAccess = DateTime.Parse(tokens[1]);
                entry.Hash = tokens[2];
                entry.Bytes = long.Parse(tokens[3]);
                entry.ProcessedBytes = long.Parse(tokens[4]);
                entry.TimeOfModification = DateTime.Parse(tokens[5]);
                entry.DataStartTime = DateTime.Parse(tokens[6]);
                entry.DataEndTime = DateTime.Parse(tokens[7]);
                FileList.Add(entry);
            }
            return true;
        }

        public bool SaveFileListFile()
        {
            string[] lines = new string[FileList.Count + 1];
            lines[0] = HEADER;
            for (int i=0; i<FileList.Count; i++)
            {
                lines[i + 1] = FileList[i].FileName + "," +
                    FileList[i].TimeOfLastAccess.ToString("MM/dd/yyyy HH:mm:ss") + "," +
                    FileList[i].Hash + "," +
                    FileList[i].Bytes.ToString() + "," +
                    FileList[i].ProcessedBytes.ToString() + "," +
                    FileList[i].TimeOfModification.ToString("MM/dd/yyyy HH:mm:ss") + "," +
                    FileList[i].DataStartTime.ToString("MM/dd/yyyy HH:mm:ss.fff") + "," +
                    FileList[i].DataEndTime.ToString("MM/dd/yyyy HH:mm:ss.fff");
            }
            try
            {
                File.WriteAllLines(FileListFile, lines);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public struct FileScan
    {
        public string FileName;
        public DateTime TimeOfLastAccess;
        public string Hash;
        public long Bytes;
        public long ProcessedBytes;
        public DateTime TimeOfModification;
        public DateTime DataStartTime;
        public DateTime DataEndTime;
    }
}
