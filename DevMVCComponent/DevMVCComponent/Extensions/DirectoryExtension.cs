using System;
using System.IO;

namespace DevMvcComponent.Extensions {
    public class DirectoryExtension {
        /// <summary>
        /// Gets "C:\Program Files (x86)\" or "C:\Program Files\" based on operating system architecture.
        /// </summary>
        /// <returns>Returns "C:\Program Files (x86)\" or "C:\Program Files\" based on operating system architecture.</returns>
        public static string GetProgramFilesX86Directory() {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Gets "C:\Program Files\" irrelevant to the operating system architecture.
        /// </summary>
        /// <returns>Returns "C:\Program Files\" irrelevant to the operating system architecture.</returns>
        public static string GetProgramFilesDirectory() {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + Path.DirectorySeparatorChar;
        }
    }
}