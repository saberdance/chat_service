using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsubasa
{
    public class Sensitive
    {
        public static HashSet<string> SensitiveWords = null;

        public static bool InitWords(string wordFilePath)
        {
            if (SensitiveWords != null)
            {
                return false;
            }
            try
            {
                return LoadSensitiveFromFile(wordFilePath);
            }
            catch
            {
                return false;
            }
        }

        private static bool LoadSensitiveFromFile(string wordFilePath)
        {
            if (!File.Exists(wordFilePath))
            {
                return false;
            }
            SensitiveWords = new();
            using (StreamReader sr = new(wordFilePath))
            {
                string oneline;
                while ((oneline = sr.ReadLine()) != null)
                {
                    SensitiveWords.Add(oneline);
                }
            }
            return true;
        }

        //敏感词查询
        public static bool Valid(string s)
        {
            if (SensitiveWords == null)
            {
                throw new Exception("未初始化敏感词库");
            }
            return !SensitiveWords.Where(o => s.Contains(o)).Any();
        }
    }
}
