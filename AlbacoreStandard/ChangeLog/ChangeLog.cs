using System;

namespace Albacore.ChangeLog
{
    public class ChangeLog
    {
        public int ID { get; set; }
        public string ScriptName { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
