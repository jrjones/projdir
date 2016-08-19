using System;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {
        //TODO - projectsDirectory should be a configuration option. If no config file exists at launch, ask the user for the directory location and create a new config.
        private static string projectsDirectory = "/Users/jrj/Dropbox (Personal)/@projects/";
        private static string projectName = "";

        public static void Main(string[] args)
        {
            projectName = args[0];
            Console.WriteLine("Creating project directory for " + projectName);

            if (Directory.Exists(projectsDirectory + projectName))
            {
                Console.WriteLine("Project already exists. No directory created.");
            }
            else
            {
                // Create project directory 
                string projdir = projectsDirectory + projectName + "/";
                Console.WriteLine("Creating directory... \t" + projdir);
                Directory.CreateDirectory(projdir);

                //Create subdirectories
                Console.WriteLine("Creating subdirs... \tArchived and gfx");
                Directory.CreateDirectory(projdir + "Archived");
                Directory.CreateDirectory(projdir + "gfx");

                // Creating FoldingText file
                Console.WriteLine("Creating FT file... \t" + projectName);
                CreateFtFile(projdir,projectName);

                // Copy basic mindmap file
                Console.WriteLine("Creating MindMap... \t" + "ProjectMap.mindnode");
                string mindMapPath = projectsDirectory + "@Archived/Sample Project/ProjectMap.mindnode";
                DirectoryCopy(mindMapPath,projdir + "ProjectMap.mindnode", true);
            }

        }

        private static void CreateFtFile(string dir, string name)
        {
                // Creates a FoldingText file with basic default structure and content
                StreamWriter ftFile = File.CreateText(dir + name + ".ft");
                ftFile.WriteLine("# " + name);
                ftFile.WriteLine("");
                ftFile.WriteLine("## Outcome: ");
                ftFile.WriteLine("## Timeline: ");
                ftFile.Flush();
        }


        // Copies source directory to destination directory, optionally recursive.
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
