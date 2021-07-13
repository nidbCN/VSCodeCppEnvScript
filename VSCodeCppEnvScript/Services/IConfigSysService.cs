using System;
using System.Collections.Generic;
using System.Text;

namespace VSCodeCppEnvScript.Services
{
    public interface IConfigSysService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">MinGW bin path.</param>
        /// <returns></returns>
        public void ConfigEnvVariables(string path);
    }
}
