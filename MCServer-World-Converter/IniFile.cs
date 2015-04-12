using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MCServer_World_Converter
{
    class IniFile
    {
        private string _path;

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
         SetLastError = true,
         CharSet = CharSet.Unicode, ExactSpelling = true,
         CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          string lpReturnString,
          int nSize,
          string lpFilename);
        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
          SetLastError = true,
          CharSet = CharSet.Unicode, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFilename);

        public IniFile(string iniPath)
        {
            _path = iniPath;
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public List<string> Sections
        {
            get
            {
                string returnString = new string(' ', 65536);
                GetPrivateProfileString(null, null, null, returnString, 65536, _path);
                List<string> result = new List<string>(returnString.Split('\0'));
                result.RemoveRange(result.Count - 2, 2);
                return result;
            }
        }

        public List<string> GetKeys(string section)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(section, null, null, returnString, 32768, _path);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        public string ReadValue(string section, string key, string def)
        {
            string returnString = new string(' ', 1024);
            int i = GetPrivateProfileString(section, key, def, returnString, 1024, this.Path);
            return returnString;
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.Path);
        }
    }
}
