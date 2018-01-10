using System;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.CSharp;

namespace DeVes.Extensions.CodeCompiler
{
    public class RuntimeCompiler : IDisposable
    {
        private readonly CompilerParameters m_compilerParameters;
        private readonly CSharpCodeProvider m_provider;

        private CompilerResults m_compilerResults;


        public ComileAtRuntimeInfo[] Messages { get; private set; }
        public bool HasErrors => Messages?.Any(p => p.IsError) ?? false;
        public bool HasWarnings => Messages?.Any(p => p.IsWarning) ?? false;


        public RuntimeCompiler()
            : this("System.dll")
        {
        }
        public RuntimeCompiler(params string[] referenceAssemblies)
        {
            m_provider = new CSharpCodeProvider();

            m_compilerParameters = new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            m_compilerParameters.ReferencedAssemblies.AddRange(referenceAssemblies);
        }


        public bool CompileCode(params string[] codeLines)
        {
            Messages = new ComileAtRuntimeInfo[0];

            m_compilerResults = m_provider?.CompileAssemblyFromSource(m_compilerParameters, codeLines);

            if (m_compilerResults == null)
            {
                Messages = new ComileAtRuntimeInfo[]
                {
                    new RuntimeError("No compiler result!", true)
                };
                return false;
            }

            Messages =
                m_compilerResults.Errors.Cast<System.CodeDom.Compiler.CompilerError>()
                    .Select(p => new CompilerError(p) as ComileAtRuntimeInfo)
                    .ToArray();

            return !m_compilerResults.Errors.HasErrors;
        }
        public bool CompileFromFile(params string[] fileNames)
        {
            Messages = new ComileAtRuntimeInfo[0];

            m_compilerResults = m_provider?.CompileAssemblyFromFile(m_compilerParameters, fileNames);

            if (m_compilerResults == null)
            {
                Messages = new ComileAtRuntimeInfo[]
                {
                    new RuntimeError("No compiler result!", true)
                };
                return false;
            }

            Messages =
                m_compilerResults.Errors.Cast<System.CodeDom.Compiler.CompilerError>()
                    .Select(p => new CompilerError(p) as ComileAtRuntimeInfo)
                    .ToArray();

            return !m_compilerResults.Errors.HasErrors;
        }


        public InvokeSummery Invoke(string boardingNamespace, string boardingClass, string boardingMethod, object instanceObject, params object[] parameters)
        {
            if (m_compilerResults.Errors.HasErrors)
            {
                return new InvokeSummery { IsSuccessfull = false };
            }

            var _bordingNamespaceClass = boardingNamespace + "." + boardingClass;

            var _modules = m_compilerResults?.CompiledAssembly.GetModules();

            var _module = _modules.FirstOrDefault(p => p.GetType(_bordingNamespaceClass) != null);
            var _classType = _module?.GetType(_bordingNamespaceClass);

            if (_classType == null)
            {
                Messages = new ComileAtRuntimeInfo[]
                {
                    new RuntimeError($"'{_bordingNamespaceClass}' not found in code!", true)
                };
                return new InvokeSummery { IsSuccessfull = false };
            }

            var _method = _classType.GetMethod(boardingMethod);

            if (_method == null)
            {
                Messages = new ComileAtRuntimeInfo[]
                {
                    new RuntimeError($"'{boardingMethod}' not found in {_bordingNamespaceClass}!", true)
                };
                return new InvokeSummery { IsSuccessfull = false };
            }

            var _returnObject = _method.Invoke(instanceObject, parameters);

            return new InvokeSummery
            {
                IsSuccessfull = true,
                Result = _returnObject,
                Parameter = parameters
            };
        }


        public void Dispose()
        {
            m_provider?.Dispose();
        }
    }
}
