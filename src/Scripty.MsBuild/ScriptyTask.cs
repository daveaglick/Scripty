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

        [Output]
        public ITaskItem[] GeneratedFiles
        {
            get
            {
                return Array.Empty<ITaskItem>();
            }
        }

        public override bool Execute()
        {
            Log.LogError($"Script files: {String.Join(",", ScriptFiles.Select(x => x.ItemSpec))}");
            return true;
        }
    }
}
