using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Scripty.MsBuild
{
    public class ScriptyTask : Task
    {
        [Required]
        public string ProjectRoot { get; set; }

        public ITaskItem[] ScriptFiles { get; set; }

        public override bool Execute()
        {
            Log.LogMessage($"Script files: {String.Join(",", ScriptFiles.Select(x => x.ItemSpec))}");
            return true;
        }
    }
}
