using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace TranslationLib
{
    public enum EnabledLanguages
    {
        Russian,
        English
    }


    public class Translation
    {
        private EnabledLanguages currentLanguage = EnabledLanguages.Russian;
        public EnabledLanguages Language 
        { 
            get { return this.currentLanguage; }
        }

        public string PathToLanguageFiles = "scannerLanguages";
        public static Dictionary<string, string> TableRef = null;

        public Dictionary<string, string> Table = null;

        public Translation()
        {
            this.Table = new Dictionary<string, string>();
            this.InitTranslationTable();
            //this.FirstSetLanguage();
            //this.FirstSetLanguageByLocalization();
        }


        //protected internal void FirstSetLanguageByForm()
        //{
        //    if (Properties.Settings.Default.IsFirstStart)
        //    //if(true)
        //    {
        //        FirstSelectLanguage firstSelect = new FirstSelectLanguage();
        //        firstSelect.StartPosition = FormStartPosition.CenterParent;
        //        if (firstSelect.ShowDialog() == DialogResult.OK)
        //        {
        //            //this.SetLanguage(EnabledLanguages.Russian);
        //            Properties.Settings.Default.LanguageId = 0;
        //        }
        //        else
        //            Properties.Settings.Default.LanguageId = 1;
        //        //this.SetLanguage(EnabledLanguages.English);

        //        Properties.Settings.Default.IsFirstStart = false;
        //        Properties.Settings.Default.Save();
        //    }
        //}

        public static EnabledLanguages GetLanguageByLocalization()
        {
            EnabledLanguages language;
            if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ru")
                language = EnabledLanguages.Russian;
            else
                language =  EnabledLanguages.English;
            return language;
        }

        public void SetLanguage(EnabledLanguages language)
        {
            string path = string.Empty;

            this.currentLanguage = language;

            switch (language)
            {
                case EnabledLanguages.Russian:
                    path = PathToLanguageFiles + @"\language_ru.xml";
                    break;
                case EnabledLanguages.English:
                    path = PathToLanguageFiles + @"\language_en.xml";
                    break;
                default:
                    path = PathToLanguageFiles + @"\language_ru.xml";
                    break;
            }

            FileInfo languageFile = new FileInfo(path);

            if (System.IO.File.Exists(languageFile.FullName))
                ReadFromXML(path);
            else
                SaveToXML(languageFile.FullName);

            TableRef = this.Table;
        }

        private void InitTranslationTable()
        {
            Table.Clear();
            // Main
            Table.Add("MainForm", "Сканнер");
            Table.Add("tabPageMeshure", "Результаты измерений");
        }

        public void SaveToXML(string fileName)
        {
            SerializeTest sTest = new SerializeTest();
            sTest.Map_list = this.Table;

            SerializeTest.Serialize(fileName, sTest);
            SerializeTest ReadTest = SerializeTest.DeSerialize(fileName);
        }

        public void ReadFromXML(string fileName)
        {
            SerializeTest ReadTest = SerializeTest.DeSerialize(fileName);
            this.Table = ReadTest.Map_list;
        }

        public bool SaveAsJSON(string filePath)
        {
            try
            {
                string dict = MyDictionaryToJson(this.Table);
                System.IO.File.WriteAllText(filePath, dict);
            }
            catch (Exception ex) { return false; }

            return true;
        }

        private string MyDictionaryToJson(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join("," + Environment.NewLine, entries) + "}";
        }

    }

    static public class XmlDictionarySerializer<A, B>
    {
        public static string fileName = "//SerializationOverview.xml";
        public class Item
        {
            public A Key { get; set; }
            public B Value { get; set; }
        }

        public static void WriteXML()
        {
            Item item = new Item();

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(KeyValuePair<A, B>));

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + fileName;



            System.IO.FileStream file = System.IO.File.Create(fileName);

            writer.Serialize(file, item);
            file.Close();
        }
    }

    [DataContract]
    public class SerializeTest
    {
        [DataMember]
        public Dictionary<string, string> Map_list { set; get; }

        public SerializeTest()
        {
            Map_list = new Dictionary<string, string>();
        }

        private static DataContractSerializer serializer;

        public static void Serialize(string filePath, SerializeTest sTest)
        {
            serializer = new DataContractSerializer(typeof(SerializeTest));

            using (var writer = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                serializer.WriteObject(writer, sTest);
            }
        }

        public static SerializeTest DeSerialize(string filePath)
        {
            serializer = new DataContractSerializer(typeof(SerializeTest));

            SerializeTest serializeTest;

            using (var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                serializeTest = serializer.ReadObject(reader) as SerializeTest;
            }

            return serializeTest;
        }
    }

}
