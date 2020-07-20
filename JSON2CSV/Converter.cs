using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JSON2CSV
{
    public class Converter
    {
        DirectoryInfo FolderJson = new DirectoryInfo(@"c:\FilesJson\Input");
        private static string pathJson = @"c:\FilesJson\Input";
        private static string pathReport = @"c:\FilesJson\Report.txt";

        static void Main(string[] args)
        {
            var json = @"{
                       ""employees"": [
                       { ""firstName"":""John"" , ""lastName"":""Doe"" },
                       { ""firstName"":""Anna"" , ""lastName"":""Smith"" },
                       { ""firstName"":""Peter"" , ""lastName"":""Jones"" }
                       ]
                       }";
            //ConvertJson2CSV();

        }

        public async void ConvertJson2CSV()
        {
            FileInfo[] Files = FolderJson.GetFiles("*.json", SearchOption.TopDirectoryOnly);

            //Verificar Diretorios
            if (!Directory.Exists(@"c:\FilesJson\"))
            {
                Directory.CreateDirectory(pathJson);
            }

            //Criar Arquivo de Log
            if (File.Exists(pathReport))
            {
                File.AppendAllText(pathReport, "" + Environment.NewLine);
            }
            else
            {
                using (var tw = new StreamWriter(pathReport, true))
                {
                    tw.WriteLine("" + Environment.NewLine);
                }
            }

            foreach (FileInfo j in Files)
            {
                try
                {
                    string json = j.OpenText().ReadToEnd();
                    json = CleanAndConvertJson(json);
                }
                catch (Exception e)
                {
                    //Logar Erro
                    if (File.Exists(pathReport))
                    {
                        File.AppendAllText(pathReport, string.Format("O arquivo {0} possui especificou o seguinte(s) Erros: {1}" + Environment.NewLine, j.FullName, e.InnerException != null ? e.InnerException.Message : e.Message));
                    }
                    else
                    {
                        using (var tw = new StreamWriter(pathReport, true))
                        {
                            tw.WriteLine(pathReport, string.Format("O arquivo {0} possui especificou o seguinte(s) Erros: {1}" + Environment.NewLine, j.FullName, e.InnerException != null ? e.InnerException.Message : e.Message));
                        }
                    }
                }
            }
        }

        private string CleanAndConvertJson(string json)
        {
            string Csvreturn = "", headerCsv = "", IntsCsv = "";

            List<string> ItnsJson = new List<string>();
            List<string> HeaderCsv = new List<string>();
            List<string> ItnsCsv = new List<string>();


            //Verificar se é aninahdo
            // if (json.Split(",[").ToArray().Count() > 1 || json.Split("],").ToArray().Count() > 1)
            //   throw new Exception("Não é suportado arquivos aninhados.");
            //

            //Manter somente itens da entidade
            json = json.Substring(json.IndexOf("["), json.LastIndexOf("]"));


            Dictionary<string, string> itns = new Dictionary<string, string>();
            foreach (string j in ItnsJson)
            {
                //Tirar Espaços para facilitar a Limpeza
                string nTextJson = j.Trim();

                nTextJson.Contains("");

            }




            return Csvreturn;
        }

        private string CreateHeaders(string json)
        {
            string Jreturn = "";


            return Jreturn;
        }


        private string CreateItens(string json)
        {
            string Jreturn = "";


            return Jreturn;
        }

        public string CreateCsv()
        {
            string CsvReturn = "";
            return CsvReturn;
        }

    }
}
