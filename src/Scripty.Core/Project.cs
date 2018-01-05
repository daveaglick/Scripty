using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis;

namespace Scripty.Core
{
    public class Project
    {
        private readonly object _compileLock = new object();
        private readonly object _analyzeLock = new object();

        private readonly ScriptEngine _engine;

        private ProjectAnalyzer _projectAnalyzer;
        private Microsoft.Build.Evaluation.Project _buildProject;
        private ProjectInstance _projectInstance;
        private AdhocWorkspace _workspace;
        private Microsoft.CodeAnalysis.Project _analysisProject;

        internal Project(ScriptEngine engine)
        {
            _engine = engine;
        }

        private void Compile()
        {
            if (_projectAnalyzer == null)
            {
                lock (_compileLock)
                {
                    AnalyzerManager analyzerManager = string.IsNullOrEmpty(_engine.SolutionFilePath)
                        ? new AnalyzerManager(_engine.LoggerFactory)
                        : new AnalyzerManager(_engine.SolutionFilePath, _engine.LoggerFactory);
                    _projectAnalyzer = analyzerManager.GetProject(_engine.ProjectFilePath);
                    _buildProject = _projectAnalyzer.Load();
                    _projectInstance = _projectAnalyzer.Compile();
                }
            }
        }

        private void Analyze()
        {
            if(_workspace == null)
            {
                Compile();
                lock (_analyzeLock)
                {
                    _workspace = new AdhocWorkspace();
                    _analysisProject = _projectAnalyzer.AddToWorkspace(_workspace);
                }
            }
        }

        public Microsoft.Build.Evaluation.Project Build
        {
            get
            {
                if(_buildProject == null)
                {
                    Compile();
                }
                return _buildProject;
            }
        }

        public ProjectInstance Instance
        {
            get
            {
                if (_projectInstance == null)
                {
                    Compile();
                }
                return _projectInstance;
            }
        }

        public Workspace Workspace
        {
            get
            {
                if(_workspace == null)
                {
                    Analyze();
                }
                return _workspace;
            }
        }

        public Microsoft.CodeAnalysis.Project Analysis
        {
            get
            {
                if(_analysisProject == null)
                {
                    Analyze();
                }
                return _analysisProject;
            }
        }
    }
}
