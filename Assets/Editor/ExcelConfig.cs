using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;
using UnityEditor;

public class ExcelConfig
{
    public static readonly string excelFolderPath = Application.dataPath + "/Tables/";
    public static readonly string assetPath = "Assets/Resources/DataAssets/";

    public class ExcelTool
    {
        public static Dialogue[] ImportLinesWithExcel(string filePath)
        {
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

            //the first line in excel makes no sence
            Dialogue[] dialogues = new Dialogue[rowNum - 1];
            for (int i = 1; i < rowNum; i++)
            {
                Dialogue dialogue = new Dialogue();

                //read line info
                dialogue.plotID = int.Parse(collect[i][0].ToString());
                dialogue.index = int.Parse(collect[i][1].ToString());
                dialogue.nextIndex = int.Parse(collect[i][2].ToString());
                dialogue.text = collect[i][3].ToString();
                dialogue.audio = Resources.Load<AudioClip>(collect[i][4].ToString());
                dialogues[i - 1] = dialogue;
            }

            return dialogues;
        }


        static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            //read first table
            columnNum = result.Tables[0].Columns.Count;
            rowNum = result.Tables[0].Rows.Count;
            return result.Tables[0].Rows;
        }
    }


    public class ExcelBuild : Editor
    {

        [MenuItem("CustomEditor/ImportDialogues")]
        public static void CreateLinesAsset()
        {
            PlotManager manager = ScriptableObject.CreateInstance<PlotManager>();

            manager.dialogues = ExcelTool.ImportLinesWithExcel(ExcelConfig.excelFolderPath + "Dialogue.xlsx");

            if (!Directory.Exists(ExcelConfig.assetPath))
            {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }

            //asset file should be named with "Assets/..." in the beginning
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Dialogues");
            //generate asset file
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}


