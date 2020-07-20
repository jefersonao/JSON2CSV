using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSON2CSV
{
    public class Converter
    {
        DirectoryInfo FolderJson = new DirectoryInfo(@"c:\FilesJson\Input");
        private static string pathJson = @"c:\FilesJson\Input";
        private static string pathCsv = @"c:\FilesJson\Ouput\";
        private static string pathReport = @"c:\FilesJson\Report.txt";

        public void ConvertJson2CSV()
        {
            FileInfo[] Files = FolderJson.GetFiles("*.json", SearchOption.TopDirectoryOnly);

            //Verificar Diretorios
            if (!Directory.Exists(@"c:\FilesJson\"))
            {
                Directory.CreateDirectory(pathJson);
            }

            foreach (FileInfo j in Files)
            {
                try
                {
                    string json = j.OpenText().ReadToEnd();
                    CleanAndConvertJson(json, j.Name.Replace(j.Extension, ""));
                }
                catch (Exception e)
                {
                    //Logar Erro
                    if (File.Exists(pathReport))
                    {
                        File.AppendAllTextAsync(pathReport, string.Format(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - O arquivo {0} possui especificou o seguinte(s) Erros: {1}" + Environment.NewLine, j.FullName, e.InnerException != null ? e.InnerException.Message : e.Message)
                            + string.Format(" - Valide o:", "https://jsonformatter.curiousconcept.com/"));
                    }
                    else
                    {
                        using (var tw = new StreamWriter(pathReport, true))
                        {
                            tw.WriteLine(pathReport, string.Format(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - O arquivo {0} possui especificou o seguinte(s) Erros: {1}" + Environment.NewLine, j.FullName, e.InnerException != null ? e.InnerException.Message : e.Message)
                                 + string.Format(" - Valide o:", "https://jsonformatter.curiousconcept.com/"));
                        }
                    }
                }
            }

            MessageBox.Show("Finalizado");
        }

        private void CleanAndConvertJson(string json, string fName)
        {

            List<string> ItnsJson = new List<string>();
            List<string> HeaderCsv = new List<string>();
            List<string> ItnsCsv = new List<string>();


            //Verificar se o arquivo tem mais de uma entidade
            if (json.IndexOf("[") > 0 || json.IndexOf("]") > 0)
                throw new Exception("Multiplas entidades não suportado.");

            //Verificar se o arquivo tem entidades aninhadas
            if (json.IndexOf("[") > 0 || json.IndexOf("},") > 0)
                throw new Exception("Aninhamentos e/ou multiplos Elementos não suportados.");

            //Manter somente itens da entidade
            if (json.Contains("[") || json.Contains("]"))
                json = json.Substring(json.IndexOf("["), json.LastIndexOf("]")).Trim();

            ItnsJson = json.Split(",").ToList();

            foreach (string js in ItnsJson)
            {
                string j = js.Replace("\r\n", "").Trim();

                List<string> itns = new List<string>();

                string header = j.Substring(0, j.IndexOf(":"));
                string itn = j.Substring(j.IndexOf(':'));

                //Verificar se coluna ja existe
                if (!HeaderCsv.Contains(RemoveCharacters(header)))
                    HeaderCsv.Add(RemoveCharacters(header));

                //Adicionar Itens a coluna
                ItnsCsv.Add(RemoveCharacters(itn) + ";");
            };

            //Criando Header Definitivo
            string headerDef = String.Join(";", HeaderCsv.ToArray());
            CreateCsv(headerDef, ItnsCsv, fName);

        }


        public bool CreateCsv(string header, List<string> itnsCsv, string fName)
        {
            try
            {
                //Escrever CSV
                if (!Directory.Exists(@"c:\FilesJson\Output"))
                {
                    Directory.CreateDirectory(pathCsv);
                }

                string fileCsvFinal = string.Format("{0}{1}{2}", pathCsv, fName, ".csv");

                if (File.Exists(fileCsvFinal))
                {
                    File.Delete(fileCsvFinal);
                }

                File.AppendAllText(fileCsvFinal, header + Environment.NewLine);

                foreach (string s in itnsCsv)
                {
                    File.AppendAllText(fileCsvFinal, s);
                }

                //Quebrar linha no csv
                File.AppendAllText(fileCsvFinal, Environment.NewLine);

                //Criar Arquivo de Log
                if (File.Exists(pathReport))
                {
                    File.AppendAllText(pathReport, string.Format(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - Arquivo {0} Processado com êxito,", fName + ".json") + Environment.NewLine);
                }
                else
                {
                    using (var tw = new StreamWriter(pathReport, true))
                    {
                        tw.WriteLine(string.Format(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - Arquivo {0} Processado com êxito,", fName + ".json") + Environment.NewLine);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static string RemoveCharacters(string text)
        {
            text = text.Replace("\"", "");
            text = text.Replace(":", "");
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            return text.Trim();
        }

    }
}
