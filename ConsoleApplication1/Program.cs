using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Program
{

    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("文件所在目录");
            string str =  Console.ReadLine();
            string dirPath = @"E:\\A";

            // LINQ query.
            var dirs = from dir in
                           Directory.EnumerateDirectories(dirPath)
                       select dir;

            // Show results.
            foreach (var dir in dirs)
            {
                // Remove path information from string.
                Console.WriteLine("{0}",
                    dir.Substring(dir.LastIndexOf("\\") + 1));

                var files = from file in
                                Directory.EnumerateFiles(@dir, "*.pdf")
                            select file;

                // Show results.
                foreach (var file in files)
                {

                    Console.WriteLine("{0}", file);
                    chuans(file.ToString(),dir.ToString());
                }

            }
            Console.WriteLine("{0} directories found.",
                dirs.Count<string>().ToString());

            // Optionally create a List collection.
            List<string> workDirs = new List<string>(dirs);
        }
        catch (UnauthorizedAccessException UAEx)
        {
            Console.WriteLine(UAEx.Message);
        }
        catch (PathTooLongException PathEx)
        {
            Console.WriteLine(PathEx.Message);
        }
    }


    private static void chuans(string file,string  dir)
    {
        string fileName = file.Replace("\\\\","\\") ;
        string fileExtention = file.Substring(fileName.LastIndexOf(".") + 1);
        string filePath = fileName;

        //切记，使用pdf2swf.exe 打开的文件名之间不能有空格，否则会失败
        string cmdStr = @"C:pdf2swf.exe";
        string savePath = dir.Replace("\\\\","\\") ;
        savePath = savePath + ("\\");
        string sourcePath =  fileName ;
        string filename1 = fileName.Replace(savePath,"");
        string targetPath = @savePath + filename1.Substring(0, filename1.LastIndexOf(".")) + ".swf";
        //@"""" 四个双引号得到一个双引号，如果你所存放的文件所在文件夹名有空格的话，要在文件名的路径前后加上双引号，才能够成功
        // -t 源文件的路径
        // -s 参数化（也就是为pdf2swf.exe 执行添加一些窗外的参数(可省略)）
        string argsStr = "  -t " + sourcePath + " -s flashversion=9 -o " + targetPath;


        exec(cmdStr, argsStr);
    }

    private static void exec(string cmd, string args)
    {
        using (Process p = new Process())
        {
            ProcessStartInfo psi = new ProcessStartInfo(cmd, args.Replace("\"", ""));
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();

        }
    }
}