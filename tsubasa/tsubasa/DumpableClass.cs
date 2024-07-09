using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsubasa
{
    public class DumpableClass
    {
        public virtual void Dump()
        {
            System.Reflection.PropertyInfo[] properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            Logger.ConsoleLog("[DUMP]" + this.GetType().ToString());
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.GetValue(this) is DumpableClass)
                {
                    Logger.ConsoleLog("Class [" + property.Name + "]:");
                    (property.GetValue(this) as DumpableClass).Dump();
                }
                else
                {
                    Logger.ConsoleLog("[" + property.Name + "]:" + property.GetValue(this));
                }
                
            }
        }
    }
}
